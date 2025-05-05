using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using RestaurantSimulator.Models;
using RestaurantSimulator.Services;

namespace RestaurantSimulator.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    [ObservableProperty]
    private ObservableCollection<KitchenStationViewModel> _kitchenStations = new();

    [ObservableProperty]
    private ObservableCollection<RecipeItemViewModel> _availableRecipes = new();

    [ObservableProperty]
    private bool _isSimulationRunning;

    private readonly DataProviderService _dataProviderService;
    private readonly JsonDataService _jsonDataService;
    private readonly MealPreparationService _mealPreparationService;

    [RelayCommand]
    private async Task ToggleSimulation()
    {
        if (IsSimulationRunning)
        {
            StopSimulation();
        }
        else
        {
            await Task.Run(StartSimulation);
        }
    }

    [RelayCommand]
    private void AddRecipeToQueue(Recipe recipe)
    {
        var recipeReal = recipe.Clone();
        Console.WriteLine($"[MainWindowViewModel] AddRecipeToQueue called for recipe: {recipeReal?.Name}");
        if (recipe != null && !recipeReal.IsInProgress && !recipeReal.IsCompleted)
        {
            Console.WriteLine($"[MainWindowViewModel] Adding recipe {recipeReal.Name} to queue");
            _mealPreparationService.AddRecipe(recipeReal);
            recipeReal.IsInProgress = true;
        }
        else
        {
            Console.WriteLine($"[MainWindowViewModel] Cannot add recipe {recipeReal?.Name} - InProgress: {recipeReal?.IsInProgress}, Completed: {recipeReal?.IsCompleted}");
        }
    }

    public MainWindowViewModel()
    {
        _jsonDataService = new JsonDataService("ExerciseJSON.json");
        _dataProviderService = new DataProviderService(_jsonDataService);
        _mealPreparationService = new MealPreparationService(3); // Initialize with 3 stations
        
        InitializeKitchenStationsViewModels();
        LoadRecipes();
        SubscribeToMealServiceEvents();
    }

    private void SubscribeToMealServiceEvents()
    {
        Console.WriteLine("[MainWindowViewModel] Subscribing to meal service events");
        _mealPreparationService.RecipeProgressUpdated += OnRecipeProgressUpdated;
        _mealPreparationService.RecipeCompleted += OnRecipeCompleted;
    }

    private void OnRecipeProgressUpdated(object? sender, RecipeProgressEventArgs e)
    {
        var recipe = e.Recipe;
        Console.WriteLine($"[MainWindowViewModel] Recipe progress updated: {recipe.Name}");
        
        var stationModel = _mealPreparationService.KitchenStations.FirstOrDefault(st => st.CurrentRecipe == recipe);
        if (stationModel != null)
        {
            Console.WriteLine($"[MainWindowViewModel] Found station model: {stationModel.Name}");
            var stationVM = KitchenStations.FirstOrDefault(vm => vm.KitchenStation.Id == stationModel.Id);
            if (stationVM != null)
            {
                Console.WriteLine($"[MainWindowViewModel] Found station view model: {stationVM.Station.Name}, updating progress");
                stationVM.UpdateProgress();
            }
            else
            {
                Console.WriteLine($"[MainWindowViewModel] Could not find station view model for station {stationModel.Name}");
            }
        }
        else
        {
            Console.WriteLine($"[MainWindowViewModel] Could not find station model for recipe {recipe.Name}");
        }
    }

    private void OnRecipeCompleted(object? sender, RecipeEventArgs e)
    {
        var recipe = e.Recipe;
        Console.WriteLine($"[MainWindowViewModel] Recipe completed: {recipe.Name}");
        
        var stationModel = _mealPreparationService.KitchenStations.FirstOrDefault(st => st.CurrentRecipe == recipe);
        if (stationModel != null)
        {
            Console.WriteLine($"[MainWindowViewModel] Found station model: {stationModel.Name}");
            var stationVM = KitchenStations.FirstOrDefault(vm => vm.KitchenStation.Id == stationModel.Id);
            if (stationVM != null)
            {
                Console.WriteLine($"[MainWindowViewModel] Found station view model: {stationVM.Station.Name}, clearing recipe");
                stationVM.ClearCurrentRecipe();
            }
            else
            {
                Console.WriteLine($"[MainWindowViewModel] Could not find station view model for station {stationModel.Name}");
            }
        }
        else
        {
            Console.WriteLine($"[MainWindowViewModel] Could not find station model for recipe {recipe.Name}");
        }
    }

    private void InitializeKitchenStationsViewModels()
    {
        Console.WriteLine("[MainWindowViewModel] Initializing kitchen station view models");
        var kitchenStations = _mealPreparationService.KitchenStations;
        foreach (var station in kitchenStations)
        {
            Console.WriteLine($"[MainWindowViewModel] Creating view model for station {station.Name}");
            KitchenStations.Add(new KitchenStationViewModel(station));
        }
    }

    private async void LoadRecipes()
    {
        Console.WriteLine("[MainWindowViewModel] Loading recipes");
        try
        {
            await _dataProviderService.LoadRecipesAsync();
            var recipes = _dataProviderService.GetAllRecipes();
            AvailableRecipes.Clear();
            foreach (var recipe in recipes)
            {
                Console.WriteLine($"[MainWindowViewModel] Adding recipe to available recipes: {recipe.Name}");
                AvailableRecipes.Add(new RecipeItemViewModel(recipe, AddRecipeToQueueCommand));
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[MainWindowViewModel] Error loading recipes: {ex.Message}");
        }
    }

    public async Task StartSimulation()
    {
        Console.WriteLine("[MainWindowViewModel] Starting simulation");
        IsSimulationRunning = true;
        ResetAllRecipes();
    }

    private void ResetAllRecipes()
    {
        Console.WriteLine("[MainWindowViewModel] Resetting all recipes");
        foreach (var recipeViewModel in AvailableRecipes)
        {
            var recipe = recipeViewModel.Recipe;
            Console.WriteLine($"[MainWindowViewModel] Resetting recipe: {recipe.Name}");
            recipe.IsInProgress = false;
            recipe.IsCompleted = false;
            recipe.CurrentStepIndex = 0;
            foreach (var step in recipe.Steps)
            {
                step.IsCompleted = false;
                step.StartTime = null;
                step.EndTime = null;
            }
        }
    }

    public void StopSimulation()
    {
        Console.WriteLine("[MainWindowViewModel] Stopping simulation");
        IsSimulationRunning = false;
        _mealPreparationService.StopProcessing();
        
        foreach (var station in KitchenStations)
        {
            Console.WriteLine($"[MainWindowViewModel] Clearing recipe from station: {station.Station.Name}");
            station.ClearCurrentRecipe();
        }
    }
}
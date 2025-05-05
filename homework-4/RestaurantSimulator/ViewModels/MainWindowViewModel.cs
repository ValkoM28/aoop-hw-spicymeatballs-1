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
            await StartSimulation();
        }
    }

    [RelayCommand]
    private void AddRecipeToQueue(Recipe recipe)
    {
        Console.WriteLine($"AddRecipeToQueue called for recipe: {recipe?.Name}"); // Debug log
        if (recipe != null && !recipe.IsInProgress && !recipe.IsCompleted)
        {
            Console.WriteLine($"Adding recipe {recipe.Name} to queue"); // Debug log
            _mealPreparationService.AddRecipe(recipe);
            recipe.IsInProgress = true;
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
        _mealPreparationService.RecipeProgressUpdated += OnRecipeProgressUpdated;
        _mealPreparationService.RecipeCompleted += OnRecipeCompleted;
    }

    private void OnRecipeProgressUpdated(object? sender, RecipeProgressEventArgs e)
    {
        var recipe = e.Recipe;
        var station = KitchenStations.FirstOrDefault(s => s.KitchenStation.CurrentRecipe == recipe);
        if (station != null)
        {
            station.UpdateProgress();
        }
    }

    private void OnRecipeCompleted(object? sender, RecipeEventArgs e)
    {
        var recipe = e.Recipe;
        var station = KitchenStations.FirstOrDefault(s => s.KitchenStation.CurrentRecipe == recipe);
        if (station != null)
        {
            station.ClearCurrentRecipe();
        }
    }

    private void InitializeKitchenStationsViewModels()
    {
        var kitchenStations = _mealPreparationService.KitchenStations;
        foreach (var station in kitchenStations)
        {
            KitchenStations.Add(new KitchenStationViewModel(station));
        }
    }

    private async void LoadRecipes()
    {
        try
        {
            await _dataProviderService.LoadRecipesAsync();
            var recipes = _dataProviderService.GetAllRecipes();
            AvailableRecipes.Clear();
            foreach (var recipe in recipes)
            {
                AvailableRecipes.Add(new RecipeItemViewModel(recipe, AddRecipeToQueueCommand));
            }
        }
        catch (Exception ex)
        {
            // TODO: Add proper error handling and logging
            Console.WriteLine($"Error loading recipes: {ex.Message}");
        }
    }

    public async Task StartSimulation()
    {
        IsSimulationRunning = true;
        ResetAllRecipes();
    }

    private void ResetAllRecipes()
    {
        foreach (var recipeViewModel in AvailableRecipes)
        {
            var recipe = recipeViewModel.Recipe;
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
        IsSimulationRunning = false;
        _mealPreparationService.StopProcessing();
        
        foreach (var station in KitchenStations)
        {
            station.ClearCurrentRecipe();
        }
    }
}
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using RestaurantSimulator.Models;
using RestaurantSimulator.Services;
using RestaurantSimulator.Utilities;

namespace RestaurantSimulator.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    [ObservableProperty]
    private ObservableCollection<KitchenStationViewModel> _kitchenStations = new();

    [ObservableProperty]
    private ObservableCollection<Recipe> _availableRecipes = new();

    [ObservableProperty]
    private bool _isSimulationRunning;

    private readonly DataProviderService _dataProviderService;
    private readonly JsonDataService _jsonDataService;

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

    public MainWindowViewModel()
    {
        _jsonDataService = new JsonDataService(Constants.JsonFilePath);
        _dataProviderService = new DataProviderService(_jsonDataService);
        
        InitializeKitchenStations();
        LoadRecipes();
    }

    private void InitializeKitchenStations()
    {
        KitchenStations.Add(new KitchenStationViewModel(new KitchenStation
        {
            Id = 1,
            Name = "Station 1",
            AvailableEquipment = new List<string> { "Oven", "Rolling Pin", "Mixing Bowl" }
        }));

        KitchenStations.Add(new KitchenStationViewModel(new KitchenStation
        {
            Id = 2,
            Name = "Station 2",
            AvailableEquipment = new List<string> { "Stove", "Saucepan", "Colander" }
        }));

        KitchenStations.Add(new KitchenStationViewModel(new KitchenStation
        {
            Id = 3,
            Name = "Station 3",
            AvailableEquipment = new List<string> { "Grill", "Bowl", "Brush" }
        }));
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
                AvailableRecipes.Add(recipe);
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
        // Simulation logic will be implemented here
    }

    public void StopSimulation()
    {
        IsSimulationRunning = false;
    }
}
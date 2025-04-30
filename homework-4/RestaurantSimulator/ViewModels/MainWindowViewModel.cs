using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using RestaurantSimulator.Models;

namespace RestaurantSimulator.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    [ObservableProperty]
    private ObservableCollection<KitchenStationViewModel> _kitchenStations = new();

    [ObservableProperty]
    private ObservableCollection<Recipe> _availableRecipes = new();

    [ObservableProperty]
    private bool _isSimulationRunning;

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

    private void LoadRecipes()
    {
        // This will be replaced with actual JSON loading
        var recipe = new Recipe
        {
            Name = "Margherita Pizza",
            Difficulty = "Medium",
            Equipment = new List<string> { "Oven", "Rolling Pin", "Mixing Bowl" },
            Steps = new List<RecipeStep>
            {
                new() { Step = "Preheat the oven", Duration = 5 },
                new() { Step = "Prepare the dough", Duration = 15 },
                new() { Step = "Spread tomato sauce", Duration = 5 },
                new() { Step = "Add cheese and basil", Duration = 3 },
                new() { Step = "Bake the pizza", Duration = 15 }
            }
        };
        AvailableRecipes.Add(recipe);
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
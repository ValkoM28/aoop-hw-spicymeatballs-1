using System;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using RestaurantSimulator.Models;

namespace RestaurantSimulator.ViewModels;

public partial class KitchenStationViewModel : ViewModelBase
{
    [ObservableProperty]
    private KitchenStation _station;

    [ObservableProperty]
    private RecipeViewModel? _currentRecipeViewModel;

    public KitchenStationViewModel(KitchenStation station)
    {
        _station = station;
        // Subscribe to changes in the station's CurrentRecipe
        station.PropertyChanged += (s, e) =>
        {
            if (e.PropertyName == nameof(KitchenStation.CurrentRecipe))
            {
                if (station.CurrentRecipe != null)
                {
                    CurrentRecipeViewModel = new RecipeViewModel(station.CurrentRecipe);
                }
                else
                {
                    CurrentRecipeViewModel = null;
                }
            }
        };
    }

    public KitchenStation KitchenStation => Station;

    public void UpdateProgress()
    {
        if (CurrentRecipeViewModel != null)
        {
            CurrentRecipeViewModel.UpdateProgress();
        }
    }

    public void ClearCurrentRecipe()
    {
        Station.CurrentRecipe = null;
        CurrentRecipeViewModel = null;
    }

    public async Task AssignRecipe(Recipe recipe)
    {
        if (!Station.IsAvailable) return;

        Station.CurrentRecipe = recipe;
        CurrentRecipeViewModel = new RecipeViewModel(recipe);
        await CurrentRecipeViewModel.StartPreparation();
        Station.CurrentRecipe = null;
        CurrentRecipeViewModel = null;
    }
} 
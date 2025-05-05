using System;
using System.Threading.Tasks;
using System.Threading;
using CommunityToolkit.Mvvm.ComponentModel;
using RestaurantSimulator.Models;

namespace RestaurantSimulator.ViewModels;

public partial class KitchenStationViewModel : ViewModelBase
{
    [ObservableProperty]
    private KitchenStation _station;

    [ObservableProperty]
    private RecipeViewModel? _currentRecipeViewModel;

    private readonly SemaphoreSlim _recipeLock = new SemaphoreSlim(1, 1);

    public KitchenStationViewModel(KitchenStation station)
    {
        _station = station;
        // Subscribe to changes in the station's CurrentRecipe
        station.PropertyChanged += (s, e) =>
        {
            if (e.PropertyName == nameof(KitchenStation.CurrentRecipe))
            {
                UpdateCurrentRecipeViewModel();
            }
        };
    }

    private void UpdateCurrentRecipeViewModel()
    {
        if (Station.CurrentRecipe != null)
        {
            CurrentRecipeViewModel = new RecipeViewModel(Station.CurrentRecipe);
            OnPropertyChanged(nameof(CurrentRecipeViewModel));
        }
        else
        {
            CurrentRecipeViewModel = null;
            OnPropertyChanged(nameof(CurrentRecipeViewModel));
        }
    }

    public KitchenStation KitchenStation => Station;

    public void UpdateProgress()
    {
        if (CurrentRecipeViewModel != null)
        {
            CurrentRecipeViewModel.UpdateProgress();
        }
    }

    public async Task ClearCurrentRecipe()
    {
        await _recipeLock.WaitAsync();
        try
        {
            if (CurrentRecipeViewModel != null)
            {
                await CurrentRecipeViewModel.CancelPreparation();
            }
            Station.CurrentRecipe = null;
            CurrentRecipeViewModel = null;
        }
        finally
        {
            _recipeLock.Release();
        }
    }

    public async Task AssignRecipe(Recipe recipe)
    {
        await _recipeLock.WaitAsync();
        try
        {
            if (!Station.IsAvailable)
            {
                throw new InvalidOperationException("Station is not available");
            }

            if (recipe.IsInProgress)
            {
                throw new InvalidOperationException("Recipe is already in progress");
            }

            Station.CurrentRecipe = recipe;
            CurrentRecipeViewModel = new RecipeViewModel(recipe);
            await CurrentRecipeViewModel.StartPreparation();
            
            // Only clear if the recipe completed successfully
            if (recipe.IsCompleted)
            {
                Station.CurrentRecipe = null;
                CurrentRecipeViewModel = null;
            }
        }
        finally
        {
            _recipeLock.Release();
        }
    }
} 
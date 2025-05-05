using System;
using System.Threading.Tasks;
using System.Threading;
using CommunityToolkit.Mvvm.ComponentModel;
using RestaurantSimulator.Models;
using Avalonia.Threading;

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
        Console.WriteLine($"[KitchenStationViewModel] Creating view model for station {station.Name}");
        _station = station;
        // Subscribe to changes in the station's CurrentRecipe
        station.PropertyChanged += (s, e) =>
        {
            Console.WriteLine($"[KitchenStationViewModel] Property changed: {e.PropertyName} for station {station.Name}");
            if (e.PropertyName == nameof(KitchenStation.CurrentRecipe))
            {
                UpdateCurrentRecipeViewModel();
            }
        };
    }

    private void UpdateCurrentRecipeViewModel()
    {
        Console.WriteLine($"[KitchenStationViewModel] UpdateCurrentRecipeViewModel called for station {Station.Name}");
        Console.WriteLine($"[KitchenStationViewModel] Current recipe: {Station.CurrentRecipe?.Name ?? "null"}");
        Console.WriteLine($"[KitchenStationViewModel] Current recipe view model: {CurrentRecipeViewModel?.Recipe.Name ?? "null"}");
        
        Dispatcher.UIThread.Post(() =>
        {
            if (Station.CurrentRecipe == null)
            {
                Console.WriteLine($"[KitchenStationViewModel] Clearing recipe view model for station {Station.Name}");
                CurrentRecipeViewModel = null;
                return;
            }

            if (CurrentRecipeViewModel?.Recipe != Station.CurrentRecipe)
            {
                Console.WriteLine($"[KitchenStationViewModel] Creating new recipe view model for {Station.CurrentRecipe.Name} at station {Station.Name}");
                CurrentRecipeViewModel = new RecipeViewModel(Station.CurrentRecipe);
            }
        });
    }

    public KitchenStation KitchenStation => Station;

    public void UpdateProgress()
    {
        Console.WriteLine($"[KitchenStationViewModel] Updating progress for station {Station.Name}");
        Dispatcher.UIThread.Post(() =>
        {
            if (CurrentRecipeViewModel != null)
            {
                Console.WriteLine($"[KitchenStationViewModel] Current recipe: {CurrentRecipeViewModel.Recipe.Name}");
                CurrentRecipeViewModel.UpdateProgress();
            }
            else
            {
                Console.WriteLine($"[KitchenStationViewModel] No recipe to update for station {Station.Name}");
            }
        });
    }

    public async Task ClearCurrentRecipe()
    {
        Console.WriteLine($"[KitchenStationViewModel] Clearing recipe for station {Station.Name}");
        await _recipeLock.WaitAsync();
        try
        {
            if (CurrentRecipeViewModel != null)
            {
                Console.WriteLine($"[KitchenStationViewModel] Cancelling preparation for recipe {CurrentRecipeViewModel.Recipe.Name}");
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
        Console.WriteLine($"[KitchenStationViewModel] Attempting to assign recipe {recipe.Name} to station {Station.Name}");
        await _recipeLock.WaitAsync();
        try
        {
            if (!Station.IsAvailable)
            {
                Console.WriteLine($"[KitchenStationViewModel] Station {Station.Name} is not available");
                throw new InvalidOperationException("Station is not available");
            }

            if (recipe.IsInProgress)
            {
                Console.WriteLine($"[KitchenStationViewModel] Recipe {recipe.Name} is already in progress");
                throw new InvalidOperationException("Recipe is already in progress");
            }

            Console.WriteLine($"[KitchenStationViewModel] Assigning recipe {recipe.Name} to station {Station.Name}");
            Station.CurrentRecipe = recipe;
            CurrentRecipeViewModel = new RecipeViewModel(recipe);
            await CurrentRecipeViewModel.StartPreparation();
            
            if (recipe.IsCompleted)
            {
                Console.WriteLine($"[KitchenStationViewModel] Recipe {recipe.Name} completed, clearing station {Station.Name}");
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
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
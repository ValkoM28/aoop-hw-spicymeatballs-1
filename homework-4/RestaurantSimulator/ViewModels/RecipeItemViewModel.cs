using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using RestaurantSimulator.Models;
using System;

namespace RestaurantSimulator.ViewModels;

public partial class RecipeItemViewModel : ViewModelBase
{
    [ObservableProperty]
    private Recipe _recipe;

    public ICommand AddRecipeToQueueCommand { get; }

    public RecipeItemViewModel(Recipe recipe, ICommand addRecipeToQueueCommand)
    {
        _recipe = recipe;
        AddRecipeToQueueCommand = addRecipeToQueueCommand;
        Console.WriteLine($"Created RecipeItemViewModel for {recipe.Name}"); // Debug log
    }
} 
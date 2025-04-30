using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using RestaurantSimulator.Models;

namespace RestaurantSimulator.ViewModels;

public partial class RecipeViewModel : ViewModelBase
{
    [ObservableProperty]
    private Recipe _recipe;

    [ObservableProperty]
    private string _currentStepDescription = string.Empty;

    [ObservableProperty]
    private double _stepProgress;

    public RecipeViewModel(Recipe recipe)
    {
        _recipe = recipe;
        UpdateCurrentStep();
    }

    public async Task StartPreparation()
    {
        Recipe.IsInProgress = true;
        
        for (int i = 0; i < Recipe.Steps.Count; i++)
        {
            Recipe.CurrentStepIndex = i;
            var step = Recipe.Steps[i];
            step.StartTime = DateTime.Now;
            CurrentStepDescription = step.Step;
            
            for (int j = 0; j < step.Duration; j++)
            {
                StepProgress = (double)j / step.Duration * 100;
                await Task.Delay(1000); // Simulate 1 second per unit of duration
            }
            
            step.EndTime = DateTime.Now;
            step.IsCompleted = true;
            OnPropertyChanged(nameof(Recipe.ProgressPercentage));
        }
        
        Recipe.IsCompleted = true;
        Recipe.IsInProgress = false;
    }

    private void UpdateCurrentStep()
    {
        if (Recipe.CurrentStepIndex < Recipe.Steps.Count)
        {
            CurrentStepDescription = Recipe.Steps[Recipe.CurrentStepIndex].Step;
        }
    }
} 
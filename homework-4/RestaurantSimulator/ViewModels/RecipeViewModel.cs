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

    public void UpdateProgress()
    {
        if (Recipe.CurrentStepIndex < Recipe.Steps.Count)
        {
            var currentStep = Recipe.Steps[Recipe.CurrentStepIndex];
            CurrentStepDescription = currentStep.Step;
            
            if (currentStep.StartTime.HasValue && !currentStep.IsCompleted)
            {
                var elapsed = DateTime.Now - currentStep.StartTime.Value;
                var totalDuration = TimeSpan.FromSeconds(currentStep.Duration);
                StepProgress = Math.Min(100, (elapsed.TotalSeconds / totalDuration.TotalSeconds) * 100);
            }
            else if (currentStep.IsCompleted)
            {
                StepProgress = 100;
            }
        }
        
        OnPropertyChanged(nameof(Recipe.ProgressPercentage));
    }

    public async Task StartPreparation()
    {
        if (Recipe.IsInProgress) return;
        
        Recipe.IsInProgress = true;
        
        for (int i = 0; i < Recipe.Steps.Count; i++)
        {
            Recipe.CurrentStepIndex = i;
            var step = Recipe.Steps[i];
            step.StartTime = DateTime.Now;
            CurrentStepDescription = step.Step;
            
            for (int j = 0; j < step.Duration; j++)
            {
                if (!Recipe.IsInProgress) return; // Check if preparation was cancelled
                
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
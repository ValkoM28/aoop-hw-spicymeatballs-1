using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Threading;
using Avalonia.Threading;
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

    [ObservableProperty]
    private string _remainingTime = string.Empty;

    private CancellationTokenSource? _cancellationTokenSource;
    private readonly object _progressLock = new object();

    public RecipeViewModel(Recipe recipe)
    {
        _recipe = recipe;
        UpdateCurrentStep();
    }

    public void UpdateProgress()
    {
        lock (_progressLock)
        {
            Dispatcher.UIThread.Post(() =>
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
                        
                        var remaining = totalDuration - elapsed;
                        RemainingTime = remaining.ToString(@"mm\:ss");
                    }
                    else if (currentStep.IsCompleted)
                    {
                        StepProgress = 100;
                        RemainingTime = "Completed";
                    }
                }
                
                OnPropertyChanged(nameof(Recipe.ProgressPercentage));
            });
        }
    }

    public async Task StartPreparation()
    {
        if (Recipe.IsInProgress) return;
        
        _cancellationTokenSource = new CancellationTokenSource();
        Recipe.IsInProgress = true;
        
        try
        {
            for (int i = 0; i < Recipe.Steps.Count; i++)
            {
                if (_cancellationTokenSource.Token.IsCancellationRequested)
                {
                    Recipe.IsInProgress = false;
                    return;
                }

                Recipe.CurrentStepIndex = i;
                var step = Recipe.Steps[i];
                step.StartTime = DateTime.Now;
                CurrentStepDescription = step.Step;
                
                for (int j = 0; j < step.Duration; j++)
                {
                    if (_cancellationTokenSource.Token.IsCancellationRequested)
                    {
                        Recipe.IsInProgress = false;
                        return;
                    }
                    
                    StepProgress = (double)j / step.Duration * 100;
                    RemainingTime = TimeSpan.FromSeconds(step.Duration - j).ToString(@"mm\:ss");
                    await Task.Delay(1000, _cancellationTokenSource.Token);
                }
                
                step.EndTime = DateTime.Now;
                step.IsCompleted = true;
                OnPropertyChanged(nameof(Recipe.ProgressPercentage));
            }
            
            Recipe.IsCompleted = true;
            Recipe.IsInProgress = false;
            RemainingTime = "Completed";
        }
        catch (OperationCanceledException)
        {
            Recipe.IsInProgress = false;
            RemainingTime = "Cancelled";
        }
        finally
        {
            _cancellationTokenSource?.Dispose();
            _cancellationTokenSource = null;
        }
    }

    public async Task CancelPreparation()
    {
        _cancellationTokenSource?.Cancel();
        Recipe.IsInProgress = false;
        RemainingTime = "Cancelled";
    }

    private void UpdateCurrentStep()
    {
        if (Recipe.CurrentStepIndex < Recipe.Steps.Count)
        {
            CurrentStepDescription = Recipe.Steps[Recipe.CurrentStepIndex].Step;
        }
    }
} 
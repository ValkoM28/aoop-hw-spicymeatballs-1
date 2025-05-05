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
    private DateTime _stepStartTime;
    private int _currentStepDuration;

    public RecipeViewModel(Recipe recipe)
    {
        Console.WriteLine($"[RecipeViewModel] Creating view model for recipe: {recipe.Name}");
        _recipe = recipe;
        UpdateCurrentStep();
    }

    public void UpdateProgress()
    {
        Console.WriteLine($"[RecipeViewModel] Updating progress for recipe: {Recipe.Name}");
        lock (_progressLock)
        {
            Dispatcher.UIThread.Post(() =>
            {
                if (Recipe.CurrentStepIndex < Recipe.Steps.Count)
                {
                    var currentStep = Recipe.Steps[Recipe.CurrentStepIndex];
                    Console.WriteLine($"[RecipeViewModel] Current step: {currentStep.Step}");
                    CurrentStepDescription = currentStep.Step;
                    
                    if (currentStep.StartTime.HasValue && !currentStep.IsCompleted)
                    {
                        var elapsed = DateTime.Now - currentStep.StartTime.Value;
                        var totalDuration = TimeSpan.FromSeconds(currentStep.Duration);
                        StepProgress = Math.Min(100, (elapsed.TotalSeconds / totalDuration.TotalSeconds) * 100);
                        
                        var remaining = totalDuration - elapsed;
                        RemainingTime = remaining.ToString(@"mm\:ss");
                        Console.WriteLine($"[RecipeViewModel] Progress: {StepProgress:F1}%, Time remaining: {RemainingTime}");
                    }
                    else if (currentStep.IsCompleted)
                    {
                        StepProgress = 100;
                        RemainingTime = "Completed";
                        Console.WriteLine($"[RecipeViewModel] Step completed");
                    }
                }
                
                OnPropertyChanged(nameof(Recipe.ProgressPercentage));
            });
        }
    }

    public async Task StartPreparation()
    {
        Console.WriteLine($"[RecipeViewModel] Starting preparation for recipe: {Recipe.Name}");
        if (Recipe.IsInProgress) 
        {
            Console.WriteLine($"[RecipeViewModel] Recipe {Recipe.Name} is already in progress");
            return;
        }
        
        _cancellationTokenSource = new CancellationTokenSource();
        Recipe.IsInProgress = true;
        
        try
        {
            for (int i = 0; i < Recipe.Steps.Count; i++)
            {
                if (_cancellationTokenSource.Token.IsCancellationRequested)
                {
                    Console.WriteLine($"[RecipeViewModel] Recipe {Recipe.Name} preparation cancelled");
                    Recipe.IsInProgress = false;
                    return;
                }

                Recipe.CurrentStepIndex = i;
                var step = Recipe.Steps[i];
                step.StartTime = DateTime.Now;
                _stepStartTime = DateTime.Now;
                _currentStepDuration = step.Duration;
                CurrentStepDescription = step.Step;
                Console.WriteLine($"[RecipeViewModel] Starting step {i + 1}: {step.Step}");
                
                // Start a background task to update progress more frequently
                var progressTask = Task.Run(async () =>
                {
                    while (!_cancellationTokenSource.Token.IsCancellationRequested)
                    {
                        var elapsed = DateTime.Now - _stepStartTime;
                        var progress = Math.Min(100, (elapsed.TotalSeconds / _currentStepDuration) * 100);
                        var remaining = TimeSpan.FromSeconds(_currentStepDuration - elapsed.TotalSeconds);
                        
                        await Dispatcher.UIThread.InvokeAsync(() =>
                        {
                            StepProgress = progress;
                            RemainingTime = remaining.ToString(@"mm\:ss");
                        });
                        
                        if (progress >= 100) break;
                        await Task.Delay(100, _cancellationTokenSource.Token);
                    }
                }, _cancellationTokenSource.Token);

                await Task.Delay(TimeSpan.FromSeconds(step.Duration), _cancellationTokenSource.Token);
                
                step.EndTime = DateTime.Now;
                step.IsCompleted = true;
                Console.WriteLine($"[RecipeViewModel] Completed step {i + 1}: {step.Step}");
                OnPropertyChanged(nameof(Recipe.ProgressPercentage));
            }
            
            Recipe.IsCompleted = true;
            Recipe.IsInProgress = false;
            RemainingTime = "Completed";
            Console.WriteLine($"[RecipeViewModel] Recipe {Recipe.Name} preparation completed");
        }
        catch (OperationCanceledException)
        {
            Console.WriteLine($"[RecipeViewModel] Recipe {Recipe.Name} preparation was cancelled");
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
        Console.WriteLine($"[RecipeViewModel] Cancelling preparation for recipe: {Recipe.Name}");
        _cancellationTokenSource?.Cancel();
        Recipe.IsInProgress = false;
        RemainingTime = "Cancelled";
    }

    private void UpdateCurrentStep()
    {
        if (Recipe.CurrentStepIndex < Recipe.Steps.Count)
        {
            CurrentStepDescription = Recipe.Steps[Recipe.CurrentStepIndex].Step;
            Console.WriteLine($"[RecipeViewModel] Updated current step: {CurrentStepDescription}");
        }
    }
} 
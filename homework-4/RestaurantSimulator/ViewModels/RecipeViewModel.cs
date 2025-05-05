using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Threading;
using Avalonia.Threading;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
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
    private bool _isPaused;
    private DateTime? _pauseStartTime;
    private TimeSpan _totalPausedTime = TimeSpan.Zero;

    public RecipeViewModel(Recipe recipe)
    {
        Console.WriteLine($"[RecipeViewModel] Creating view model for recipe: {recipe.Name}");
        _recipe = recipe;
        UpdateCurrentStep();
    }

    [RelayCommand]
    private async Task PauseRecipe()
    {
        _isPaused = !_isPaused;
        Recipe.IsPaused = _isPaused;
        if (_isPaused)
        {
            _pauseStartTime = DateTime.Now;
        }
        else if (_pauseStartTime.HasValue)
        {
            _totalPausedTime += DateTime.Now - _pauseStartTime.Value;
            _pauseStartTime = null;
        }
    }

    [RelayCommand]
    private async Task StopRecipe()
    {
        _cancellationTokenSource?.Cancel();
        Recipe.IsInProgress = false;
        Recipe.IsPaused = false;
        Recipe.IsCompleted = true;
    }

    public void UpdateProgress()
    {
        if (_isPaused || Recipe.CurrentStepIndex >= Recipe.Steps.Count)
            return;

        var currentStep = Recipe.Steps[Recipe.CurrentStepIndex];
        if (!currentStep.StartTime.HasValue)
            return;

        var elapsed = DateTime.Now - currentStep.StartTime.Value - _totalPausedTime;
        if (_pauseStartTime.HasValue)
        {
            elapsed -= (DateTime.Now - _pauseStartTime.Value);
        }

        var progress = Math.Min(100, (elapsed.TotalSeconds / currentStep.Duration) * 100);
        StepProgress = progress;
        RemainingTime = TimeSpan.FromSeconds(Math.Max(0, currentStep.Duration - elapsed.TotalSeconds)).ToString(@"mm\:ss");
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
                        if (!_isPaused)
                        {
                            var elapsed = DateTime.Now - _stepStartTime - _totalPausedTime;
                            if (_pauseStartTime.HasValue)
                            {
                                elapsed -= (DateTime.Now - _pauseStartTime.Value);
                            }
                            
                            var progress = Math.Min(100, (elapsed.TotalSeconds / _currentStepDuration) * 100);
                            var remaining = TimeSpan.FromSeconds(_currentStepDuration - elapsed.TotalSeconds);
                            
                            await Dispatcher.UIThread.InvokeAsync(() =>
                            {
                                StepProgress = progress;
                                RemainingTime = remaining.ToString(@"mm\:ss");
                            });
                            
                            if (progress >= 100) break;
                        }
                        await Task.Delay(100, _cancellationTokenSource.Token);
                    }
                }, _cancellationTokenSource.Token);

                // Wait for the step duration, accounting for pauses
                var stepStartTime = DateTime.Now;
                var remainingDuration = TimeSpan.FromSeconds(step.Duration);
                while (remainingDuration > TimeSpan.Zero && !_cancellationTokenSource.Token.IsCancellationRequested)
                {
                    if (!_isPaused)
                    {
                        var elapsed = DateTime.Now - stepStartTime - _totalPausedTime;
                        if (_pauseStartTime.HasValue)
                        {
                            elapsed -= (DateTime.Now - _pauseStartTime.Value);
                        }
                        remainingDuration = TimeSpan.FromSeconds(step.Duration) - elapsed;
                    }
                    await Task.Delay(100, _cancellationTokenSource.Token);
                }
                
                if (_cancellationTokenSource.Token.IsCancellationRequested)
                {
                    break;
                }
                
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
        Recipe.IsPaused = false;
        Recipe.IsCompleted = false;
        Recipe.CurrentStepIndex = 0;
        foreach (var step in Recipe.Steps)
        {
            step.IsCompleted = false;
            step.StartTime = null;
            step.EndTime = null;
        }
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
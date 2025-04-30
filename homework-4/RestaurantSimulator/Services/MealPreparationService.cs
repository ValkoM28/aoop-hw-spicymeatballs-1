using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using RestaurantSimulator.Models;

namespace RestaurantSimulator.Services;

public class MealPreparationService
{
    private readonly ConcurrentQueue<Recipe> _recipeQueue = new();
    private readonly SemaphoreSlim _stationSemaphore;
    private readonly object _lockObject = new();
    private CancellationTokenSource? _cancellationTokenSource;


    public List<KitchenStation> KitchenStations { get; private set; } = [];
    public event EventHandler<RecipeProgressEventArgs>? RecipeProgressUpdated;
    public event EventHandler<RecipeEventArgs>? RecipeCompleted;

    public MealPreparationService(int numberOfStations)
    {
        _stationSemaphore = new SemaphoreSlim(numberOfStations, numberOfStations);
        InitializeKitchenStations(numberOfStations);
    }

    private void InitializeKitchenStations(int count)
    {
        for (int i = 0; i < count; i++)
        {
            KitchenStations.Add(new KitchenStation
            {
                Id = i + 1,
                Name = $"Station {i + 1}",
            });
        }
    }

    public void AddRecipe(Recipe recipe)
    {
        _recipeQueue.Enqueue(recipe);
        StartProcessing();
    }

    private async void StartProcessing()
    {
        if (_cancellationTokenSource == null)
        {
            _cancellationTokenSource = new CancellationTokenSource();
            await ProcessRecipesAsync(_cancellationTokenSource.Token);
        }
    }

    private async Task ProcessRecipesAsync(CancellationToken cancellationToken)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            if (_recipeQueue.TryDequeue(out var recipe))
            {
                await _stationSemaphore.WaitAsync(cancellationToken);
                var station = GetAvailableStation();
                if (station != null)
                {
                    _ = ProcessRecipeAtStationAsync(station, recipe, cancellationToken);
                }
                else
                {
                    _stationSemaphore.Release();
                    _recipeQueue.Enqueue(recipe);
                }
            }
            else
            {
                await Task.Delay(100, cancellationToken);
            }
        }
    }

    private KitchenStation? GetAvailableStation()
    {
        lock (_lockObject)
        {
            return KitchenStations.Find(s => s.IsAvailable);
        }
    }

    private async Task ProcessRecipeAtStationAsync(KitchenStation station, Recipe recipe, CancellationToken cancellationToken)
    {
        try
        {
            station.CurrentRecipe = recipe;
            recipe.IsInProgress = true;

            foreach (var step in recipe.Steps)
            {
                if (cancellationToken.IsCancellationRequested)
                    break;

                step.StartTime = DateTime.Now;
                await Task.Delay(step.Duration * 1000, cancellationToken);
                step.EndTime = DateTime.Now;
                step.IsCompleted = true;
                recipe.CurrentStepIndex++;

                RecipeProgressUpdated?.Invoke(this, new RecipeProgressEventArgs(recipe));
            }

            recipe.IsCompleted = true;
            recipe.IsInProgress = false;
            RecipeCompleted?.Invoke(this, new RecipeEventArgs(recipe));
        }
        finally
        {
            station.CurrentRecipe = null;
            _stationSemaphore.Release();
        }
    }

    public void StopProcessing()
    {
        _cancellationTokenSource?.Cancel();
        _cancellationTokenSource = null;
    }
}

public class RecipeProgressEventArgs : EventArgs
{
    public Recipe Recipe { get; }

    public RecipeProgressEventArgs(Recipe recipe)
    {
        Recipe = recipe;
    }
}

public class RecipeEventArgs : EventArgs
{
    public Recipe Recipe { get; }

    public RecipeEventArgs(Recipe recipe)
    {
        Recipe = recipe;
    }
} 
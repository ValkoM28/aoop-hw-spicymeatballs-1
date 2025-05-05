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
    private readonly Dictionary<Recipe, KitchenStation> _activeRecipes = new();

    public List<KitchenStation> KitchenStations { get; private set; } = [];
    public event EventHandler<RecipeProgressEventArgs>? RecipeProgressUpdated;
    public event EventHandler<RecipeEventArgs>? RecipeCompleted;
    public event EventHandler<RecipeErrorEventArgs>? RecipeError;

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
        Console.WriteLine($"[MealPreparationService] Adding recipe: {recipe?.Name}");
        if (recipe.IsInProgress || recipe.IsCompleted)
        {
            Console.WriteLine($"[MealPreparationService] Recipe {recipe.Name} is already in progress or completed");
            RecipeError?.Invoke(this, new RecipeErrorEventArgs(recipe, "Recipe is already in progress or completed"));
            return;
        }

        _recipeQueue.Enqueue(recipe);
        Console.WriteLine($"[MealPreparationService] Recipe {recipe.Name} added to queue");
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
            try
            {
                if (_recipeQueue.TryDequeue(out var recipe))
                {
                    await _stationSemaphore.WaitAsync(cancellationToken);
                    var station = GetAvailableStation();
                    if (station != null)
                    {
                        _activeRecipes[recipe] = station;
                        _ = ProcessRecipeAtStationAsync(station, recipe, cancellationToken);
                    }
                    else
                    {
                        _stationSemaphore.Release();
                        _recipeQueue.Enqueue(recipe);
                        await Task.Delay(1000, cancellationToken); // Wait before retrying
                    }
                }
                else
                {
                    await Task.Delay(100, cancellationToken);
                }
            }
            catch (OperationCanceledException)
            {
                break;
            }
            catch (Exception ex)
            {
                RecipeError?.Invoke(this, new RecipeErrorEventArgs(null, $"Error processing recipe: {ex.Message}"));
                await Task.Delay(1000, cancellationToken); // Wait before retrying
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
        Console.WriteLine($"[MealPreparationService] Starting to process recipe {recipe.Name} at station {station.Name}");
        try
        {
            station.CurrentRecipe = recipe;
            Console.WriteLine($"[MealPreparationService] Assigned recipe {recipe.Name} to station {station.Name}");
            recipe.IsInProgress = true;

            foreach (var step in recipe.Steps)
            {
                if (cancellationToken.IsCancellationRequested)
                    break;

                step.StartTime = DateTime.Now;
                Console.WriteLine($"[MealPreparationService] Starting step: {step.Step} for recipe {recipe.Name}");
                
                // Trigger initial progress update
                RecipeProgressUpdated?.Invoke(this, new RecipeProgressEventArgs(recipe));
                
                // Create a timer to update progress more frequently
                using var timer = new System.Timers.Timer(100); // Update every 100ms
                timer.Elapsed += (s, e) => RecipeProgressUpdated?.Invoke(this, new RecipeProgressEventArgs(recipe));
                timer.Start();
                
                try
                {
                    await Task.Delay(TimeSpan.FromSeconds(step.Duration), cancellationToken);
                }
                finally
                {
                    timer.Stop();
                }
                
                step.EndTime = DateTime.Now;
                step.IsCompleted = true;
                recipe.CurrentStepIndex++;
                Console.WriteLine($"[MealPreparationService] Completed step {recipe.CurrentStepIndex} for recipe {recipe.Name}");
                RecipeProgressUpdated?.Invoke(this, new RecipeProgressEventArgs(recipe));
            }

            recipe.IsCompleted = true;
            recipe.IsInProgress = false;
            Console.WriteLine($"[MealPreparationService] Completed recipe {recipe.Name}");
            RecipeCompleted?.Invoke(this, new RecipeEventArgs(recipe));
        }
        catch (OperationCanceledException)
        {
            Console.WriteLine($"[MealPreparationService] Recipe {recipe.Name} was cancelled");
            recipe.IsInProgress = false;
            RecipeError?.Invoke(this, new RecipeErrorEventArgs(recipe, "Recipe preparation was cancelled"));
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[MealPreparationService] Error processing recipe {recipe.Name}: {ex.Message}");
            recipe.IsInProgress = false;
            RecipeError?.Invoke(this, new RecipeErrorEventArgs(recipe, $"Error during recipe preparation: {ex.Message}"));
        }
        finally
        {
            Console.WriteLine($"[MealPreparationService] Clearing recipe {recipe.Name} from station {station.Name}");
            station.CurrentRecipe = null;
            _activeRecipes.Remove(recipe);
            _stationSemaphore.Release();
        }
    }

    public void StopProcessing()
    {
        _cancellationTokenSource?.Cancel();
        _cancellationTokenSource = null;
        
        // Clear all active recipes
        foreach (var recipe in _activeRecipes.Keys)
        {
            recipe.IsInProgress = false;
            recipe.IsCompleted = false;
        }
        _activeRecipes.Clear();
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

public class RecipeErrorEventArgs : EventArgs
{
    public Recipe? Recipe { get; }
    public string ErrorMessage { get; }

    public RecipeErrorEventArgs(Recipe? recipe, string errorMessage)
    {
        Recipe = recipe;
        ErrorMessage = errorMessage;
    }
} 
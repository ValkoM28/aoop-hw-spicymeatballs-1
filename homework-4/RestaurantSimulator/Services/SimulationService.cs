using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RestaurantSimulator.Models;

namespace RestaurantSimulator.Services;

public class SimulationService
{
    private readonly ConcurrentQueue<Recipe> _recipeQueue = new();
    private readonly List<KitchenStation> _kitchenStations;
    private bool _isRunning;
    private readonly object _lockObject = new();

    public SimulationService(List<KitchenStation> kitchenStations)
    {
        _kitchenStations = kitchenStations;
    }

    public void StartSimulation()
    {
        _isRunning = true;
        Task.Run(ProcessRecipes);
    }

    public void StopSimulation()
    {
        _isRunning = false;
    }

    public void AddRecipe(Recipe recipe)
    {
        _recipeQueue.Enqueue(recipe);
    }

    private async Task ProcessRecipes()
    {
        while (_isRunning)
        {
            if (_recipeQueue.TryDequeue(out var recipe))
            {
                var availableStation = FindAvailableStation(recipe);
                if (availableStation != null)
                {
                    await ProcessRecipeAtStation(recipe, availableStation);
                }
                else
                {
                    // If no station is available, put the recipe back in the queue
                    _recipeQueue.Enqueue(recipe);
                    await Task.Delay(1000); // Wait a second before trying again
                }
            }
            else
            {
                await Task.Delay(1000); // Wait a second before checking the queue again
            }
        }
    }

    private KitchenStation? FindAvailableStation(Recipe recipe)
    {
        lock (_lockObject)
        {
            return _kitchenStations.Find(station =>
                station.IsAvailable && 
                recipe.Equipment.All(e => station.AvailableEquipment.Contains(e)));
        }
    }

    private async Task ProcessRecipeAtStation(Recipe recipe, KitchenStation station)
    {
        station.CurrentRecipe = recipe;
        recipe.IsInProgress = true;

        foreach (var step in recipe.Steps)
        {
            step.StartTime = DateTime.Now;
            await Task.Delay(step.Duration * 1000); // Convert duration to milliseconds
            step.EndTime = DateTime.Now;
            step.IsCompleted = true;
        }

        recipe.IsCompleted = true;
        recipe.IsInProgress = false;
        station.CurrentRecipe = null;
    }
} 
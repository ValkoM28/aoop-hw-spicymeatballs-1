using System;
using System.Collections.Generic;
using System.Linq;
using RestaurantSimulator.Models;

namespace RestaurantSimulator.Utilities;

public static class Extensions
{
    public static string ToStatusString(this bool isRunning)
    {
        return isRunning ? "Running" : "Stopped";
    }

    public static string ToStartStopString(this bool isRunning)
    {
        return isRunning ? "Stop" : "Start";
    }

    public static bool CanProcessRecipe(this KitchenStation station, Recipe recipe)
    {
        return station.IsAvailable && 
               recipe.Equipment.All(e => station.AvailableEquipment.Contains(e));
    }

    public static TimeSpan GetTotalDuration(this Recipe recipe)
    {
        return TimeSpan.FromSeconds(recipe.Steps.Sum(step => step.Duration));
    }

    public static string FormatDuration(this TimeSpan duration)
    {
        return duration.TotalMinutes >= 1
            ? $"{duration.TotalMinutes:F1} minutes"
            : $"{duration.TotalSeconds:F0} seconds";
    }
} 
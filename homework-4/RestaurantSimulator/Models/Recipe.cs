using System;
using System.Collections.Generic;

namespace RestaurantSimulator.Models;

public class Recipe
{
    public string Name { get; set; } = string.Empty;
    public string Difficulty { get; set; } = string.Empty;
    public List<string> Equipment { get; set; } = new();
    public List<RecipeStep> Steps { get; set; } = new();
    public bool IsInProgress { get; set; }
    public bool IsCompleted { get; set; }
    public int CurrentStepIndex { get; set; }
    public double ProgressPercentage => Steps.Count > 0 ? (double)CurrentStepIndex / Steps.Count * 100 : 0;
} 
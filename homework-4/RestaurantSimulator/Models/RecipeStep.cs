using System;

namespace RestaurantSimulator.Models;

public class RecipeStep
{
    public string Step { get; set; } = string.Empty;
    public int Duration { get; set; }
    public bool IsCompleted { get; set; }
    public DateTime? StartTime { get; set; }
    public DateTime? EndTime { get; set; }
} 
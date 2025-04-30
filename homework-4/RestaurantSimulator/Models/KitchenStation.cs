using System;
using System.Collections.Generic;

namespace RestaurantSimulator.Models;

public class KitchenStation
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public Recipe? CurrentRecipe { get; set; }
    public bool IsAvailable => CurrentRecipe == null;
    public List<string> AvailableEquipment { get; set; } = new();
    public bool IsActive { get; set; }
} 
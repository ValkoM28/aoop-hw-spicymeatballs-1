using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using RestaurantSimulator.Models;

namespace RestaurantSimulator.Services;

public class JsonDataService
{
    private readonly string _jsonFilePath;

    public JsonDataService(string jsonFilePath)
    {
        _jsonFilePath = jsonFilePath;
    }

    public async Task<RestaurantData> LoadDataAsync()
    {
        try
        {
            var jsonString = await File.ReadAllTextAsync(_jsonFilePath);
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            
            var data = JsonSerializer.Deserialize<RestaurantData>(jsonString, options);
            if (data == null)
            {
                throw new InvalidOperationException("Failed to deserialize JSON data");
            }
            
            return data;
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"Error loading JSON data: {ex.Message}", ex);
        }
    }
}

public class RestaurantData
{
    public List<Ingredient> Ingredients { get; set; } = new();
    public List<Recipe> Recipes { get; set; } = new();
} 
using System.Collections.Generic;
using System.Threading.Tasks;
using RestaurantSimulator.Models;

namespace RestaurantSimulator.Services;

public class DataProviderService
{
    private readonly JsonDataService _jsonDataService;
    private List<Recipe> _recipes = new();

    public DataProviderService(JsonDataService jsonDataService)
    {
        _jsonDataService = jsonDataService;
    }

    public async Task LoadRecipesAsync()
    {
        var data = await _jsonDataService.LoadDataAsync();
        _recipes = data.Recipes;
    }

    public IEnumerable<Recipe> GetAllRecipes()
    {
        return _recipes;
    }

    public Recipe? GetRecipeByName(string name)
    {
        return _recipes.Find(r => r.Name == name);
    }

    public IEnumerable<Recipe> GetRecipesByDifficulty(string difficulty)
    {
        return _recipes.FindAll(r => r.Difficulty == difficulty);
    }

    public IEnumerable<Recipe> GetRecipesByEquipment(string equipment)
    {
        return _recipes.FindAll(r => r.Equipment.Contains(equipment));
    }

    public void ResetRecipeState(Recipe recipe)
    {
        recipe.IsInProgress = false;
        recipe.IsCompleted = false;
        recipe.CurrentStepIndex = 0;
        foreach (var step in recipe.Steps)
        {
            step.IsCompleted = false;
            step.StartTime = null;
            step.EndTime = null;
        }
    }
} 
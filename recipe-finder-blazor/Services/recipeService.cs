using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

public class RecipeService
{
    private readonly HttpClient _httpClient;
    private const string API_KEY = "36ff91910cc4441aa249e32ab7ac7813";

    public RecipeService(HttpClient httpClient)
    {
        _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
    }

    public async Task<List<Recipe>> SearchRecipes(string query, string diet, string cuisine)
    {
        var url = $"https://api.spoonacular.com/recipes/complexSearch?apiKey={API_KEY}&query={query}&diet={diet}&cuisine={cuisine}";

        try
        {
            var result = await _httpClient.GetFromJsonAsync<RecipeResponse>(url);
            return result?.Results;
        }
        catch (Exception ex)
        {
            // Log or handle the exception as needed
            Console.WriteLine($"Error fetching recipes: {ex.Message}");
            return new List<Recipe>();
        }
    }

    public async Task<Recipe> GetRecipeInformation(int id)
    {
        var url = $"https://api.spoonacular.com/recipes/{id}/information?apiKey={API_KEY}";

        try
        {
            var result = await _httpClient.GetFromJsonAsync<Recipe>(url);
            return result;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error fetching recipe information: {ex.Message}");
            return new Recipe();
        }
    }
}

public class RecipeResponse
{
    public List<Recipe> Results { get; set; }
}

public class Recipe
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Image { get; set; } = string.Empty;
    public double ReadyInMinutes { get; set; } = 0;
    public double Servings { get; set; } = 0;
    public double HealthScore { get; set; } = 0;
    public double PricePerServing { get; set; } = 0;
    public List<Ingredient> ExtendedIngredients { get; set; } = new List<Ingredient>();
    public string Instructions { get; set; } = string.Empty;
}

public class Ingredient
{
    public string Name { get; set; } = string.Empty;
    public Measures Measures { get; set; } = new Measures();
}

public class Measures
{
    public Metric Metric { get; set; } = new Metric();
}

public class Metric
{
    public double Amount { get; set; } = 0;
    public string UnitShort { get; set; } = string.Empty;
}

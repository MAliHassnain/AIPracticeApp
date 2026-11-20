using System.Text.Json.Nodes;
using PracticeApp.Core.Agents.Common;
using PracticeApp.Core.Agents.Recipes.Data;
using PracticeApp.Core.Agents.Recipes.Models;
using static PracticeApp.Core.Agents.Common.ToolSchema;

namespace PracticeApp.Core.Agents.Recipes.Tools
{
    public sealed class SaveRecipeTool(IRecipeRepository repository) : IAgentTool
    {
        public string Name => "save_recipe";

        public JsonObject Definition => new()
        {
            ["name"] = Name,
            ["description"] = "Saves the final chosen recipe with full details. Call it once, after researching the recipe.",
            ["input_schema"] = new JsonObject
            {
                ["type"] = "object",
                ["properties"] = new JsonObject
                {
                    ["title"] = Property("string", "Dish name"),
                    ["description"] = Property("string", "One or two sentence summary of the dish"),
                    ["servings"] = Property("integer", "Number of servings"),
                    ["total_time"] = Property("string", "Total time, e.g. '45 minutes'"),
                    ["ingredients"] = new JsonObject
                    {
                        ["type"] = "array",
                        ["description"] = "Every ingredient with its quantity",
                        ["items"] = new JsonObject
                        {
                            ["type"] = "object",
                            ["properties"] = new JsonObject
                            {
                                ["item"] = Property("string", "Ingredient name"),
                                ["quantity"] = Property("string", "Amount, e.g. '2 tbsp' or '500 g'")
                            },
                            ["required"] = new JsonArray("item", "quantity")
                        }
                    },
                    ["method"] = new JsonObject
                    {
                        ["type"] = "array",
                        ["description"] = "Cooking steps in order, one string per step",
                        ["items"] = new JsonObject { ["type"] = "string" }
                    },
                    ["source_url"] = Property("string", "URL of the recipe you based this on")
                },
                ["required"] = new JsonArray("title", "ingredients", "method")
            }
        };

        public Task<ToolResult> ExecuteAsync(JsonNode? input, CancellationToken cancellationToken = default)
        {
            return Task.FromResult(Save(input));
        }

        private ToolResult Save(JsonNode? input)
        {
            var title = ReadString(input?["title"]).Trim();
            if (title.Length == 0)
            {
                return new ToolResult("Error: title is required.", IsError: true);
            }

            var recipe = new Recipe
            {
                Title = title,
                Description = ReadString(input?["description"]).Trim(),
                Servings = ReadInt(input?["servings"]),
                TotalTime = ReadString(input?["total_time"]).Trim(),
                SourceUrl = ReadString(input?["source_url"]).Trim(),
                Ingredients = ReadIngredients(input?["ingredients"]),
                Method = ReadMethod(input?["method"])
            };

            if (recipe.Ingredients.Count == 0 || recipe.Method.Count == 0)
            {
                return new ToolResult(
                    "Error: ingredients and method are required. Call save_recipe again with complete data.",
                    IsError: true);
            }

            repository.Add(recipe);
            return new ToolResult($"Saved recipe '{recipe.Title}'.", recipe);
        }

        private static List<Ingredient> ReadIngredients(JsonNode? node)
        {
            if (node is not JsonArray items) return new List<Ingredient>();

            return items
                .Select(i => new Ingredient
                {
                    Item = ReadString(i?["item"]).Trim(),
                    Quantity = ReadString(i?["quantity"]).Trim()
                })
                .Where(i => i.Item.Length > 0)
                .ToList();
        }

        private static List<string> ReadMethod(JsonNode? node)
        {
            if (node is not JsonArray steps) return new List<string>();

            return steps
                .Select(s => ReadString(s).Trim())
                .Where(s => s.Length > 0)
                .ToList();
        }
    }
}

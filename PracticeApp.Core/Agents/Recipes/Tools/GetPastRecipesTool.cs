using System.Text.Json.Nodes;
using PracticeApp.Core.Agents.Common;
using PracticeApp.Core.Agents.Recipes.Data;

namespace PracticeApp.Core.Agents.Recipes.Tools
{
    public sealed class GetPastRecipesTool(IRecipeRepository repository) : IAgentTool
    {
        public string Name => "get_past_recipes";

        public JsonObject Definition => new()
        {
            ["name"] = Name,
            ["description"] = "Returns the titles of dishes already saved. Call this first and do not repeat any of them.",
            ["input_schema"] = ToolSchema.EmptyObject()
        };

        public Task<ToolResult> ExecuteAsync(JsonNode? input, CancellationToken cancellationToken = default)
        {
            var titles = repository.GetTitles();
            var output = titles.Count == 0 ? "No saved recipes yet." : string.Join(", ", titles);
            return Task.FromResult(new ToolResult(output));
        }
    }
}

using System.Text.Json.Nodes;
using PracticeApp.Core.Agents.Common;
using PracticeApp.Core.Agents.Recipes.Models;
using PracticeApp.Core.Agents.Recipes.Tools;

namespace PracticeApp.Core.Agents.Recipes
{
    /// <summary>
    /// The desi recipe agent: checks past recipes, searches the web and saves one new recipe.
    /// Defines which tools and prompt the agent uses; the loop itself is in <see cref="AgentRunner"/>.
    /// </summary>
    public sealed class RecipeAgent(
        AgentRunner runner,
        RecipeAgentSettings settings,
        GetPastRecipesTool getPastRecipes,
        SaveRecipeTool saveRecipe)
    {
        public async Task<RecipeRunResult> RunAsync(string goal, CancellationToken cancellationToken = default)
        {
            var request = new AgentRunRequest(
                goal,
                settings.SystemPrompt,
                new IAgentTool[] { getPastRecipes, saveRecipe },
                new[] { BuildWebSearchTool() },
                settings.MaxTurns);

            var outcome = await runner.RunAsync(request, cancellationToken);
            var recipe = outcome.Artifacts.OfType<Recipe>().LastOrDefault();

            return new RecipeRunResult(outcome.FinalAnswer, recipe, outcome.Steps, outcome.StopReason, outcome.Turns);
        }

        private JsonObject BuildWebSearchTool() => new()
        {
            ["type"] = settings.WebSearchToolType,
            ["name"] = "web_search",
            ["max_uses"] = settings.WebSearchMaxUses
        };
    }
}

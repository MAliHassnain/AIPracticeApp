using PracticeApp.Core.AI;
using PracticeApp.Core.Agents.Recipes.Data;
using PracticeApp.Core.Agents.Recipes.Models;

namespace PracticeApp.Core.Agents.Recipes
{
    public sealed class RecipeAgentService(
        RecipeAgent agent,
        IRecipeRepository repository,
        RecipeAgentSettings settings,
        ILlmClient llm) : IRecipeAgentService
    {
        public RecipeAgentInfo GetInfo() => new(llm.Model, settings.MaxTurns, settings.DefaultGoal);

        public Task<RecipeRunResult> RunAsync(string? goal, CancellationToken cancellationToken = default)
        {
            var effectiveGoal = string.IsNullOrWhiteSpace(goal) ? settings.DefaultGoal : goal.Trim();
            return agent.RunAsync(effectiveGoal, cancellationToken);
        }

        public IReadOnlyList<Recipe> GetHistory() =>
            repository.GetAll().OrderByDescending(r => r.CreatedUtc).ToList();

        public bool DeleteHistoryItem(string id) => repository.Delete(id);

        public void ClearHistory() => repository.Clear();
    }
}

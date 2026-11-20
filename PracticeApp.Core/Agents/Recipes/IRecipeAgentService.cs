using PracticeApp.Core.Agents.Recipes.Models;

namespace PracticeApp.Core.Agents.Recipes
{
    /// <summary>Business operations for the recipe agent: running it and managing its history.</summary>
    public interface IRecipeAgentService
    {
        RecipeAgentInfo GetInfo();
        Task<RecipeRunResult> RunAsync(string? goal, CancellationToken cancellationToken = default);
        IReadOnlyList<Recipe> GetHistory();
        bool DeleteHistoryItem(string id);
        void ClearHistory();
    }
}

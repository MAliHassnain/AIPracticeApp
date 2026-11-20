using PracticeApp.Core.Agents.Common;

namespace PracticeApp.Core.Agents.Recipes.Models
{
    public sealed record RecipeAgentInfo(string Model, int MaxTurns, string DefaultGoal);

    /// <param name="FinalAnswer">The agent's closing note. The recipe itself is in <paramref name="Recipe"/>.</param>
    /// <param name="Recipe">The recipe saved during this run, if any.</param>
    public sealed record RecipeRunResult(
        string FinalAnswer,
        Recipe? Recipe,
        IReadOnlyList<AgentStep> Steps,
        string StopReason,
        int Turns);
}

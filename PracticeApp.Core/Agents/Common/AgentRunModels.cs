using System.Text.Json.Nodes;

namespace PracticeApp.Core.Agents.Common
{
    /// <param name="Goal">The user's request.</param>
    /// <param name="SystemPrompt">Instructions for the agent.</param>
    /// <param name="Tools">Tools that our code runs.</param>
    /// <param name="ServerTools">Tools the model provider runs, such as web search.</param>
    /// <param name="MaxTurns">Safety limit for the number of model calls.</param>
    public sealed record AgentRunRequest(
        string Goal,
        string SystemPrompt,
        IReadOnlyList<IAgentTool> Tools,
        IReadOnlyList<JsonObject> ServerTools,
        int MaxTurns);

    /// <param name="FinalAnswer">The model's closing message.</param>
    /// <param name="Artifacts">Objects returned by tools during the run, in order.</param>
    public sealed record AgentRunOutcome(
        string FinalAnswer,
        IReadOnlyList<AgentStep> Steps,
        string StopReason,
        int Turns,
        IReadOnlyList<object> Artifacts);
}

namespace PracticeApp.Core.Agents.Common
{
    /// <param name="Output">Text sent back to the model.</param>
    /// <param name="Artifact">Optional object the tool produced, returned to the caller of the agent.</param>
    /// <param name="IsError">Tells the model the call failed so it can retry.</param>
    public sealed record ToolResult(string Output, object? Artifact = null, bool IsError = false);
}

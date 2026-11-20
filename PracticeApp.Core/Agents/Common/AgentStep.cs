namespace PracticeApp.Core.Agents.Common
{
    /// <summary>One thing that happened during an agent run, shown as a timeline in the UI.</summary>
    public sealed record AgentStep(int Turn, string Kind, string Detail);

    public static class AgentStepKind
    {
        public const string Text = "text";
        public const string WebSearch = "web_search";
        public const string WebSearchResult = "web_search_result";
        public const string ToolRequest = "tool_request";
        public const string ToolResult = "tool_result";
        public const string Info = "info";
    }
}

using System.Text.Json.Nodes;

namespace PracticeApp.Core.Agents.Common
{
    /// <summary>Turns the content blocks of a model response into timeline steps.</summary>
    internal static class AgentStepRecorder
    {
        private const int MaxDetailLength = 240;
        private const int MaxSearchResults = 5;

        public static void RecordBlocks(int turn, JsonArray content, List<AgentStep> steps, bool includeText)
        {
            foreach (var block in content)
            {
                if (block is null) continue;

                switch (block["type"]?.GetValue<string>())
                {
                    case "text":
                        var text = block["text"]?.GetValue<string>();
                        if (includeText && !string.IsNullOrWhiteSpace(text))
                        {
                            steps.Add(new AgentStep(turn, AgentStepKind.Text, text));
                        }
                        break;

                    case "server_tool_use":
                        var query = block["input"]?["query"]?.GetValue<string>() ?? "";
                        steps.Add(new AgentStep(turn, AgentStepKind.WebSearch, query));
                        break;

                    case "web_search_tool_result":
                        steps.Add(new AgentStep(turn, AgentStepKind.WebSearchResult, SummarizeSearchResult(block["content"])));
                        break;

                    case "tool_use":
                        var name = block["name"]?.GetValue<string>() ?? "?";
                        var input = block["input"]?.ToJsonString() ?? "{}";
                        steps.Add(new AgentStep(turn, AgentStepKind.ToolRequest, Truncate($"{name} {input}")));
                        break;
                }
            }
        }

        private static string SummarizeSearchResult(JsonNode? content)
        {
            if (content is JsonArray results)
            {
                return string.Join(
                    Environment.NewLine,
                    results.Take(MaxSearchResults).Select(r => $"{r?["title"]} - {r?["url"]}"));
            }

            if (content is JsonObject error)
            {
                return $"Search error: {error["error_code"]}";
            }

            return "Search result received.";
        }

        private static string Truncate(string value) =>
            value.Length <= MaxDetailLength ? value : value[..MaxDetailLength] + "...";
    }
}

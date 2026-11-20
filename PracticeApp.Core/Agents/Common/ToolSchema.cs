using System.Text.Json.Nodes;

namespace PracticeApp.Core.Agents.Common
{
    /// <summary>Helpers for building tool input schemas and reading tool input safely.</summary>
    public static class ToolSchema
    {
        public static JsonObject Property(string type, string description) =>
            new() { ["type"] = type, ["description"] = description };

        public static JsonObject EmptyObject() =>
            new() { ["type"] = "object", ["properties"] = new JsonObject() };

        public static string ReadString(JsonNode? node)
        {
            if (node is null) return "";
            if (node is JsonValue value && value.TryGetValue<string>(out var text)) return text;
            return node.ToString();
        }

        public static int? ReadInt(JsonNode? node)
        {
            if (node is null) return null;
            if (node is JsonValue value && value.TryGetValue<int>(out var number)) return number;
            return int.TryParse(node.ToString(), out var parsed) ? parsed : null;
        }
    }
}

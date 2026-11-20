using System.Text.Json.Nodes;

namespace PracticeApp.Core.Agents.Common
{
    /// <summary>A tool that our code runs when the model asks for it.</summary>
    public interface IAgentTool
    {
        /// <summary>Name the model uses to call the tool (letters, digits, underscores).</summary>
        string Name { get; }

        /// <summary>Tool definition sent to the model. Must return a new object on every call.</summary>
        JsonObject Definition { get; }

        Task<ToolResult> ExecuteAsync(JsonNode? input, CancellationToken cancellationToken = default);
    }
}

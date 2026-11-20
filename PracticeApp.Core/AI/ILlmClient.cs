using System.Text.Json.Nodes;

namespace PracticeApp.Core.AI
{
    /// <summary>Sends one request to the language model and returns the raw JSON response.</summary>
    public interface ILlmClient
    {
        string Model { get; }

        Task<JsonNode> SendAsync(
            JsonArray messages,
            JsonArray? tools = null,
            string? systemPrompt = null,
            CancellationToken cancellationToken = default);
    }
}

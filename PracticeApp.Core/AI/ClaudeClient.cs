using System.Text;
using System.Text.Json.Nodes;

namespace PracticeApp.Core.AI
{
    /// <summary>Client for the Anthropic Messages API using raw JSON.</summary>
    public sealed class ClaudeClient(HttpClient http, LlmSettings settings) : ILlmClient
    {
        private const string PlaceholderKey = "YOUR_API_KEY_HERE";

        public string Model => settings.Model;

        public async Task<JsonNode> SendAsync(
            JsonArray messages,
            JsonArray? tools = null,
            string? systemPrompt = null,
            CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(settings.ApiKey) || settings.ApiKey == PlaceholderKey)
            {
                throw new LlmConfigurationException(
                    "API key is not set. Put it in PracticeApp.Server/appsettings.json under Llm:ApiKey.");
            }

            var body = new JsonObject
            {
                ["model"] = settings.Model,
                ["max_tokens"] = settings.MaxTokens,
                ["messages"] = messages.DeepClone()
            };
            if (systemPrompt is not null) body["system"] = systemPrompt;
            if (tools is not null) body["tools"] = tools.DeepClone();

            using var request = new HttpRequestMessage(HttpMethod.Post, settings.ApiUrl)
            {
                Content = new StringContent(body.ToJsonString(), Encoding.UTF8, "application/json")
            };
            request.Headers.Add("x-api-key", settings.ApiKey);
            request.Headers.Add("anthropic-version", settings.ApiVersion);

            using var response = await http.SendAsync(request, cancellationToken);
            var json = await response.Content.ReadAsStringAsync(cancellationToken);

            if (!response.IsSuccessStatusCode)
            {
                throw new LlmApiException((int)response.StatusCode, json);
            }

            return JsonNode.Parse(json)!;
        }
    }
}

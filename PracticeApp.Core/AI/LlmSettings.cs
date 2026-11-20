namespace PracticeApp.Core.AI
{
    /// <summary>
    /// Connection settings for the language model, bound from the "Llm" section of appsettings.json.
    /// The API key is read from user secrets or the Llm__ApiKey environment variable.
    /// </summary>
    public sealed class LlmSettings
    {
        public const string SectionName = "Llm";

        public string ApiKey { get; set; } = "";
        public string ApiUrl { get; set; } = "https://api.anthropic.com/v1/messages";
        public string ApiVersion { get; set; } = "2023-06-01";
        public string Model { get; set; } = "claude-haiku-4-5";
        public int MaxTokens { get; set; } = 4096;
        public int RequestTimeoutMinutes { get; set; } = 3;
    }
}

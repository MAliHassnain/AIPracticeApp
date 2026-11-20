namespace PracticeApp.Core.AI
{
    /// <summary>The model cannot be called because a setting such as the API key is missing.</summary>
    public sealed class LlmConfigurationException(string message) : Exception(message);

    /// <summary>The model API returned an unsuccessful response.</summary>
    public sealed class LlmApiException(int statusCode, string responseBody)
        : Exception($"Model API error {statusCode}: {responseBody}")
    {
        public int StatusCode { get; } = statusCode;
        public string ResponseBody { get; } = responseBody;
    }
}

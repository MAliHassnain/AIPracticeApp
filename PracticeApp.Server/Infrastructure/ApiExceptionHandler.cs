using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using PracticeApp.Core.AI;

namespace PracticeApp.Server.Infrastructure
{
    /// <summary>Converts unhandled exceptions into ProblemDetails responses the client can display.</summary>
    public sealed class ApiExceptionHandler(ILogger<ApiExceptionHandler> logger) : IExceptionHandler
    {
        public async ValueTask<bool> TryHandleAsync(HttpContext context, Exception exception, CancellationToken cancellationToken)
        {
            var (status, title, detail) = exception switch
            {
                LlmConfigurationException ex => (StatusCodes.Status500InternalServerError, "Configuration error", ex.Message),
                LlmApiException ex => (StatusCodes.Status502BadGateway, "Model API error", ex.Message),
                HttpRequestException ex => (StatusCodes.Status502BadGateway, "Model API unreachable", ex.Message),
                TaskCanceledException when !context.RequestAborted.IsCancellationRequested =>
                    (StatusCodes.Status504GatewayTimeout, "Model API timeout", "The model did not respond in time."),
                _ => (StatusCodes.Status500InternalServerError, "Unexpected error", "Something went wrong. Check the server log.")
            };

            logger.LogError(exception, "{Title}", title);

            context.Response.StatusCode = status;
            await context.Response.WriteAsJsonAsync(
                new ProblemDetails { Status = status, Title = title, Detail = detail },
                cancellationToken);
            return true;
        }
    }
}

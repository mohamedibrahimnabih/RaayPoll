using System.Globalization;

namespace RaayPoll.API.Middlewares
{
    public class LoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<LoggingMiddleware> _logger;

        public LoggingMiddleware(RequestDelegate next, ILogger<LoggingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            _logger.LogInformation("Received the request");

            // Call the next delegate/middleware in the pipeline.
            await _next(context);

            _logger.LogInformation("Send the response");
        }
    }

    public static class LoggingMiddlewareExtensions
    {
        public static IApplicationBuilder UseLogging(
            this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<LoggingMiddleware>();
        }
    }
}

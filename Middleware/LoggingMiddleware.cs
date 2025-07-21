namespace UserManagementAPI.Middleware;

/// <summary>
/// Middleware for logging HTTP requests and responses with execution time tracking.
/// </summary>
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
        // Log incoming request
        _logger.LogInformation("Request: {Method} {Path} started at {Time}",
            context.Request.Method,
            context.Request.Path,
            DateTime.UtcNow);

        var stopwatch = System.Diagnostics.Stopwatch.StartNew();

        await _next(context);

        stopwatch.Stop();

        // Log outgoing response
        _logger.LogInformation("Response: {Method} {Path} returned {StatusCode} in {ElapsedMs}ms",
            context.Request.Method,
            context.Request.Path,
            context.Response.StatusCode,
            stopwatch.ElapsedMilliseconds);
    }
}

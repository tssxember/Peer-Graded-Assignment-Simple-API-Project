using UserManagementAPI.Middleware;

namespace UserManagementAPI.Configuration;

/// <summary>
/// Extension methods for configuring middleware in the application pipeline.
/// </summary>
public static class MiddlewareExtensions
{
    /// <summary>
    /// Configures all custom middleware in the correct order.
    /// </summary>
    /// <param name="app">The application builder</param>
    /// <returns>The application builder for method chaining</returns>
    public static IApplicationBuilder UseCustomMiddleware(this IApplicationBuilder app)
    {
        // Configure middleware in the correct order:
        // 1. Error handling middleware first
        app.UseMiddleware<ErrorHandlingMiddleware>();

        // 2. Authentication middleware next
        app.UseMiddleware<AuthenticationMiddleware>();

        // 3. Logging middleware last
        app.UseMiddleware<LoggingMiddleware>();

        return app;
    }
}

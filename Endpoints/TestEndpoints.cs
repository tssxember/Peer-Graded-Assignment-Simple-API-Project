namespace UserManagementAPI.Endpoints;

/// <summary>
/// Extension methods for configuring testing-related API endpoints.
/// </summary>
public static class TestEndpoints
{
    /// <summary>
    /// Configures all testing-related endpoints.
    /// </summary>
    /// <param name="app">The web application</param>
    /// <returns>The web application for method chaining</returns>
    public static WebApplication MapTestEndpoints(this WebApplication app)
    {
        // Test endpoint to trigger exception for middleware testing
        app.MapGet("/test/error", () =>
        {
            throw new InvalidOperationException("Test exception for middleware validation");
        })
            .WithName("TestError")
            .WithSummary("Test error handling")
            .WithDescription("Triggers an exception to test the error handling middleware")
            .WithTags("Testing");

        return app;
    }
}

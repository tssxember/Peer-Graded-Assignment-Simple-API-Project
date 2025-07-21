using System.Text.Json;

namespace UserManagementAPI.Middleware;

/// <summary>
/// Middleware for validating Bearer tokens and handling authentication.
/// </summary>
public class AuthenticationMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<AuthenticationMiddleware> _logger;

    public AuthenticationMiddleware(RequestDelegate next, ILogger<AuthenticationMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        // Skip authentication for OpenAPI and Swagger endpoints in development
        if (context.Request.Path.StartsWithSegments("/openapi") ||
            context.Request.Path.StartsWithSegments("/swagger"))
        {
            await _next(context);
            return;
        }

        // Check for Authorization header
        if (!context.Request.Headers.ContainsKey("Authorization"))
        {
            _logger.LogWarning("Request to {Path} missing Authorization header", context.Request.Path);
            await WriteUnauthorizedResponse(context);
            return;
        }

        var authHeader = context.Request.Headers["Authorization"].ToString();
        
        // Simple token validation (replace with proper JWT validation in production)
        if (!IsValidToken(authHeader))
        {
            _logger.LogWarning("Invalid token provided for {Path}", context.Request.Path);
            await WriteUnauthorizedResponse(context);
            return;
        }

        await _next(context);
    }

    private static bool IsValidToken(string authHeader)
    {
        // Simple validation - in production, use proper JWT validation
        // Expected format: "Bearer your-valid-token"
        return authHeader.StartsWith("Bearer ") && authHeader.Length > 7;
    }

    private static async Task WriteUnauthorizedResponse(HttpContext context)
    {
        context.Response.StatusCode = 401;
        context.Response.ContentType = "application/json";
        
        var response = new { error = "Unauthorized. Valid token required." };
        var jsonResponse = JsonSerializer.Serialize(response);
        
        await context.Response.WriteAsync(jsonResponse);
    }
}

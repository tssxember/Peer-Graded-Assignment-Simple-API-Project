var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddOpenApi();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "User Management API", Version = "v1" });
    
    // Add security definition for Bearer token
    c.AddSecurityDefinition("Bearer", new()
    {
        Name = "Authorization",
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "Token",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Description = "Enter 'Bearer' followed by a space and your token"
    });
    
    // Add security requirement
    c.AddSecurityRequirement(new()
    {
        {
            new()
            {
                Reference = new()
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline in the correct order
// 1. Error handling middleware first
app.UseMiddleware<ErrorHandlingMiddleware>();

// 2. Authentication middleware next
app.UseMiddleware<AuthenticationMiddleware>();

// 3. Logging middleware last
app.UseMiddleware<LoggingMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "User Management API v1");
        c.RoutePrefix = string.Empty; // Serve Swagger UI at the app's root
    });
}

app.UseHttpsRedirection();

// Initialize in-memory storage for demonstration (replace with database in production)
var users = new List<User>
{
    new User { Id = 1, Name = "John Doe", Email = "john.doe@example.com", Age = 30 },
    new User { Id = 2, Name = "Jane Smith", Email = "jane.smith@example.com", Age = 25 }
};

// User Endpoints
// GET: Retrieve all users
app.MapGet("/users", () => users)
    .WithName("GetAllUsers")
    .WithSummary("Get all users")
    .WithDescription("Retrieves a list of all users in the system")
    .WithTags("Users");

// GET: Retrieve a specific user by ID
app.MapGet("/users/{id}", (int id) =>
{
    var user = users.FirstOrDefault(u => u.Id == id);
    return user is not null ? Results.Ok(user) : Results.NotFound();
})
    .WithName("GetUserById")
    .WithSummary("Get user by ID")
    .WithDescription("Retrieves a specific user by their ID")
    .WithTags("Users");

// POST: Add a new user
app.MapPost("/users", (User user) =>
{
    user.Id = users.Count > 0 ? users.Max(u => u.Id) + 1 : 1;
    users.Add(user);
    return Results.Created($"/users/{user.Id}", user);
})
    .WithName("CreateUser")
    .WithSummary("Create a new user")
    .WithDescription("Creates a new user in the system")
    .WithTags("Users");

// PUT: Update an existing user
app.MapPut("/users/{id}", (int id, User updatedUser) =>
{
    var user = users.FirstOrDefault(u => u.Id == id);
    if (user is null) return Results.NotFound();
    
    user.Name = updatedUser.Name;
    user.Email = updatedUser.Email;
    user.Age = updatedUser.Age;
    
    return Results.Ok(user);
})
    .WithName("UpdateUser")
    .WithSummary("Update an existing user")
    .WithDescription("Updates an existing user's information")
    .WithTags("Users");

// DELETE: Remove a user by ID
app.MapDelete("/users/{id}", (int id) =>
{
    var user = users.FirstOrDefault(u => u.Id == id);
    if (user is null) return Results.NotFound();
    
    users.Remove(user);
    return Results.NoContent();
})
    .WithName("DeleteUser")
    .WithSummary("Delete a user")
    .WithDescription("Removes a user from the system")
    .WithTags("Users");

// Test endpoint to trigger exception for middleware testing
app.MapGet("/test/error", () =>
{
    throw new InvalidOperationException("Test exception for middleware validation");
})
    .WithName("TestError")
    .WithSummary("Test error handling")
    .WithDescription("Triggers an exception to test the error handling middleware")
    .WithTags("Testing");

app.Run();

// User model
public class User
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public int Age { get; set; }
}

// Logging Middleware
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

// Error Handling Middleware
public class ErrorHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ErrorHandlingMiddleware> _logger;

    public ErrorHandlingMiddleware(RequestDelegate next, ILogger<ErrorHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unhandled exception occurred");
            await HandleExceptionAsync(context, ex);
        }
    }

    private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = 500;

        var response = new { error = "Internal server error." };
        var jsonResponse = System.Text.Json.JsonSerializer.Serialize(response);

        await context.Response.WriteAsync(jsonResponse);
    }
}

// Authentication Middleware
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
        // Skip authentication for OpenAPI, Swagger endpoints, and root path (Swagger UI) in development
        if (context.Request.Path.StartsWithSegments("/openapi") ||
            context.Request.Path.StartsWithSegments("/swagger") ||
            context.Request.Path == "/")
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
        var jsonResponse = System.Text.Json.JsonSerializer.Serialize(response);
        
        await context.Response.WriteAsync(jsonResponse);
    }
}

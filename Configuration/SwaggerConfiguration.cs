namespace UserManagementAPI.Configuration;

/// <summary>
/// Extension methods for configuring Swagger in the application.
/// </summary>
public static class SwaggerConfiguration
{
    /// <summary>
    /// Configures Swagger services.
    /// </summary>
    /// <param name="services">The service collection</param>
    /// <returns>The service collection for method chaining</returns>
    public static IServiceCollection AddSwaggerConfiguration(this IServiceCollection services)
    {
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(c =>
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

        return services;
    }

    /// <summary>
    /// Configures Swagger UI middleware.
    /// </summary>
    /// <param name="app">The application builder</param>
    /// <returns>The application builder for method chaining</returns>
    public static IApplicationBuilder UseSwaggerConfiguration(this IApplicationBuilder app)
    {
        app.UseSwagger();
        app.UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint("/swagger/v1/swagger.json", "User Management API v1");
            c.RoutePrefix = string.Empty; // Serve Swagger UI at the app's root
        });

        return app;
    }
}

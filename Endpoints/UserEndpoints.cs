using UserManagementAPI.Models;

namespace UserManagementAPI.Endpoints;

/// <summary>
/// Extension methods for configuring user-related API endpoints.
/// </summary>
public static class UserEndpoints
{
    /// <summary>
    /// Configures all user-related endpoints.
    /// </summary>
    /// <param name="app">The web application</param>
    /// <param name="users">The in-memory user collection</param>
    /// <returns>The web application for method chaining</returns>
    public static WebApplication MapUserEndpoints(this WebApplication app, List<User> users)
    {
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

        return app;
    }
}

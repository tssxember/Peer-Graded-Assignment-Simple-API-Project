# User Management API

A simple ASP.NET Core 9.0 Web API for managing users with authentication, error handling, and logging.

## Features

- CRUD operations for users
- Bearer token authentication  
- Error handling middleware
- Request logging
- Swagger documentation
- In-memory storage

## Getting Started

### Prerequisites
- .NET 9.0 SDK

### Running the Application
```bash
dotnet run
```

Access the API at:
- **HTTPS**: `https://localhost:7001`
- **Swagger UI**: `https://localhost:7001` (root path)

## API Endpoints

| Method | Endpoint | Description | Auth Required |
|--------|----------|-------------|---------------|
| `GET` | `/users` | Get all users | ❌ |
| `GET` | `/users/{id}` | Get user by ID | ❌ |
| `POST` | `/users` | Create user | ✅ |
| `PUT` | `/users/{id}` | Update user | ✅ |
| `DELETE` | `/users/{id}` | Delete user | ✅ |

### User Model
```json
{
  "id": 1,
  "name": "John Doe",
  "email": "john.doe@example.com",
  "age": 30
}
```

## Authentication

For secured endpoints, include a Bearer token in the Authorization header:
```
Authorization: Bearer your-token-here
```

Example request:
```bash
curl -X POST "https://localhost:7001/users" \
  -H "Authorization: Bearer test-token" \
  -H "Content-Type: application/json" \
  -d '{"name": "Alice", "email": "alice@example.com", "age": 28}'
```

## Testing

Use the provided `UserManagementAPI.http` file in VS Code or test manually:

```bash
# Get all users
GET https://localhost:7001/users

# Create user (requires auth)
POST https://localhost:7001/users
Authorization: Bearer test-token
Content-Type: application/json

{
  "name": "Test User",
  "email": "test@example.com", 
  "age": 25
}
```

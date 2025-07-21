# User Management API

A modern ASP.NET Core 9.0 Web API for managing users with built-in authentication, error handling, and logging middleware.

## 📋 Table of Contents

- [Overview](#overview)
- [Features](#features)
- [Prerequisites](#prerequisites)
- [Installation](#installation)
- [Running the Application](#running-the-application)
- [API Endpoints](#api-endpoints)
- [Authentication](#authentication)
- [Middleware](#middleware)
- [Project Structure](#project-structure)
- [Testing](#testing)
- [Contributing](#contributing)

## 🔍 Overview

This User Management API is a RESTful web service built with ASP.NET Core 9.0 that provides basic CRUD operations for user management. The API includes comprehensive middleware for authentication, error handling, and request logging, making it production-ready and secure.

## ✨ Features

- **Full CRUD Operations**: Create, Read, Update, and Delete users
- **Authentication Middleware**: Bearer token authentication
- **Error Handling**: Global exception handling with structured error responses
- **Request Logging**: Comprehensive request/response logging
- **Swagger Documentation**: Interactive API documentation
- **OpenAPI Specification**: Auto-generated API documentation
- **In-Memory Storage**: Simple data persistence for demonstration purposes

## 🛠 Prerequisites

- [.NET 9.0 SDK](https://dotnet.microsoft.com/download/dotnet/9.0)
- [Visual Studio 2022](https://visualstudio.microsoft.com/vs/) or [Visual Studio Code](https://code.visualstudio.com/)
- [Git](https://git-scm.com/) (optional)

## 📦 Installation

1. **Clone the repository** (if using Git):
   ```bash
   git clone <repository-url>
   cd UserManagementAPI
   ```

2. **Restore dependencies**:
   ```bash
   dotnet restore
   ```

3. **Build the project**:
   ```bash
   dotnet build
   ```

## 🚀 Running the Application

### Development Mode

```bash
dotnet run
```

The API will be available at:
- **HTTPS**: `https://localhost:7001`
- **HTTP**: `http://localhost:5099`

### Using Visual Studio

1. Open `UserManagementAPI.sln`
2. Press `F5` or click "Start Debugging"

### Swagger UI

Access the interactive API documentation at:
- `https://localhost:7001/swagger` (HTTPS)
- `http://localhost:5099/swagger` (HTTP)

## 🔗 API Endpoints

### Users

| Method | Endpoint | Description | Authentication Required |
|--------|----------|-------------|------------------------|
| `GET` | `/users` | Get all users | ❌ |
| `GET` | `/users/{id}` | Get user by ID | ❌ |
| `POST` | `/users` | Create a new user | ✅ |
| `PUT` | `/users/{id}` | Update an existing user | ✅ |
| `DELETE` | `/users/{id}` | Delete a user | ✅ |

### Test Endpoints

| Method | Endpoint | Description | Authentication Required |
|--------|----------|-------------|------------------------|
| `GET` | `/test` | Test endpoint | ❌ |
| `GET` | `/test/secure` | Secure test endpoint | ✅ |
| `GET` | `/test/error` | Error test endpoint | ❌ |

### User Model

```json
{
  "id": 1,
  "name": "John Doe",
  "email": "john.doe@example.com",
  "age": 30
}
```

## 🔐 Authentication

The API uses Bearer token authentication for protected endpoints. To access secured endpoints:

1. **Include the Authorization header**:
   ```
   Authorization: Bearer your-token-here
   ```

2. **For testing purposes**, any non-empty token will work in development mode.

### Example Request

```bash
curl -X POST "https://localhost:7001/users" \
  -H "Authorization: Bearer test-token" \
  -H "Content-Type: application/json" \
  -d '{
    "name": "Alice Johnson",
    "email": "alice.johnson@example.com",
    "age": 28
  }'
```

## 🔧 Middleware

The application includes three custom middleware components executed in order:

### 1. Error Handling Middleware
- Catches and handles all unhandled exceptions
- Returns structured error responses
- Logs errors for debugging

### 2. Authentication Middleware
- Validates Bearer tokens for protected endpoints
- Returns 401 Unauthorized for invalid/missing tokens
- Allows anonymous access to public endpoints

### 3. Logging Middleware
- Logs all HTTP requests and responses
- Includes request method, path, status code, and execution time
- Useful for monitoring and debugging

## 📁 Project Structure

```
UserManagementAPI/
├── Configuration/
│   ├── MiddlewareExtensions.cs    # Middleware configuration extensions
│   └── SwaggerConfiguration.cs    # Swagger setup configuration
├── Endpoints/
│   ├── TestEndpoints.cs          # Test-related endpoints
│   └── UserEndpoints.cs          # User management endpoints
├── Middleware/
│   ├── AuthenticationMiddleware.cs   # Bearer token authentication
│   ├── ErrorHandlingMiddleware.cs   # Global error handling
│   └── LoggingMiddleware.cs         # Request/response logging
├── Models/
│   └── User.cs                   # User data model
├── Properties/
│   └── launchSettings.json       # Development settings
├── Program.cs                    # Application entry point
├── UserManagementAPI.csproj     # Project file
├── UserManagementAPI.http       # HTTP test requests
└── README.md                    # This file
```

## 🧪 Testing

### Using the provided HTTP file

The project includes `UserManagementAPI.http` with sample requests:

1. Open the file in Visual Studio Code or Visual Studio
2. Click "Send Request" above each HTTP request
3. View responses in the output panel

### Manual Testing

```bash
# Get all users
curl -X GET "https://localhost:7001/users"

# Create a user (requires authentication)
curl -X POST "https://localhost:7001/users" \
  -H "Authorization: Bearer test-token" \
  -H "Content-Type: application/json" \
  -d '{"name": "Test User", "email": "test@example.com", "age": 25}'

# Get user by ID
curl -X GET "https://localhost:7001/users/1"

# Update user (requires authentication)
curl -X PUT "https://localhost:7001/users/1" \
  -H "Authorization: Bearer test-token" \
  -H "Content-Type: application/json" \
  -d '{"name": "Updated User", "email": "updated@example.com", "age": 26}'

# Delete user (requires authentication)
curl -X DELETE "https://localhost:7001/users/1" \
  -H "Authorization: Bearer test-token"
```

## 🔄 Configuration

### Development Settings

- **Environment**: Development
- **HTTPS Port**: 7001
- **HTTP Port**: 5099
- **Swagger**: Enabled in development mode

### Application Settings

Configuration can be modified in:
- `appsettings.json` - Base configuration
- `appsettings.Development.json` - Development-specific settings

## 📝 Contributing

1. Fork the repository
2. Create a feature branch: `git checkout -b feature-name`
3. Make your changes
4. Add tests if applicable
5. Commit your changes: `git commit -m 'Add some feature'`
6. Push to the branch: `git push origin feature-name`
7. Submit a pull request

## 📄 License

This project is licensed under the MIT License - see the LICENSE file for details.

## 🤝 Support

If you have any questions or issues, please:
1. Check the existing documentation
2. Review the HTTP test file for examples
3. Open an issue in the repository

---

**Note**: This is a demonstration project for learning purposes. For production use, consider implementing:
- Proper database integration
- Enhanced security measures
- Comprehensive validation
- Rate limiting
- Health checks
- Logging to external services

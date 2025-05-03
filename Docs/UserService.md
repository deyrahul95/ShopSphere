
# 🚀 UserService — Clean Architecture (Initial Setup)
## 📂 Project Structure


```md
UserService
│
├── UserService.API (Presentation Layer)
│    ├── Controllers
│    ├── Middlewares
│    ├── Extensions
│    └── Program.cs
│
├── UserService.Application (Application/Business Layer)
│    ├── DTOs
│    ├── Interfaces
│    ├── Services
│
├── UserService.Infrastructure (Infrastructure Layer)
│    ├── MockData
│
├── UserService.Domain (Domain Layer)
│    ├── Models
│
└── UserService.sln
```


## 1️⃣ Domain Layer (UserService.Domain)

### Models/User.cs

```csharp
namespace UserService.Domain.Models;

public class User
{
    public Guid Id { get; set; }
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty; // In real-world hash this
}
```

## 2️⃣ Application Layer (UserService.Application)

### DTOs/LoginRequestDto.cs

```csharp
namespace UserService.Application.DTOs;

public class LoginRequestDto
{
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}
```

### DTOs/LoginResponseDto.cs

```csharp
namespace UserService.Application.DTOs;

public class LoginResponseDto
{
    public string Token { get; set; } = string.Empty;
    public Guid UserId { get; set; }
}
```


### Interfaces/IUserService.cs

```csharp
using UserService.Application.DTOs;

namespace UserService.Application.Interfaces;

public interface IUserService
{
    Task<LoginResponseDto> LoginAsync(LoginRequestDto request);
}
```


### Services/UserService.cs

```csharp
using UserService.Application.DTOs;
using UserService.Application.Interfaces;
using UserService.Domain.Models;
using UserService.Infrastructure.MockData;

namespace UserService.Application.Services;

public class UserService : IUserService
{
    public async Task<LoginResponseDto> LoginAsync(LoginRequestDto request)
    {
        // Simulate database call
        var user = MockUserData.Users
            .FirstOrDefault(u => u.Username == request.Username && u.Password == request.Password);

        if (user == null)
        {
            throw new UnauthorizedAccessException("Invalid username or password.");
        }

        return await Task.FromResult(new LoginResponseDto
        {
            UserId = user.Id,
            Token = "mock-jwt-token"
        });
    }
}
```


## 3️⃣ Infrastructure Layer (UserService.Infrastructure)

### MockData/MockUserData.cs

```csharp
using UserService.Domain.Models;

namespace UserService.Infrastructure.MockData;

public static class MockUserData
{
    public static List<User> Users = new List<User>
    {
        new User { Id = Guid.NewGuid(), Username = "admin", Password = "admin123" },
        new User { Id = Guid.NewGuid(), Username = "user", Password = "user123" }
    };
}
```


## 4️⃣ API Layer (UserService.API)

### Controllers/UserController.cs

```csharp
using Microsoft.AspNetCore.Mvc;
using UserService.Application.DTOs;
using UserService.Application.Interfaces;

namespace UserService.API.Controllers;

[ApiController]
[Route("api/users")]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequestDto request)
    {
        var response = await _userService.LoginAsync(request);
        return Ok(response);
    }
}
```


### Middlewares/ExceptionHandlingMiddleware.cs

```csharp
using System.Net;
using Newtonsoft.Json;

namespace UserService.API.Middlewares;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;

    public ExceptionHandlingMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (UnauthorizedAccessException ex)
        {
            await HandleExceptionAsync(context, ex, HttpStatusCode.Unauthorized);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex, HttpStatusCode.InternalServerError);
        }
    }

    private Task HandleExceptionAsync(HttpContext context, Exception exception, HttpStatusCode statusCode)
    {
        var response = new
        {
            error = exception.Message
        };

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)statusCode;
        return context.Response.WriteAsync(JsonConvert.SerializeObject(response));
    }
}
```


### Extensions/ServiceCollectionExtensions.cs

```csharp
using UserService.Application.Interfaces;
using UserService.Application.Services;

namespace UserService.API.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<IUserService, UserService>();
    }
}
```


### Program.cs

```csharp
using UserService.API.Extensions;
using UserService.API.Middlewares;

var builder = WebApplication.CreateBuilder(args);

// Add services
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddApplicationServices();

var app = builder.Build();

// Configure middleware
app.UseMiddleware<ExceptionHandlingMiddleware>();

// Swagger
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();

app.Run();
```


## 🐳 Dockerfile for UserService
We want it clean, multi-stage (for small image size), production-ready.

Create a file named Dockerfile inside UserService.API project folder.

```dockerfile
# Stage 1: Build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy solution and project files
COPY ["UserService.sln", "./"]
COPY ["UserService.API/UserService.API.csproj", "UserService.API/"]
COPY ["UserService.Application/UserService.Application.csproj", "UserService.Application/"]
COPY ["UserService.Domain/UserService.Domain.csproj", "UserService.Domain/"]
COPY ["UserService.Infrastructure/UserService.Infrastructure.csproj", "UserService.Infrastructure/"]

# Restore dependencies
RUN dotnet restore "UserService.API/UserService.API.csproj"

# Copy all files and build
COPY . .
WORKDIR "/src/UserService.API"
RUN dotnet publish "UserService.API.csproj" -c Release -o /app/publish

# Stage 2: Run
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/publish .

# Expose Port
EXPOSE 80

# Start the app
ENTRYPOINT ["dotnet", "UserService.API.dll"]
```


### 🛠️ How to Build and Push Image (DockerHub)

-  1. Build the Docker Image

```bash
docker build -t <your-dockerhub-username>/userservice:latest .
```

 - 2. Login to Docker Hub
 
```bash
docker login
```

 - 3. Push the image

```bash
docker push <your-dockerhub-username>/userservice:latest
```


#### Example:

```bash
docker build -t rahulsharma/userservice:latest .
docker push rahulsharma/userservice:latest
```

## 🛠️ Health Check (Recommended)
👉 Add this endpoint to UserController.cs for container health monitoring (best practice):

```csharp
[HttpGet("health")]
public IActionResult HealthCheck()
{
    return Ok("UserService is up and running");
}
```

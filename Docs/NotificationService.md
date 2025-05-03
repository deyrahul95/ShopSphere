
# 📢 NotificationService — (Simple Logger Microservice)

## 📂 Project Structure

```md
NotificationService
│
├── NotificationService.API
│    ├── Controllers
│    └── Program.cs
│
├── NotificationService.Application
│    ├── DTOs
│    ├── Interfaces
│    ├── Services
│
└── NotificationService.sln
```


## 1️⃣ Domain Layer - (Not needed — no models, just logging text.)

## 2️⃣ Application Layer (NotificationService.Application)

### DTOs/NotificationRequestDto.cs

```csharp
namespace NotificationService.Application.DTOs;

public class NotificationRequestDto
{
    public Guid UserId { get; set; }
    public string Message { get; set; } = string.Empty;
}
```


### Interfaces/INotificationService.cs

```csharp
using NotificationService.Application.DTOs;

namespace NotificationService.Application.Interfaces;

public interface INotificationService
{
    Task SendNotificationAsync(NotificationRequestDto request);
}
```

### Services/NotificationService.cs

```csharp
using NotificationService.Application.DTOs;
using NotificationService.Application.Interfaces;

namespace NotificationService.Application.Services;

public class NotificationService : INotificationService
{
    public async Task SendNotificationAsync(NotificationRequestDto request)
    {
        Console.WriteLine($"[NotificationService] User: {request.UserId} - {request.Message}");
        await Task.CompletedTask;
    }
}
```


## 3️⃣ API Layer (NotificationService.API)

### Controllers/NotificationController.cs

```csharp
using Microsoft.AspNetCore.Mvc;
using NotificationService.Application.DTOs;
using NotificationService.Application.Interfaces;

namespace NotificationService.API.Controllers;

[ApiController]
[Route("api/notifications")]
public class NotificationController : ControllerBase
{
    private readonly INotificationService _notificationService;

    public NotificationController(INotificationService notificationService)
    {
        _notificationService = notificationService;
    }

    [HttpPost]
    public async Task<IActionResult> SendNotification([FromBody] NotificationRequestDto request)
    {
        await _notificationService.SendNotificationAsync(request);
        return Ok("Notification sent.");
    }

    [HttpGet("health")]
    public IActionResult HealthCheck()
    {
        return Ok("NotificationService is healthy");
    }
}
```

## 🐳 Dockerfile for NotificationService

```dockerfile
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ["NotificationService.sln", "./"]
COPY ["NotificationService.API/NotificationService.API.csproj", "NotificationService.API/"]
COPY ["NotificationService.Application/NotificationService.Application.csproj", "NotificationService.Application/"]

RUN dotnet restore "NotificationService.API/NotificationService.API.csproj"
COPY . .
WORKDIR "/src/NotificationService.API"
RUN dotnet publish "NotificationService.API.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/publish .

EXPOSE 80
ENTRYPOINT ["dotnet", "NotificationService.API.dll"]
```
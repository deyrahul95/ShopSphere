# 🛒 InventoryService — (Simple In-Memory Stock Checker)

## 📂 Project Structure

```md
InventoryService
│
├── InventoryService.API
│    ├── Controllers
│    └── Program.cs
│
├── InventoryService.Application
│    ├── DTOs
│    ├── Interfaces
│    ├── Services
│
├── InventoryService.Infrastructure
│    ├── MockData
│
└── InventoryService.sln
```

## 1️⃣ Application Layer (InventoryService.Application)

### DTOs/InventoryCheckRequestDto.cs

```csharp
namespace InventoryService.Application.DTOs;

public class InventoryCheckRequestDto
{
    public Guid ProductId { get; set; }
    public int Quantity { get; set; }
}
```

### Interfaces/IInventoryService.cs

```csharp
using InventoryService.Application.DTOs;

namespace InventoryService.Application.Interfaces;

public interface IInventoryService
{
    Task<bool> CheckInventoryAsync(InventoryCheckRequestDto request);
}
```

### Services/InventoryService.cs

```csharp
using InventoryService.Application.DTOs;
using InventoryService.Application.Interfaces;
using InventoryService.Infrastructure.MockData;

namespace InventoryService.Application.Services;

public class InventoryService : IInventoryService
{
    public async Task<bool> CheckInventoryAsync(InventoryCheckRequestDto request)
    {
        var stock = MockInventoryData.ProductsStock
            .FirstOrDefault(p => p.ProductId == request.ProductId)?.AvailableQuantity ?? 0;

        return await Task.FromResult(stock >= request.Quantity);
    }
}
```

## 2️⃣ Infrastructure Layer (InventoryService.Infrastructure)

### MockData/MockInventoryData.cs

```csharp
namespace InventoryService.Infrastructure.MockData;

public class ProductStock
{
    public Guid ProductId { get; set; }
    public int AvailableQuantity { get; set; }
}

public static class MockInventoryData
{
    public static List<ProductStock> ProductsStock = new()
    {
        new ProductStock { ProductId = Guid.Parse("11111111-1111-1111-1111-111111111111"), AvailableQuantity = 10 },
        new ProductStock { ProductId = Guid.Parse("22222222-2222-2222-2222-222222222222"), AvailableQuantity = 5 }
    };
}
```


## 3️⃣ API Layer (InventoryService.API)

### Controllers/InventoryController.cs

```csharp
using Microsoft.AspNetCore.Mvc;
using InventoryService.Application.DTOs;
using InventoryService.Application.Interfaces;

namespace InventoryService.API.Controllers;

[ApiController]
[Route("api/inventory")]
public class InventoryController : ControllerBase
{
    private readonly IInventoryService _inventoryService;

    public InventoryController(IInventoryService inventoryService)
    {
        _inventoryService = inventoryService;
    }

    [HttpPost("check")]
    public async Task<IActionResult> CheckInventory([FromBody] InventoryCheckRequestDto request)
    {
        var result = await _inventoryService.CheckInventoryAsync(request);
        return Ok(new { Available = result });
    }

    [HttpGet("health")]
    public IActionResult HealthCheck()
    {
        return Ok("InventoryService is healthy");
    }
}
```

## 🐳 Dockerfile for InventoryService

```dockerfile
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ["InventoryService.sln", "./"]
COPY ["InventoryService.API/InventoryService.API.csproj", "InventoryService.API/"]
COPY ["InventoryService.Application/InventoryService.Application.csproj", "InventoryService.Application/"]
COPY ["InventoryService.Infrastructure/InventoryService.Infrastructure.csproj", "InventoryService.Infrastructure/"]

RUN dotnet restore "InventoryService.API/InventoryService.API.csproj"
COPY . .
WORKDIR "/src/InventoryService.API"
RUN dotnet publish "InventoryService.API.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/publish .

EXPOSE 80
ENTRYPOINT ["dotnet", "InventoryService.API.dll"]
```

# 🛒📦 OrderService — Clean Production-Grade Setup

## 📂 Project Structure

```md
OrderService
│
├── OrderService.API
│    ├── Controllers
│    ├── Middlewares
│    ├── Extensions
│    └── Program.cs
│
├── OrderService.Application
│    ├── DTOs
│    ├── Interfaces
│    ├── Services
│
├── OrderService.Infrastructure
│    ├── MockClients (For calling Inventory/Payment)
│
├── OrderService.Domain
│    ├── Models
│
└── OrderService.sln
```


## 1️⃣ Domain Layer (OrderService.Domain)


### Models/OrderItem.cs

```csharp
namespace OrderService.Domain.Models;

public class OrderItem
{
    public Guid ProductId { get; set; }
    public int Quantity { get; set; }
    public decimal Price { get; set; }
}
```


### Models/Order.cs

```csharp
namespace OrderService.Domain.Models;

public class Order
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public List<OrderItem> Items { get; set; } = new();
    public decimal TotalAmount { get; set; }
    public string Status { get; set; } = "Pending"; // Confirmed, Cancelled
}
```


## 2️⃣ Application Layer (OrderService.Application)

### DTOs/CreateOrderRequestDto.cs

```csharp
namespace OrderService.Application.DTOs;

public class CreateOrderRequestDto
{
    public Guid UserId { get; set; }
    public List<OrderItemDto> Items { get; set; } = new();
}

public class OrderItemDto
{
    public Guid ProductId { get; set; }
    public int Quantity { get; set; }
    public decimal Price { get; set; }
}
```

### Interfaces/IOrderService.cs

```csharp
using OrderService.Application.DTOs;
using OrderService.Domain.Models;

namespace OrderService.Application.Interfaces;

public interface IOrderService
{
    Task<Order> CreateOrderAsync(CreateOrderRequestDto request);
}
```


### Services/OrderService.cs

```csharp
using OrderService.Application.DTOs;
using OrderService.Application.Interfaces;
using OrderService.Domain.Models;
using OrderService.Infrastructure.MockClients;

namespace OrderService.Application.Services;

public class OrderService : IOrderService
{
    public async Task<Order> CreateOrderAsync(CreateOrderRequestDto request)
    {
        var order = new Order
        {
            Id = Guid.NewGuid(),
            UserId = request.UserId,
            Items = request.Items.Select(x => new OrderItem
            {
                ProductId = x.ProductId,
                Quantity = x.Quantity,
                Price = x.Price
            }).ToList(),
            TotalAmount = request.Items.Sum(x => x.Price * x.Quantity),
            Status = "Pending"
        };

        var stockAvailable = await MockInventoryClient.CheckStockAsync(request.Items);
        var paymentSuccess = await MockPaymentClient.ProcessPaymentAsync(order.TotalAmount);

        if (stockAvailable && paymentSuccess)
        {
            order.Status = "Confirmed";
            MockOrderData.Orders.Add(order);

            Console.WriteLine($"Notification: Order {order.Id} confirmed for User {order.UserId}");
        }
        else
        {
            order.Status = "Cancelled";
            Console.WriteLine($"Notification: Order {order.Id} cancelled for User {order.UserId}");
        }

        return order;
    }
}
```


## 3️⃣ Infrastructure Layer (OrderService.Infrastructure)

### MockClients/MockInventoryClient.cs

```csharp
using OrderService.Application.DTOs;

namespace OrderService.Infrastructure.MockClients;

public static class MockInventoryClient
{
    public static async Task<bool> CheckStockAsync(List<OrderItemDto> items)
    {
        // Mock check: assume stock always available unless quantity > 5
        var inStock = items.All(x => x.Quantity <= 5);
        return await Task.FromResult(inStock);
    }
}
```


### MockClients/MockPaymentClient.cs

```csharp
namespace OrderService.Infrastructure.MockClients;

public static class MockPaymentClient
{
    public static async Task<bool> ProcessPaymentAsync(decimal amount)
    {
        // Mock payment: fail if amount > 5000
        var success = amount <= 5000;
        return await Task.FromResult(success);
    }
}
```

### MockData/MockOrderData.cs

```csharp
using OrderService.Domain.Models;

namespace OrderService.Infrastructure.MockData;

public static class MockOrderData
{
    public static List<Order> Orders = new List<Order>();
}
```


## 4️⃣ API Layer (OrderService.API)

### Controllers/OrderController.cs

```csharp
using Microsoft.AspNetCore.Mvc;
using OrderService.Application.DTOs;
using OrderService.Application.Interfaces;

namespace OrderService.API.Controllers;

[ApiController]
[Route("api/orders")]
public class OrderController : ControllerBase
{
    private readonly IOrderService _orderService;

    public OrderController(IOrderService orderService)
    {
        _orderService = orderService;
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateOrderRequestDto request)
    {
        var order = await _orderService.CreateOrderAsync(request);
        return Ok(order);
    }

    [HttpGet("health")]
    public IActionResult HealthCheck()
    {
        return Ok("OrderService is healthy");
    }
}
```

## 🐳 Dockerfile for OrderService

```dockerfile
# Stage 1: Build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ["OrderService.sln", "./"]
COPY ["OrderService.API/OrderService.API.csproj", "OrderService.API/"]
COPY ["OrderService.Application/OrderService.Application.csproj", "OrderService.Application/"]
COPY ["OrderService.Domain/OrderService.Domain.csproj", "OrderService.Domain/"]
COPY ["OrderService.Infrastructure/OrderService.Infrastructure.csproj", "OrderService.Infrastructure/"]

RUN dotnet restore "OrderService.API/OrderService.API.csproj"
COPY . .
WORKDIR "/src/OrderService.API"
RUN dotnet publish "OrderService.API.csproj" -c Release -o /app/publish

# Stage 2: Run
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/publish .

EXPOSE 80
ENTRYPOINT ["dotnet", "OrderService.API.dll"]
```

# 🛒 CartService — Clean Setup

## 📂 Project Structure

```md
CartService
│
├── CartService.API
│    ├── Controllers
│    ├── Middlewares
│    ├── Extensions
│    └── Program.cs
│
├── CartService.Application
│    ├── DTOs
│    ├── Interfaces
│    ├── Services
│
├── CartService.Infrastructure
│    ├── MockData
│
├── CartService.Domain
│    ├── Models
│
└── CartService.sln
```

## 1️⃣ Domain Layer (CartService.Domain)

### Models/CartItem.cs

```csharp
namespace CartService.Domain.Models;

public class CartItem
{
    public Guid ProductId { get; set; }
    public string ProductName { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public decimal Price { get; set; }
}
```


### Models/Cart.cs

```csharp
namespace CartService.Domain.Models;

public class Cart
{
    public Guid UserId { get; set; }
    public List<CartItem> Items { get; set; } = new List<CartItem>();
}
```


## 2️⃣ Application Layer (CartService.Application)

### DTOs/AddToCartRequestDto.cs

```csharp
namespace CartService.Application.DTOs;

public class AddToCartRequestDto
{
    public Guid ProductId { get; set; }
    public string ProductName { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public decimal Price { get; set; }
}
```


### Interfaces/ICartService.cs

```csharp
using CartService.Application.DTOs;
using CartService.Domain.Models;

namespace CartService.Application.Interfaces;

public interface ICartService
{
    Task AddToCartAsync(Guid userId, AddToCartRequestDto item);
    Task<Cart> GetCartAsync(Guid userId);
    Task ClearCartAsync(Guid userId);
}
```


### Services/CartService.cs

```csharp
using CartService.Application.DTOs;
using CartService.Application.Interfaces;
using CartService.Domain.Models;
using CartService.Infrastructure.MockData;

namespace CartService.Application.Services;

public class CartService : ICartService
{
    public async Task AddToCartAsync(Guid userId, AddToCartRequestDto item)
    {
        var cart = MockCartData.Carts.FirstOrDefault(c => c.UserId == userId);
        if (cart == null)
        {
            cart = new Cart { UserId = userId };
            MockCartData.Carts.Add(cart);
        }

        var existingItem = cart.Items.FirstOrDefault(p => p.ProductId == item.ProductId);
        if (existingItem != null)
        {
            existingItem.Quantity += item.Quantity;
        }
        else
        {
            cart.Items.Add(new CartItem
            {
                ProductId = item.ProductId,
                ProductName = item.ProductName,
                Quantity = item.Quantity,
                Price = item.Price
            });
        }

        await Task.CompletedTask;
    }

    public async Task<Cart> GetCartAsync(Guid userId)
    {
        var cart = MockCartData.Carts.FirstOrDefault(c => c.UserId == userId) ?? new Cart { UserId = userId };
        return await Task.FromResult(cart);
    }

    public async Task ClearCartAsync(Guid userId)
    {
        var cart = MockCartData.Carts.FirstOrDefault(c => c.UserId == userId);
        if (cart != null)
        {
            cart.Items.Clear();
        }
        await Task.CompletedTask;
    }
}
```


## 3️⃣ Infrastructure Layer (CartService.Infrastructure)

### MockData/MockCartData.cs

```csharp
using CartService.Domain.Models;

namespace CartService.Infrastructure.MockData;

public static class MockCartData
{
    public static List<Cart> Carts = new List<Cart>();
}
```


## 4️⃣ API Layer (CartService.API)

### Controllers/CartController.cs

```csharp
using Microsoft.AspNetCore.Mvc;
using CartService.Application.DTOs;
using CartService.Application.Interfaces;

namespace CartService.API.Controllers;

[ApiController]
[Route("api/cart")]
public class CartController : ControllerBase
{
    private readonly ICartService _cartService;

    public CartController(ICartService cartService)
    {
        _cartService = cartService;
    }

    [HttpPost("{userId}")]
    public async Task<IActionResult> AddToCart(Guid userId, [FromBody] AddToCartRequestDto request)
    {
        await _cartService.AddToCartAsync(userId, request);
        return Ok("Item added to cart.");
    }

    [HttpGet("{userId}")]
    public async Task<IActionResult> GetCart(Guid userId)
    {
        var cart = await _cartService.GetCartAsync(userId);
        return Ok(cart);
    }

    [HttpDelete("{userId}")]
    public async Task<IActionResult> ClearCart(Guid userId)
    {
        await _cartService.ClearCartAsync(userId);
        return Ok("Cart cleared.");
    }

    [HttpGet("health")]
    public IActionResult HealthCheck()
    {
        return Ok("CartService is healthy");
    }
}
```


## 🐳 Dockerfile for CartService

```dockerfile
# Stage 1: Build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ["CartService.sln", "./"]
COPY ["CartService.API/CartService.API.csproj", "CartService.API/"]
COPY ["CartService.Application/CartService.Application.csproj", "CartService.Application/"]
COPY ["CartService.Domain/CartService.Domain.csproj", "CartService.Domain/"]
COPY ["CartService.Infrastructure/CartService.Infrastructure.csproj", "CartService.Infrastructure/"]

RUN dotnet restore "CartService.API/CartService.API.csproj"
COPY . .
WORKDIR "/src/CartService.API"
RUN dotnet publish "CartService.API.csproj" -c Release -o /app/publish

# Stage 2: Run
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/publish .

EXPOSE 80
ENTRYPOINT ["dotnet", "CartService.API.dll"]
```


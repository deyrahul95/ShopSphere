# 🚀 ProductService — Clean Setup (High Code Quality)
## 📂 Project Structure

```md
ProductService
│
├── ProductService.API (Presentation Layer)
│    ├── Controllers
│    ├── Middlewares
│    ├── Extensions
│    └── Program.cs
│
├── ProductService.Application (Business Layer)
│    ├── DTOs
│    ├── Interfaces
│    ├── Services
│
├── ProductService.Infrastructure (Infra Layer)
│    ├── MockData
│
├── ProductService.Domain (Domain Layer)
│    ├── Models
│
└── ProductService.sln
```


## 1️⃣ Domain Layer (ProductService.Domain)

### Models/Product.cs

```csharp
namespace ProductService.Domain.Models;

public class Product
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public string Category { get; set; } = string.Empty;
    public bool InStock { get; set; }
}
```


## 2️⃣ Application Layer (ProductService.Application)

### DTOs/ProductResponseDto.cs

```csharp
namespace ProductService.Application.DTOs;

public class ProductResponseDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public bool InStock { get; set; }
}
```

### Interfaces/IProductService.cs

```csharp
using ProductService.Application.DTOs;

namespace ProductService.Application.Interfaces;

public interface IProductService
{
    Task<IEnumerable<ProductResponseDto>> SearchProductsAsync(string? name, string? category, int page, int pageSize);
    Task<ProductResponseDto> GetProductByIdAsync(Guid id);
}
```


#### Services/ProductService.cs

```csharp
using ProductService.Application.DTOs;
using ProductService.Application.Interfaces;
using ProductService.Domain.Models;
using ProductService.Infrastructure.MockData;

namespace ProductService.Application.Services;

public class ProductService : IProductService
{
    public async Task<IEnumerable<ProductResponseDto>> SearchProductsAsync(string? name, string? category, int page, int pageSize)
    {
        var products = MockProductData.Products
            .Where(p => (string.IsNullOrEmpty(name) || p.Name.Contains(name, StringComparison.OrdinalIgnoreCase)) &&
                        (string.IsNullOrEmpty(category) || p.Category.Equals(category, StringComparison.OrdinalIgnoreCase)))
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(p => new ProductResponseDto
            {
                Id = p.Id,
                Name = p.Name,
                Price = p.Price,
                InStock = p.InStock
            });

        return await Task.FromResult(products);
    }

    public async Task<ProductResponseDto> GetProductByIdAsync(Guid id)
    {
        var product = MockProductData.Products.FirstOrDefault(p => p.Id == id);

        if (product == null)
            throw new KeyNotFoundException("Product not found.");

        return await Task.FromResult(new ProductResponseDto
        {
            Id = product.Id,
            Name = product.Name,
            Price = product.Price,
            InStock = product.InStock
        });
    }
}
```


## 3️⃣ Infrastructure Layer (ProductService.Infrastructure)


### MockData/MockProductData.cs

```csharp
using ProductService.Domain.Models;

namespace ProductService.Infrastructure.MockData;

public static class MockProductData
{
    public static List<Product> Products = new List<Product>
    {
        new Product { Id = Guid.NewGuid(), Name = "iPhone 15", Description = "Apple smartphone", Price = 999.99M, Category = "Electronics", InStock = true },
        new Product { Id = Guid.NewGuid(), Name = "Galaxy S23", Description = "Samsung smartphone", Price = 899.99M, Category = "Electronics", InStock = true },
        new Product { Id = Guid.NewGuid(), Name = "Sony WH-1000XM5", Description = "Noise cancelling headphones", Price = 349.99M, Category = "Accessories", InStock = false }
    };
}
```


## 4️⃣ API Layer (ProductService.API)

### Controllers/ProductController.cs

```csharp
using Microsoft.AspNetCore.Mvc;
using ProductService.Application.Interfaces;

namespace ProductService.API.Controllers;

[ApiController]
[Route("api/products")]
public class ProductController : ControllerBase
{
    private readonly IProductService _productService;

    public ProductController(IProductService productService)
    {
        _productService = productService;
    }

    [HttpGet]
    public async Task<IActionResult> Search([FromQuery] string? name, [FromQuery] string? category, [FromQuery] int page = 1, [FromQuery] int pageSize = 10)
    {
        var result = await _productService.SearchProductsAsync(name, category, page, pageSize);
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var product = await _productService.GetProductByIdAsync(id);
        return Ok(product);
    }

    [HttpGet("health")]
    public IActionResult HealthCheck()
    {
        return Ok("ProductService is healthy");
    }
}
```


### Middlewares/ExceptionHandlingMiddleware.cs

```csharp
using System.Net;
using Newtonsoft.Json;

namespace ProductService.API.Middlewares;

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
        catch (KeyNotFoundException ex)
        {
            await HandleExceptionAsync(context, ex, HttpStatusCode.NotFound);
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
using ProductService.Application.Interfaces;
using ProductService.Application.Services;

namespace ProductService.API.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<IProductService, ProductService>();
    }
}
```


### Program.cs

```csharp
using ProductService.API.Extensions;
using ProductService.API.Middlewares;

var builder = WebApplication.CreateBuilder(args);

// Services
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddApplicationServices();

var app = builder.Build();

// Middlewares
app.UseMiddleware<ExceptionHandlingMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();
app.Run();
```


## 🐳 Dockerfile for ProductService
Create Dockerfile inside ProductService.API folder:

```dockerfile
# Stage 1: Build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ["ProductService.sln", "./"]
COPY ["ProductService.API/ProductService.API.csproj", "ProductService.API/"]
COPY ["ProductService.Application/ProductService.Application.csproj", "ProductService.Application/"]
COPY ["ProductService.Domain/ProductService.Domain.csproj", "ProductService.Domain/"]
COPY ["ProductService.Infrastructure/ProductService.Infrastructure.csproj", "ProductService.Infrastructure/"]

RUN dotnet restore "ProductService.API/ProductService.API.csproj"
COPY . .
WORKDIR "/src/ProductService.API"
RUN dotnet publish "ProductService.API.csproj" -c Release -o /app/publish

# Stage 2: Run
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/publish .

EXPOSE 80
ENTRYPOINT ["dotnet", "ProductService.API.dll"]
```

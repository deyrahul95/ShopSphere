
# 💳 PaymentService — (Mock Simple Payment Validator)
## 📂 Project Structure

```md
PaymentService
│
├── PaymentService.API
│    ├── Controllers
│    └── Program.cs
│
├── PaymentService.Application
│    ├── DTOs
│    ├── Interfaces
│    ├── Services
│
└── PaymentService.sln
```

## 1️⃣ Application Layer (PaymentService.Application)

### DTOs/PaymentRequestDto.cs

```csharp
namespace PaymentService.Application.DTOs;

public class PaymentRequestDto
{
    public Guid UserId { get; set; }
    public decimal Amount { get; set; }
}
```

### Interfaces/IPaymentService.cs

```csharp
using PaymentService.Application.DTOs;

namespace PaymentService.Application.Interfaces;

public interface IPaymentService
{
    Task<bool> ProcessPaymentAsync(PaymentRequestDto request);
}
```

### Services/PaymentService.cs

```csharp
using PaymentService.Application.DTOs;
using PaymentService.Application.Interfaces;

namespace PaymentService.Application.Services;

public class PaymentService : IPaymentService
{
    public async Task<bool> ProcessPaymentAsync(PaymentRequestDto request)
    {
        var isSuccess = request.Amount <= 5000;
        return await Task.FromResult(isSuccess);
    }
}
```


## 2️⃣ API Layer (PaymentService.API)

### Controllers/PaymentController.cs

```csharp
using Microsoft.AspNetCore.Mvc;
using PaymentService.Application.DTOs;
using PaymentService.Application.Interfaces;

namespace PaymentService.API.Controllers;

[ApiController]
[Route("api/payments")]
public class PaymentController : ControllerBase
{
    private readonly IPaymentService _paymentService;

    public PaymentController(IPaymentService paymentService)
    {
        _paymentService = paymentService;
    }

    [HttpPost]
    public async Task<IActionResult> ProcessPayment([FromBody] PaymentRequestDto request)
    {
        var result = await _paymentService.ProcessPaymentAsync(request);
        return Ok(new { PaymentSuccessful = result });
    }

    [HttpGet("health")]
    public IActionResult HealthCheck()
    {
        return Ok("PaymentService is healthy");
    }
}
```

## 🐳 Dockerfile for PaymentService

```dockerfile
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ["PaymentService.sln", "./"]
COPY ["PaymentService.API/PaymentService.API.csproj", "PaymentService.API/"]
COPY ["PaymentService.Application/PaymentService.Application.csproj", "PaymentService.Application/"]

RUN dotnet restore "PaymentService.API/PaymentService.API.csproj"
COPY . .
WORKDIR "/src/PaymentService.API"
RUN dotnet publish "PaymentService.API.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/publish .

EXPOSE 80
ENTRYPOINT ["dotnet", "PaymentService.API.dll"]
```
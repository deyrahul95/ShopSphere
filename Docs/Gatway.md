# 🐳 Step-by-Step: docker-compose.yml + API Gateway + Eureka
## 🧠 Big Picture


| Component       | Purpose                           |
|-----------------|-----------------------------------|
| Ocelot Gateway  | API Gateway for all requests      |
| Eureka Server   | Service Discovery                 |
| Eureka Clients  | All microservices                 |
| Docker Compose  | Boot everything from one file     |


## 📂 Folder Structure at Root

```md
/src
   - ProductService/
   - CartService/
   - OrderService/
   - UserService/
   - InventoryService/
   - PaymentService/
   - NotificationService/
   - OcelotGateway/
   - EurekaServer/
docker-compose.yml
```


# 1️⃣ Ocelot Gateway (API Gateway)
## 📂 OcelotGateway Project Setup

### Program.cs

```csharp
using Ocelot.DependencyInjection;
using Ocelot.Middleware;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddJsonFile("ocelot.json");
builder.Services.AddOcelot();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();
app.UseOcelot().Wait();
app.Run();
```


### ocelot.json

```json
{
  "Routes": [
    {
      "DownstreamPathTemplate": "/api/products/{everything}",
      "DownstreamScheme": "http",
      "DownstreamHostAndPorts": [
        { "Host": "productservice", "Port": 80 }
      ],
      "UpstreamPathTemplate": "/products/{everything}",
      "UpstreamHttpMethod": [ "GET", "POST" ]
    },
    {
      "DownstreamPathTemplate": "/api/cart/{everything}",
      "DownstreamScheme": "http",
      "DownstreamHostAndPorts": [
        { "Host": "cartservice", "Port": 80 }
      ],
      "UpstreamPathTemplate": "/cart/{everything}",
      "UpstreamHttpMethod": [ "GET", "POST" ]
    },
    {
      "DownstreamPathTemplate": "/api/orders/{everything}",
      "DownstreamScheme": "http",
      "DownstreamHostAndPorts": [
        { "Host": "orderservice", "Port": 80 }
      ],
      "UpstreamPathTemplate": "/orders/{everything}",
      "UpstreamHttpMethod": [ "GET", "POST" ]
    },
    {
      "DownstreamPathTemplate": "/api/notifications/{everything}",
      "DownstreamScheme": "http",
      "DownstreamHostAndPorts": [
        { "Host": "notificationservice", "Port": 80 }
      ],
      "UpstreamPathTemplate": "/notifications/{everything}",
      "UpstreamHttpMethod": [ "POST" ]
    },
    {
      "DownstreamPathTemplate": "/api/inventory/{everything}",
      "DownstreamScheme": "http",
      "DownstreamHostAndPorts": [
        { "Host": "inventoryservice", "Port": 80 }
      ],
      "UpstreamPathTemplate": "/inventory/{everything}",
      "UpstreamHttpMethod": [ "POST" ]
    },
    {
      "DownstreamPathTemplate": "/api/payments/{everything}",
      "DownstreamScheme": "http",
      "DownstreamHostAndPorts": [
        { "Host": "paymentservice", "Port": 80 }
      ],
      "UpstreamPathTemplate": "/payments/{everything}",
      "UpstreamHttpMethod": [ "POST" ]
    }
  ],
  "GlobalConfiguration": {
    "BaseUrl": "http://localhost:5000"
  }
}
```


## Dockerfile for OcelotGateway

```dockerfile
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ["OcelotGateway/OcelotGateway.csproj", "OcelotGateway/"]
RUN dotnet restore "OcelotGateway/OcelotGateway.csproj"
COPY . .
WORKDIR "/src/OcelotGateway"
RUN dotnet publish "OcelotGateway.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/publish .

EXPOSE 5000
ENTRYPOINT ["dotnet", "OcelotGateway.dll"]
```


# 2️⃣ Eureka Server (Service Discovery)

## 📂 EurekaServer Setup

We'll use a basic Steeltoe Discovery Server for .NET 8 (Spring Eureka alternative).

Steeltoe is the best fit for .NET.

### Program.cs

```csharp
using Steeltoe.Discovery.Eureka.Server;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDiscoveryClient(builder.Configuration);

var app = builder.Build();

app.UseRouting();
app.UseEndpoints(endpoints => { endpoints.MapGet("/", async ctx => await ctx.Response.WriteAsync("Eureka Server is Running")); });
app.Run();
```


### appsettings.json

```json
{
  "eureka": {
    "client": {
      "shouldRegisterWithEureka": false,
      "fetchRegistry": false
    },
    "instance": {
      "appName": "EurekaServer"
    },
    "server": {
      "enableSelfPreservation": false
    }
  }
}
```


## Dockerfile for EurekaServer

```dockerfile
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ["EurekaServer/EurekaServer.csproj", "EurekaServer/"]
RUN dotnet restore "EurekaServer/EurekaServer.csproj"
COPY . .
WORKDIR "/src/EurekaServer"
RUN dotnet publish "EurekaServer.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/publish .

EXPOSE 8761
ENTRYPOINT ["dotnet", "EurekaServer.dll"]
```


# 3️⃣ docker-compose.yml (Final Integration)

```yml
version: '3.8'

services:
  eureka-server:
    build: ./EurekaServer
    ports:
      - "8761:80"
    networks:
      - microservices-network

  ocelot-gateway:
    build: ./OcelotGateway
    ports:
      - "5000:5000"
    depends_on:
      - eureka-server
    networks:
      - microservices-network

  userservice:
    build: ./UserService
    ports:
      - "5001:80"
    networks:
      - microservices-network
    depends_on:
      - eureka-server

  productservice:
    build: ./ProductService
    ports:
      - "5002:80"
    networks:
      - microservices-network
    depends_on:
      - eureka-server

  cartservice:
    build: ./CartService
    ports:
      - "5003:80"
    networks:
      - microservices-network
    depends_on:
      - eureka-server

  orderservice:
    build: ./OrderService
    ports:
      - "5004:80"
    networks:
      - microservices-network
    depends_on:
      - eureka-server

  inventoryservice:
    build: ./InventoryService
    ports:
      - "5005:80"
    networks:
      - microservices-network
    depends_on:
      - eureka-server

  paymentservice:
    build: ./PaymentService
    ports:
      - "5006:80"
    networks:
      - microservices-network
    depends_on:
      - eureka-server

  notificationservice:
    build: ./NotificationService
    ports:
      - "5007:80"
    networks:
      - microservices-network
    depends_on:
      - eureka-server

networks:
  microservices-network:
    driver: bridge
```

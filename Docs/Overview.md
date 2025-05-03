# 🔥 High-Level Microservice Design (with Diagram)

## Microservices:

### Service	Responsibility
UserService	            Manages user authentication (mock login)
ProductService	        Manages products: search, fetch details
CartService	            Manages user carts (add, remove items)
OrderService	        Handles order placement, validation, status
InventoryService	    Checks stock for products
PaymentService	        Simulates payment confirmation
NotificationService	    Logs notifications for success/failure

### Supporting Systems:
 - API Gateway: Ocelot
 - Service Discovery: Eureka (Netflix OSS)

### Cross-cutting Concerns:

 - Logging (Serilog or NLog)

 - Exception Handling (Middleware)

 - Circuit Breaker (Polly library inside HTTP clients)

 - Rate Limiting (Ocelot middleware or in API Gateway)

 - Load Balancing (Eureka + Ocelot together)

## High-Level Diagram:

 Client (Postman)
      |
    (API Gateway - Ocelot)
      |
+-----+-----+-----+-----+-----+-----+
|User |Product|Cart|Order|Inventory|Payment|Notification|
 Service Services ...
      (All services registered in Eureka)

# 📈 API Endpoints (URL Definitions)
## User Service

### POST /api/users/login
Request: { "username": "user1", "password": "password" }
Response: { "token": "mock-jwt", "userId": "abc-123" }

## Product Service

### GET /api/products?name=Phone&category=Electronics
Response: [ { "id": "p1", "name": "Phone", "price": 1000, "inStock": true } ]

### GET /api/products/{id}
Response: { "id": "p1", "name": "Phone", "description": "...", "price": 1000, "category": "Electronics", "inStock": true }

## Cart Service

### POST /api/cart/{userId}/add
Request: { "productId": "p1", "quantity": 2 }
Response: { "status": "Item added to cart" }

### GET /api/cart/{userId}
Response: { "userId": "abc-123", "items": [{ "productId": "p1", "quantity": 2 }] }
Order Service

### POST /api/orders/{userId}/place
Request: { }
Response: { "orderId": "o1", "status": "Confirmed" | "Cancelled" }

## Inventory Service

### POST /api/inventory/check
Request: { "productId": "p1", "quantity": 2 }
Response: { "available": true | false }

## Payment Service

### POST /api/payment/confirm
Request: { "orderId": "o1", "amount": 2000 }
Response: { "paymentStatus": "Success" | "Failed" }

## Notification Service

### POST /api/notification/send
Request: { "userId": "abc-123", "message": "Your order was confirmed" }
Response: { "status": "Notification Logged" }

# ⚡ API Gateway (Ocelot) + Eureka Integration
Ocelot Configuration (ocelot.json)

``` json
{
    "Routes": [
        {
        "DownstreamPathTemplate": "/api/users/{everything}",
        "DownstreamScheme": "http",
        "ServiceName": "userservice",
        "UpstreamPathTemplate": "/users/{everything}",
        "UpstreamHttpMethod": [ "POST" ]
        },
    ],
    "GlobalConfiguration": {
        "ServiceDiscoveryProvider": {
        "Host": "localhost",
        "Port": 8761,
        "Type": "Eureka"
        }
    }
}
```

Each service will register itself to Eureka.

Ocelot will use Eureka to discover services dynamically.

# 📦 Source Code Structure

Each Microservice Will Have:

 - Clean Architecture

Layered:

 - API Layer

 - Application/Business Layer

 - Infrastructure Layer

 - DTOs for Request/Response Models

 - Validation using FluentValidation

 - Exception Middleware

 - Healthchecks endpoint /health

# 🔄 Inter-communication Approach

Between Services	    Approach
Synchronous calls	    REST HTTP (using HttpClientFactory + Polly Resilience)
Asynchronous	        (Not asked, but good practice): Notifications could be async via message broker (e.g., RabbitMQ) - for now just simulate with logs
Service Discovery	    Eureka Client libraries

# 🐳 Docker Setup
Each microservice will have a Dockerfile (multi-stage if needed).

## Build and Push images to Docker Hub:

```bash
docker build -t username/productservice:latest .
docker push username/productservice:latest
```

## Example Dockerfile (Product Service):

```dockerfile
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["ProductService/ProductService.csproj", "ProductService/"]
RUN dotnet restore "ProductService/ProductService.csproj"
COPY . .
WORKDIR "/src/ProductService"
RUN dotnet build "ProductService.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ProductService.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ProductService.dll"]
```

## Docker Compose:

```yml
version: '3.8'
services:
eureka:
    image: netflixoss/eureka
    ports:
    - "8761:8761"

apigateway:
    image: username/apigateway:latest
    ports:
    - "8000:80"
    depends_on:
    - eureka

userservice:
    image: username/userservice:latest
    ports:
    - "5001:80"
    depends_on:
    - eureka
```

# 🎬 Recording
You need to record:

 - Login

 - Search Product

 - Add to Cart

 - Place Order (Success)

 - Place Order (Failure - out of stock or payment failure)

 - Record using Postman runner + logs console.


# 🔥 Postman Collection
Export all APIs in Postman as a collection.

Properly organize folders:

 - Login APIs

 - Product APIs

 - Cart APIs

 - Order APIs

 - Notification APIs


# Important Best Practices to Implement:
 - ✅ Exception Handling Middleware
 - ✅ Serilog/NLog for structured logging
 - ✅ FluentValidation for inputs
 - ✅ HTTP Client Factory + Polly Retry + Circuit Breaker
 - ✅ Health Checks (/health) on all services
 - ✅ Proper Status Codes and Error Messages (400, 404, 500, etc.)
 - ✅ Pagination on Product Search
 - ✅ Rate Limiting on Product Service using Ocelot


# 📢 Final Words (Direct and Brutal):
 - Avoid Fat Controllers. Move logic to services.

 - Separate DTOs and Domain Models.

 - Every service should be small, single-responsibility, and focused.

 - Never leak internal service model to outside APIs.

 - Think for scaling horizontally (add replicas in Compose).

 - Make proper use of Retry Policies and Circuit Breaker. No excuses.




# 1️⃣ POSTMAN COLLECTION STRUCTURE

## Collection: eCommercePortal-Microservices

| Folder  | API Name            | Method | URL                                                         |
|---------|---------------------|--------|-------------------------------------------------------------|
| User    | Login               | POST   | http://localhost:5000/user/login                            |
| Product | Search Products     | GET    | http://localhost:5000/products/search?name=Shoes            |
| Product | Get Product Details | GET    | http://localhost:5000/products/{productId}                  |
| Cart    | Add to Cart         | POST   | http://localhost:5000/cart/add                              |
| Cart    | View Cart           | GET    | http://localhost:5000/cart/{userId}                         |
| Order   | Place Order         | POST   | http://localhost:5000/orders/place                          |
| Order   | View Order          | GET    | http://localhost:5000/orders/{orderId}                      |


# 2️⃣ SAMPLE REQUEST/RESPONSES

## 🔹 User Login

### POST /user/login

Request:

```json
{
  "username": "john",
  "password": "password123"
}
```

Response (200 OK):

```json
{
  "userId": "user-001",
  "message": "Login successful"
}
```

## 🔹 Product Search

### GET /products/search?name=Shoes

Response:

```json
[
  {
    "productId": "prod-001",
    "name": "Nike Running Shoes",
    "price": 4999,
    "category": "Footwear",
    "availableQuantity": 20
  }
]
```


## 🔹 Add to Cart

### POST /cart/add

Request:

```json
{
  "userId": "user-001",
  "productId": "prod-001",
  "quantity": 2
}
```
Response:

```json
{
  "message": "Product added to cart"
}
```

## 🔹 Place Order

### POST /orders/place

Request:

```json
{
  "userId": "user-001"
}
```

Response (Success):

```json
{
  "orderId": "order-789",
  "status": "Confirmed",
  "message": "Order placed successfully"
}
```

Response (Failure):

```json
{
  "orderId": null,
  "status": "Cancelled",
  "message": "Order failed due to Inventory unavailability"
}
```








# Architecture

Reasoned about microservices and communication for a couple of seconds
Let's break this down into two parts: first, the layered architecture and then the intercommunication mechanisms.

## 1. Layered Architecture

```md
In a well-architected microservices solution, the separation of concerns is achieved by dividing the application into distinct layers that each serve a specific purpose. Here’s how it works:

##  Presentation (API) Layer

Role:
Handles HTTP requests and responses. This is where controllers reside.

Responsibilities:

Exposing RESTful endpoints.

Validating and mapping incoming data (using techniques like model binding and data annotations).

Forwarding calls to the Application layer.

Example:
In our UserService, the UserController handles login requests.

## Application (Business) Layer

Role:
Contains the core business logic and orchestrates the interactions between the presentation and the infrastructure/domain layers.

Responsibilities:

Implementing business rules.

Coordinating tasks between various parts of the application.

Transforming data between DTOs (Data Transfer Objects) and domain entities.

Example:
In our OrderService, the service takes a create order request (from the API), calculates totals, and then coordinates with inventory and payment systems.

## Infrastructure Layer

Role:
Deals with external systems and technical concerns that are not part of the business domain.

Responsibilities:

Data persistence (repositories, ORM mappings, database connections).

External service integrations (HTTP clients, file storage, messaging).

Logging, caching, and exception handling.

Example:
We use in-memory mock data in our services here, but in a production system, this layer would interact with databases or external APIs. It may also include resilience mechanisms (e.g., Polly policies integrated in HttpClient calls).

## Domain Layer

Role:
Represents the core business objects and rules.

Responsibilities:

Encapsulating the business models (entities, value objects, aggregates).

Business logic that doesn't rely on any external infrastructure.

Example:
In our OrderService, domain models such as Order and OrderItem encapsulate the essential data and business logic related to order processing.

## Overall Flow
Presentation Layer: A client (or API Gateway) makes a call to a RESTful endpoint.

Application Layer: The controller translates the HTTP request into a business request (DTO), passes it to the application service, which contains the business logic.

Domain Layer: The application service creates or manipulates domain entities based on the business logic.

Infrastructure Layer: When external communication is required (e.g., saving data, calling an external inventory service), the application layer leverages infrastructure components.

This clean separation enhances:

Maintainability: Changes in business logic are isolated from presentation concerns.

Testability: Each layer can be tested independently using unit tests or integration tests.

Scalability: As the business grows, layers can be scaled or replaced independently.
```

## 2. Inter-communication with Resilience, Rate Limiting, and Load Balancing

In a microservices ecosystem, inter-service communication is crucial. For synchronous HTTP calls between services (such as between OrderService and PaymentService), we need to ensure resilience, rate control, and optimal routing.

# Circuit Breaker (Using Polly in HttpClient)

```md
## What It Does:
The Circuit Breaker pattern helps prevent cascading failures by breaking the connection to an unstable service after a threshold of failures is met. It “opens the circuit” and then attempts occasional calls (“half-open”) to see if the service is recovered.

## Implementation:

Use HttpClientFactory in .NET to configure named or typed HTTP clients.

Integrate Polly policies with retries, circuit breakers, and timeout strategies.

## Example Code:


services.AddHttpClient<IPaymentServiceClient, PaymentServiceClient>()
        .AddTransientHttpErrorPolicy(p => 
            p.WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromSeconds(2)))
        .AddTransientHttpErrorPolicy(p => 
            p.CircuitBreakerAsync(handledEventsAllowedBeforeBreaking: 5,
            durationOfBreak: TimeSpan.FromSeconds(30)));
In this snippet, for any HTTP call:

The client will retry 3 times with a 2-second delay.

If more than 5 errors occur consecutively, the circuit breaks for 30 seconds.
```

# Rate Limiting (Ocelot Middleware in API Gateway)

```md
## What It Does:
Rate limiting helps control the amount of inbound traffic to protect your services from overload (accidental or malicious).

## Implementation:

Ocelot Gateway supports rate limiting through its configuration file (ocelot.json).

You can specify rate limit options such as ClientWhitelist, EnableRateLimiting, Period, PeriodTimespan, and Limit.

## Example in ocelot.json:


{
    "Routes": [
        {
            "DownstreamPathTemplate": "/api/products/{everything}",
            "DownstreamScheme": "http",
            "DownstreamHostAndPorts": [
                { "Host": "productservice", "Port": 80 }
            ],
            "UpstreamPathTemplate": "/products/{everything}",
            "UpstreamHttpMethod": [ "GET" ],
            "RateLimitOptions": {
                "ClientWhitelist": [],
                "EnableRateLimiting": true,
                "Period": "1s",
                "PeriodTimespan": 1,
                "Limit": 10
            }
        }
    ],
    "GlobalConfiguration": {
        "BaseUrl": "http://localhost:5000"
    }
}

Here, up to 10 requests per second can be allowed on the specified route.
```

# Load Balancing (Eureka + Ocelot)

```md
## What It Does:
Load balancing ensures that the traffic is evenly distributed across multiple instances of a service, improving performance and reliability.

## Implementation:

### Eureka:
Each service registers itself with Eureka (service discovery). Eureka keeps track of all available instances.

### Ocelot:
The gateway consults Eureka to dynamically resolve the available instances of a service. When a request comes in, Ocelot can route it to one of the healthy instances.

## Configuration Example:


{
  "Routes": [
    {
      "DownstreamPathTemplate": "/api/orders/{everything}",
      "DownstreamScheme": "http",
      "ServiceName": "orderservice",
      "UpstreamPathTemplate": "/orders/{everything}",
      "UpstreamHttpMethod": [ "GET", "POST" ]
    }
  ],
  "GlobalConfiguration": {
    "ServiceDiscoveryProvider": {
      "Host": "localhost",
      "Port": 8761,
      "Type": "Eureka"
    },
    "BaseUrl": "http://localhost:5000"
  }
}


In this configuration, instead of specifying fixed host and port numbers, the gateway uses the ServiceName (for example, "orderservice"), and Eureka tells it which instances are available. Ocelot then uses this information to distribute the load across these instances.
```


## Summary

```md
## Layered Architecture:

Presentation: Handles HTTP requests and responses (controllers).

Application: Contains business logic, coordinates domain and infrastructure operations.

Infrastructure: Manages external concerns (database, HTTP clients, logging, etc.).

Domain: Contains core business models and rules.

## Inter-communication Techniques:

Circuit Breaker with Polly: Ensures resilient HTTP communication by handling transient faults and preventing cascading failures.

Rate Limiting in Ocelot: Protects downstream services from overload by restricting the number of calls per unit time.

Load Balancing using Eureka and Ocelot: Enables dynamic routing to multiple healthy instances of a service based on service discovery.

This architecture ensures a robust, scalable, and maintainable system that is both modular and resilient. Each component is decoupled, allowing you to evolve and scale your backend services with minimal disruptions.
```







# ✅ Correct Dependency Direction

## 1. Presentation Layer
Depends on:

Application Layer

Does NOT depend on:

Domain Layer

Infrastructure Layer (directly)

🧠 The API/Controller should only call Application Services. It should never directly manipulate Domain Models or Infrastructure.

## 2. Application Layer
Depends on:

Domain Layer

Infrastructure Layer (interfaces only, never concrete classes)

Does NOT depend on:

Presentation Layer

🧠 Application Services orchestrate business rules (Domain) and call external systems (via Interfaces, not concrete Infrastructure code).

## 3. Domain Layer
Depends on:

Nothing

Absolutely NO dependency on:

Infrastructure

Application

Presentation

🧠 Domain is the heart of the system. It should be the most stable, independent, pure C# code — no frameworks, no HTTP clients, no database concerns.

## 4. Infrastructure Layer
Depends on:

Domain Layer

Implements interfaces defined by:

Application Layer (for persistence, external services)

🧠 Infrastructure is a replaceable detail. It implements repositories, external API clients, etc.

### 📊 Quick Visual

```md
Presentation Layer
      ↓
Application Layer
      ↓
Domain Layer
 ↑  (Interfaces)
Infrastructure Layer
```

Notice: Infrastructure depends on Domain, but Domain knows nothing about Infrastructure.
Also, Application depends on Domain but Infrastructure implements things needed by Application.

## 🎯 Real Example in your project

| Layer         | Example                                                                                   |
|---------------|-------------------------------------------------------------------------------------------|
| Presentation  | OrderController calls `IOrderService.PlaceOrderAsync()`                                   |
| Application   | OrderService orchestrates `InventoryClient`, `PaymentClient`, `OrderDomainModel`          |
| Domain        | `Order`, `OrderItem` classes define business logic like "calculate total"                 |
| Infrastructure| `InventoryHttpClient`, `PaymentHttpClient`, logging, mock database                        |



## 🏛️ Clean Folder and Project Structure

```md
/OrderService
  /OrderService.API            --> Presentation Layer (Controllers, Startup)
  /OrderService.Application    --> Application Layer (UseCases, DTOs, Services Interfaces)
  /OrderService.Domain         --> Domain Layer (Entities, ValueObjects, Enums, Domain Events)
  /OrderService.Infrastructure --> Infrastructure Layer (Persistence, HTTP Clients, Repositories)
  /OrderService.Shared          --> Shared Kernel (Common things like BaseEntity, Result classes)
  /OrderService.Tests           --> Unit and Integration Tests
```

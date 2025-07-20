# Microservices Architecture for E-commerce Flow

## 1. Identified Microservices

For the given e-commerce user flow, the following microservices are identified based on core domain responsibilities, separation of concerns, and future scalability. Each microservice encapsulates a bounded context with its own data store and business logic.

### 1.1 Auth Service

- **Responsibilities**:

  - Validates user login credentials.
  - Mocks token/session generation.
  - Provides user identity to other services.

### 1.2 Product Service

- **Responsibilities**:

  - Maintains the catalog of products.
  - Allows querying products by name or category.
  - Provides detailed product information.

### 1.3 Cart Service

- **Responsibilities**:

  - Manages user shopping cart.
  - Adds/removes items and retrieves cart content.

### 1.4 Order Service

- **Responsibilities**:

  - Initiates order placement.
  - Validates cart contents.
  - Orchestrates inventory check and mock payment.
  - Manages order status transitions.

### 1.5 Inventory Service

- **Responsibilities**:

  - Maintains inventory stock levels.
  - Supports reserving and releasing stock.

### 1.6 Notification Service

- **Responsibilities**:

  - Sends/logs notifications based on order events.
  - Supports logging for audit purposes.

### API Gateway

- **Responsibilities**:

  - Acts as the single entry point for external clients.
  - Routes requests to respective services.
  - Handles cross-cutting concerns (rate limiting, caching, load balancing).

---

## 2. API Endpoint Definitions

### Auth Service

```
POST /auth/login
{
  "username": "john_doe",
  "password": "mock123"
}
Response:
{
  "token": "mock-jwt-token"
}
```

### Product Service

```
GET /products?name=shoes&category=sports
Response: [ { product } ]

GET /products/{productId}
Response: { product }
```

### Cart Service

- User id will be retrieved from access token provided in request header

```
POST /cart/add
{
  "productId": "prod-001",
  "quantity": 2
}
Response: { "message": "Product added to cart" }

GET /cart/
Response: { "userId": "user-123", "items": [ ... ] }
```

### Order Service

```
POST /orders
{
  "cartId": "cart-123"
}
Response (success): { "orderId": "order-987", "status": "confirmed" }
Response (failure): { "status": "cancelled" }
```

### Inventory Service

```
POST /inventory/check
{
  "productId": "prod-001", "quantity": 2
}
```

### Notification Service (Internal)

I am using message broker for notification

```
publish events
{
  "userId": "user-123",
  "type": "order_success",
  "message": "Order confirmed."
}
```

---

## 3. Inter-Service Communication Approach

### Style

- **RESTful HTTP**: All services communicate via RESTful HTTP APIs.
- **Synchronous communication**: Order Service calls Inventory and Notification services directly using http client.

### Flow Example (Placing Order):

1. User adds product to cart via Cart Service.
2. User places order via Order Service:

   - Order Service calls Cart Service to get cart items.
   - Calls Inventory Service to check items availability.
   - Mocks payment logic.
   - If successful: creates order and calls Notification Service.
   - If failed: calls Notification Service with cancellation.

### Assumptions:

- **Authentication** JWT based access
- **Inventory** system holds product ID and stock only (no full product info).
- **Notification** system is a logger (no real email/SMS integration).
- **No orchestration tool** like Saga or choreography; Order Service drives the transaction.
- Each microservice has its **own database** (In-memory).

---

## 4. High-Level Architecture Diagram (Textual Representation)

```
                       +-------------------+
                       |   API Gateway     |
                       +--------+----------+
                                |
       +------------------------+------------------------+
       |                        |                        |
+---------------+    +------------------+     +-------------------+
|  Auth Service |    | Product Service  |     |  Cart Service     |
+---------------+    +------------------+     +-------------------+
                                                   |
                                                   v
                                          +-------------------+
                                          | Order Service     |
                                          +-------------------+
                                           /         \
                                          v           v
                             +----------------+  +--------------------+
                             | Inventory Svc  |  | Notification Svc   |
                             +----------------+  +--------------------+
```

---

## Docker images used

- [GatewayService](https://hub.docker.com/r/rahuldey8320/shopsphare-gateway-service)
- [UserService](https://hub.docker.com/r/rahuldey8320/shopsphare-user-service)
- [ProductService](https://hub.docker.com/r/rahuldey8320/shopsphare-product-service)
- [CartService](https://hub.docker.com/r/rahuldey8320/shopsphare-cart-service)
- [OrderService](https://hub.docker.com/r/rahuldey8320/shopsphare-order-service)
- [InventoryService](https://hub.docker.com/r/rahuldey8320/shopsphare-inventory-service)
- [PaymentService](https://hub.docker.com/r/rahuldey8320/shopsphare-payment-service)
- [NotificationService](https://hub.docker.com/r/rahuldey8320/shopsphare-notification-service)

---

## Boot Application

```bash
docker compose -f prod.docker-compose.yml up
```

## Stop Application

```bash
docker compose -f prod.docker-compose.yml down
```

## SEQ UI (Logging & Distributed Tracing)

Open [http://localhost:8081](http://localhost:8081)

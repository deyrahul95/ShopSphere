using System.Net;
using Microsoft.Extensions.Logging;
using OrderService.Application.DTOs;
using OrderService.Application.Extensions;
using OrderService.Application.Models;
using OrderService.Application.Results;
using OrderService.Application.Services.Interfaces;
using OrderService.Domain.Entities;
using OrderService.Domain.Enums;
using OrderService.Domain.Exceptions;
using OrderService.Domain.Repositories;

namespace OrderService.Application.Services;

public class OrdersService(
    IOrderRepository orderRepository,
    ICartHttpClient cartHttpClient,
    IInventoryHttpClient inventoryHttpClient,
    IPaymentHttpClient paymentHttpClient,
    ILogger<OrdersService> logger) : IOrdersService
{
    public async Task<ServiceResult<OrderDto>> CreateOrder(
        Guid userId,
        CreateOrderRequest request,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var (isSuccess, result) = await FetchCart(cancellationToken);

            if (isSuccess is false)
            {
                return OrderResults<OrderDto>.HttpRequestFailed(result);
            }

            var cart = result?.Data;

            if (cart is null || cart.Id.Equals(request.CartId) is false)
            {
                logger.LogWarning(
                    "Cart not found. Cart Id: {CartId}, User Id: {UserId}",
                    request.CartId,
                    userId);
                return OrderResults<OrderDto>.CartNotFound(request.CartId);
            }

            if (cart.Items.Count == 0)
            {
                logger.LogWarning(
                    "Cart has no items to order. Cart Id: {CartId}, User Id: {UserId}",
                    request.CartId,
                    userId);
                return OrderResults<OrderDto>.NoItemsFound(request.CartId);
            }

            var order = await CreateNewOrder(
                userId: userId,
                cartDto: cart,
                paymentMode: request.PaymentMode,
                cancellationToken: cancellationToken);

            logger.LogInformation(
                "Order created successfully. Order: {@Order}",
                order);

            await ClearCart(cancellationToken);

            var isStockAvailable = await CheckInventory(
                orderItems: order.Items,
                cancellationToken: cancellationToken);

            if (isStockAvailable is false)
            {
                order.UpdateOrderState(OrderStatus.Cancelled);
                await orderRepository.UpdateOrder(order: order, cancellationToken: cancellationToken);

                return OrderResults<OrderDto>.StockUnavailable(order.ToDto());
            }

            order.UpdateOrderState(OrderStatus.Confirmed);

            var paymentStatus = await MakePayment(
                orderId: order.Id,
                amount: order.TotalAmount,
                paymentMode: request.PaymentMode,
                cancellationToken: cancellationToken);

            order.UpdatePaymentState(paymentStatus);
            await orderRepository.UpdateOrder(order: order, cancellationToken: cancellationToken);

            return OrderResults<OrderDto>.OrderCreated(order.ToDto());
        }
        catch (ValidationException ex)
        {
            logger.LogError(
                ex,
                "Domain validation failed. User Id: {UserId}, Error: {Error}",
                userId,
                ex.Message);

            return OrderResults<OrderDto>.ValidationFailed([ex.ToError()]);
        }
        catch (Exception ex)
        {
            logger.LogError(
                ex,
                "Failed to create order. User Id: {UserId}, Error: {Error}",
                userId,
                ex.Message);

            return OrderResults<OrderDto>.InternalServerError;
        }
    }

    public async Task<ServiceResult<OrderDto>> GetOrder(
        Guid orderId,
        Guid userId,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var order = await FetchOrderById(
                orderId: orderId,
                userId: userId,
                cancellationToken: cancellationToken);

            if (order is null)
            {
                logger.LogWarning(
                    "Order not found. Order Id: {OrderId}, User Id: {UserId}",
                    orderId,
                    userId);
                return OrderResults<OrderDto>.OrderNotFound(orderId);
            }

            logger.LogInformation(
                "Order fetched successfully. Order: {@Order}",
                order);

            return OrderResults<OrderDto>.OrderFetched(order.ToDto());
        }
        catch (Exception ex)
        {
            logger.LogError(
                ex,
                "Failed to fetch order. Order Id: {OrderId}, User Id: {UserId}, Error: {Error}",
                orderId,
                userId,
                ex.Message);

            return OrderResults<OrderDto>.InternalServerError;
        }
    }

    public async Task<ServiceResult<OrderStatusResponse>> GetOrderStatus(
        Guid orderId,
        Guid userId,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var order = await FetchOrderById(
                orderId: orderId,
                userId: userId,
                cancellationToken: cancellationToken);

            if (order is null)
            {
                logger.LogWarning(
                    "Order not found. Order Id: {OrderId}, User Id: {UserId}",
                    orderId,
                    userId);
                return OrderResults<OrderStatusResponse>.OrderNotFound(orderId);
            }

            logger.LogInformation(
                "Order fetched successfully. Order: {@Order}",
                order);

            var orderStatusResponse = new OrderStatusResponse(
                OrderStatus: order.OrderState,
                OrderPaymentStatus: order.PaymentState);

            return OrderResults<OrderStatusResponse>.OrderFetched(orderStatusResponse);
        }
        catch (Exception ex)
        {
            logger.LogError(
                ex,
                "Failed to fetch order. Order Id: {OrderId}, User Id: {UserId}, Error: {Error}",
                orderId,
                userId,
                ex.Message);

            return OrderResults<OrderStatusResponse>.InternalServerError;
        }
    }

    public async Task<ServiceResult> UpdateOrderStatus(
        Guid userId,
        UpdateStatusRequest request,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var order = await FetchOrderById(
               orderId: request.OrderId,
               userId: userId,
               cancellationToken: cancellationToken);

            if (order is null)
            {
                logger.LogWarning(
                    "Order not found. Order Id: {OrderId}, User Id: {UserId}",
                    request.OrderId,
                    userId);
                return OrderResults<OrderDto>.OrderNotFound(request.OrderId);
            }

            order.UpdateOrderState(request.OrderState);
            order.UpdatePaymentState(request.PaymentState);

            var isSuccess = await orderRepository.UpdateOrder(order: order, cancellationToken: cancellationToken);

            if (isSuccess is false)
            {
                logger.LogWarning(
                    "Order updated failed in repository. Order: {@Order}",
                    order);
                return OrderResults.InternalServerError;
            }

            logger.LogInformation(
                "Order updated successfully. Order: {@Order}",
                order);
            return OrderResults.NoContent;
        }
        catch (Exception ex)
        {
            logger.LogError(
                ex,
                "Failed to update order.Order Id: {OrderId}, User Id: {UserId}, Error: {Error}",
                request.OrderId,
                userId,
                ex.Message);

            return OrderResults<OrderDto>.InternalServerError;
        }
    }

    private async Task<Order?> FetchOrderById(Guid orderId, Guid userId, CancellationToken cancellationToken)
    {
        logger.LogInformation("Fetching order data.Order Id: {OrderId}, User Id: {UserId}", orderId, userId);
        var order = await orderRepository.GetOrder(
                userId: userId,
                orderId: orderId,
                cancellationToken: cancellationToken);

        if (order is null)
        {
            logger.LogWarning(
                "order not found. Order Id: {OrderId}, UserId: {UserId}",
                orderId,
                userId);
            return null;
        }

        logger.LogInformation(
            "order data fetched successfully. User Id: {UserId}, order Id: {OrderId}",
            userId,
            order.Id);
        return order;
    }

    private async Task<(bool, ServiceResult<CartDto>?)> FetchCart(CancellationToken cancellationToken)
    {
        logger.LogInformation("Fetching cart details.");
        var result = await cartHttpClient.GetCart(cancellationToken);

        if (result == null || result.StatusCode != HttpStatusCode.OK)
        {
            logger.LogWarning(
                "Failed to fetched cart details. Result: {@Result}",
                result);
            return (false, result);
        }

        logger.LogInformation(
            "Cart details fetched successfully. Result: {@Result}",
            result);
        return (true, result);
    }

    private async Task<Order> CreateNewOrder(
        Guid userId,
        CartDto cartDto,
        PaymentMode paymentMode,
        CancellationToken cancellationToken)
    {
        var orderItems = new List<OrderItem>();

        foreach (var cartItem in cartDto.Items)
        {
            var orderItem = OrderItem.Create(
                productId: cartItem.ProductId,
                quantity: cartItem.Quantity,
                price: cartItem.Price);

            orderItems.Add(orderItem);
        }

        var order = Order.Create(
            userId: userId,
            items: orderItems,
            paymentMode: paymentMode);

        return await orderRepository.CreateOrder(order: order, cancellationToken: cancellationToken);
    }

    private async Task<bool> ClearCart(CancellationToken cancellationToken)
    {
        logger.LogInformation("Start clearing cart details.");
        var statusCode = await cartHttpClient.ClearCart(cancellationToken);

        if (statusCode != HttpStatusCode.NoContent)
        {
            logger.LogWarning("Failed to cleared cart details");
            return false;
        }

        logger.LogInformation("Cart details cleared successfully");
        return true;
    }

    private async Task<bool> CheckInventory(List<OrderItem> orderItems, CancellationToken cancellationToken)
    {
        foreach (var item in orderItems)
        {
            var checkInventoryRequest = new CheckInventoryRequest(
                ProductId: item.ProductId,
                Quantity: item.Quantity);

            logger.LogInformation("Start fetching inventory details.Product Id: {ProductId}", item.ProductId);

            var result = await inventoryHttpClient.CheckInventory(
                request: checkInventoryRequest,
                cancellationToken: cancellationToken);

            if (result == null || result.StatusCode != HttpStatusCode.OK)
            {
                logger.LogWarning(
                    "Failed to fetched inventory details. Result: {@Result}",
                    result);
                return false;
            }

            logger.LogInformation("Inventory details fetched successfully. Product Id: {ProductId}", item.ProductId);

            if (result.Data?.Available is false)
            {
                logger.LogWarning("Product is out of stock.Product Id: {ProductId}", item.ProductId);
                return false;
            }
        }

        return true;
    }

    private async Task<OrderPaymentStatus> MakePayment(
        Guid orderId,
        decimal amount,
        PaymentMode paymentMode,
        CancellationToken cancellationToken)
    {
        logger.LogInformation("Start make payment.");
        var request = new ProcessPaymentRequest
        {
            OrderId = orderId,
            Amount = amount,
            PaymentMode = paymentMode
        };

        var result = await paymentHttpClient.ProcessPayment(
            request: request,
            cancellationToken: cancellationToken);

        if (result == null || result.StatusCode != HttpStatusCode.OK)
        {
            logger.LogWarning(
                "Failed to make payment. Result: {@Result}",
                result);
            return OrderPaymentStatus.Failed;
        }

        logger.LogInformation(
            "Payment completed. Result: {@Result}",
            result);

        var paymentDto = result.Data;

        if (paymentDto is null)
        {
            return OrderPaymentStatus.Failed;
        }

        if (paymentDto.PaymentStatus.Equals("Completed", StringComparison.OrdinalIgnoreCase))
        {
            return OrderPaymentStatus.Paid;
        }

        return OrderPaymentStatus.Failed;
    }
}

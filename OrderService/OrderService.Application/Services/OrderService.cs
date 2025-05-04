using System.Net;
using Microsoft.Extensions.Logging;
using OrderService.Application.DTOs;
using OrderService.Application.Extensions;
using OrderService.Application.Models;
using OrderService.Application.Results;
using OrderService.Application.Services.Interfaces;
using OrderService.Domain.Entities;
using OrderService.Domain.Exceptions;
using OrderService.Domain.Repositories;

namespace OrderService.Application.Services;

public class OrdersService(
    IOrderRepository orderRepository,
    ICartHttpClient cartHttpClient,
    ILogger<OrdersService> logger) : IOrdersService
{
    public async Task<ServiceResult<OrderDto>> CreateOrder(
        Guid userId,
        CreateOrderRequest request,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var (isSuccess, result) = await FetchCart();

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

            var newOrder = await CreateNewOrder(
                userId: userId,
                cartDto: cart,
                cancellationToken: cancellationToken);

            return OrderResults<OrderDto>.OrderCreated(newOrder.ToDto());
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
            
            await orderRepository.UpdateOrder(order: order, cancellationToken: cancellationToken);

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

    private async Task<(bool isSuccess, ServiceResult<CartDto>? result)> FetchCart()
    {
        logger.LogInformation("Fetching cart details.");
        var result = await cartHttpClient.GetCart();

        if (result == null || result.StatusCode != HttpStatusCode.OK)
        {
            logger.LogWarning(
                "Failed to fetched cart details.Result: {@Result}",
                result);
            return (false, result);
        }

        logger.LogInformation(
            "Cart details fetched successfully.Status Code: {StatusCode}",
            result.StatusCode);
        return (true, result);
    }

    private async Task<Order> CreateNewOrder(Guid userId, CartDto cartDto, CancellationToken cancellationToken)
    {
        var orderItems = new List<OrderItem>();

        foreach (var cartItem in cartDto.Items)
        {
            var orderItem = OrderItem.Create(
                productId: cartItem.ProductId,
                quantity: cartItem.Quantity,
                price: cartItem.Price);
        }

        var order = Order.Create(userId: userId, items: orderItems);

        return await orderRepository.CreateOrder(order: order, cancellationToken: cancellationToken);
    }
}

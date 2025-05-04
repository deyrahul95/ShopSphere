using System.Net;
using Microsoft.Extensions.Logging;
using OrderService.Application.DTOs;
using OrderService.Application.Extensions;
using OrderService.Application.Models;
using OrderService.Application.Results;
using OrderService.Application.Services.Interfaces;
using OrderService.Domain.Entities;
using OrderService.Domain.Repositories;

namespace OrderService.Application.Services;

public class OrderService(
    IOrderRepository orderRepository,
    ICartHttpClient cartHttpClient,
    ILogger<OrderService> logger) : IOrderService
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

    public Task<ServiceResult<OrderDto>> GetOrder(
        Guid orderId,
        Guid userId,
        CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<ServiceResult> UpdateOrderStatus(
        Guid userId,
        UpdateStatusRequest request,
        CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
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

using OrderService.Application.DTOs;
using OrderService.Application.Models;
using OrderService.Application.Results;

namespace OrderService.Application.Services.Interfaces;

public interface IOrdersService
{
    Task<ServiceResult<OrderDto>> CreateOrder(
        Guid userId,
        CreateOrderRequest request,
        CancellationToken cancellationToken = default);

    Task<ServiceResult<OrderDto>> GetOrder(
        Guid orderId,
        Guid userId,
        CancellationToken cancellationToken = default);

    Task<ServiceResult<OrderStatusResponse>> GetOrderStatus(
        Guid orderId,
        Guid userId,
        CancellationToken cancellationToken = default);

    Task<ServiceResult> UpdateOrderStatus(
        Guid userId,
        UpdateStatusRequest request,
        CancellationToken cancellationToken = default);
}

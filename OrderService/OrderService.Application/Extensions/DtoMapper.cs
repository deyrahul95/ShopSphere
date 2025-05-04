using OrderService.Application.DTOs;
using OrderService.Domain.Entities;

namespace OrderService.Application.Extensions;

public static class DtoMapper
{
    public static OrderItemDto ToDto(this OrderItem orderItem)
    {
        return new OrderItemDto(
            Id: orderItem.Id,
            ProductId: orderItem.ProductId,
            Quantity: orderItem.Quantity,
            Price: orderItem.Price
        );
    }

    public static OrderDto ToDto(this Order order)
    {
        return new OrderDto(
            Id: order.Id,
            UserId: order.UserId,
            Items: order.Items.Select(ToDto).ToList(),
            TotalPrice: order.TotalAmount,
            OrderState: order.OrderState,
            PaymentState: order.PaymentState
        );
    }
}

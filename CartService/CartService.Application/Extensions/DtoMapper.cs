using CartService.Application.DTOs;
using CartService.Domain.Entities;

namespace CartService.Application.Extensions;

public static class DtoMapper
{
    public static CartItemDto ToDto(this CartItem item)
    {
        return new CartItemDto(
            Id: item.Id,
            ProductId: item.ProductId,
            ProductName: item.ProductName,
            Quantity: item.Quantity,
            Price: item.Price,
            TotalPrice: item.GetTotalPrice()
        );
    }

    public static CartDto ToDto(this Cart cart)
    {
        return new CartDto(
            Id: cart.Id,
            UserId: cart.UserId,
            Items: cart.Items.Select(ToDto).ToList(),
            TotalPrice: cart.GetTotalPrice()
        );
    }
}

namespace CartService.Application.DTOs;

public record CartItemDto(
    Guid Id,
    Guid ProductId,
    string ProductName,
    int Quantity,
    decimal Price,
    decimal TotalPrice
);
namespace OrderService.Application.DTOs;

public record CartDto(
    Guid Id,
    Guid UserId,
    List<CartItemDto> Items,
    decimal TotalPrice
);

namespace PaymentService.Application.DTOs;

public record OrderDto(
    Guid Id,
    Guid UserId,
    List<OrderItemDto> Items,
    decimal TotalPrice,
    string OrderState,
    string PaymentState
);
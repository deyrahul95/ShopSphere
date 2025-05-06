namespace OrderService.Application.DTOs;

public record PaymentDto(
    Guid Id,
    Guid OrderId,
    Guid UserId,
    decimal AmountPaid,
    string PaymentMode,
    string PaymentStatus
);

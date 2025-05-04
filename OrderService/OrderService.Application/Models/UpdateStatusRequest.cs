using OrderService.Domain.Enums;

namespace OrderService.Application.Models;

public record UpdateStatusRequest(
    Guid OrderId,
    OrderStatus OrderState,
    OrderPaymentStatus PaymentState
);

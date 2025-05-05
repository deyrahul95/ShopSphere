namespace OrderService.Application.Models;

public record OrderStatusResponse(
    string OrderStatus,
    string OrderPaymentStatus
);

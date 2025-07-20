namespace OrderService.Application.Models;

public record OrderStatusResponse(
    string JobStatus,
    string OrderStatus,
    string OrderPaymentStatus
);

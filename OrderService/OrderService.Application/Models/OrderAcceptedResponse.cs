namespace OrderService.Application.Models;

public record OrderAcceptedResponse(Guid OrderId, string Status);
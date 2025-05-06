using PaymentService.Application.DTOs;
using PaymentService.Application.Results;

namespace PaymentService.Application.Services.Interfaces;

public interface IOrderHttpClient
{
    Task<ServiceResult<OrderDto>?> GetOrder(Guid orderId, CancellationToken cancellationToken = default);
}

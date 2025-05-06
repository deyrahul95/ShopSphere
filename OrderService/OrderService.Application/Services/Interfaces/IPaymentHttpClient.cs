using OrderService.Application.DTOs;
using OrderService.Application.Models;
using OrderService.Application.Results;

namespace OrderService.Application.Services.Interfaces;

public interface IPaymentHttpClient
{
    Task<ServiceResult<PaymentDto>?> ProcessPayment(
        ProcessPaymentRequest request,
        CancellationToken cancellationToken = default);
}

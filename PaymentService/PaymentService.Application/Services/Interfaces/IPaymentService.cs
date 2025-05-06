using PaymentService.Application.DTOs;
using PaymentService.Application.Models;
using PaymentService.Application.Results;

namespace PaymentService.Application.Services.Interfaces;

public interface IPaymentService
{
    Task<ServiceResult<PaymentDto>> ProcessPayment(
        Guid userId,
        ProcessPaymentRequest request,
        CancellationToken cancellationToken = default);

    Task<ServiceResult<PaymentDto>> GetPaymentDetails(
        Guid orderId,
        CancellationToken cancellationToken = default);
}

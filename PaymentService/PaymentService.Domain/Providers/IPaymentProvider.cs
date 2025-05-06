using PaymentService.Domain.Entities;
using PaymentService.Domain.Enums;

namespace PaymentService.Domain.Providers;

public interface IPaymentProvider
{
    Task<PaymentStatus> ProcessTransaction(decimal amount, CancellationToken cancellationToken = default);
}

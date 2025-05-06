using PaymentService.Domain.Entities;

namespace PaymentService.Domain.Repositories;

public interface IPaymentRepository
{
    Task<Payment> Create(Payment payment, CancellationToken cancellationToken = default);
    Task<Payment?> GetByOrderId(Guid orderId, CancellationToken cancellationToken = default);
}

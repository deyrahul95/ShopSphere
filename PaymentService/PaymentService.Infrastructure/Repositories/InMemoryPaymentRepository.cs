using PaymentService.Domain.Entities;
using PaymentService.Domain.Repositories;
using PaymentService.Infrastructure.DB;

namespace PaymentService.Infrastructure.Repositories;

public class InMemoryPaymentRepository(InMemoryDB db) : IPaymentRepository
{
    public async Task<Payment> Create(Payment payment, CancellationToken cancellationToken = default)
    {
        db.Payments[payment.OrderId] = payment;
        return await Task.FromResult(payment);
    }

    public async Task<Payment?> GetByOrderId(Guid orderId, CancellationToken cancellationToken = default)
    {
        if (db.Payments.TryGetValue(orderId, out var payment) is false)
        {
            return await Task.FromResult<Payment?>(null);
        }

        return await Task.FromResult(payment);
    }
}

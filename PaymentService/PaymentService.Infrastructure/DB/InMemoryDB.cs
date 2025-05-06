using System.Collections.Concurrent;
using PaymentService.Domain.Entities;

namespace PaymentService.Infrastructure.DB;

public class InMemoryDB
{
    // OrderId -> Payment
    public readonly ConcurrentDictionary<Guid, Payment> Payments = new();
}

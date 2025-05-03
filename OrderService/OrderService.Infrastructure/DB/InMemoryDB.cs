using System.Collections.Concurrent;
using OrderService.Domain.Entities;

namespace OrderService.Infrastructure.DB;

public class InMemoryDB
{
    public readonly ConcurrentDictionary<Guid, List<Order>> Orders = new();
}

using System.Collections.Concurrent;
using CartService.Domain.Entities;

namespace CartService.Infrastructure.DB;

public class InMemoryDB
{
    public readonly ConcurrentDictionary<Guid, Cart> Carts = new();
}

    
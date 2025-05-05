using CartService.Domain.Entities;
using CartService.Domain.Repositories;
using CartService.Infrastructure.DB;

namespace CartService.Infrastructure.Repositories;

public class InMemoryCartRepository(InMemoryDB db) : ICartRepository
{
    public async Task<Cart> Create(Cart cart, CancellationToken cancellationToken = default)
    {
        db.Carts[cart.UserId] = cart;

        return await Task.FromResult(cart);
    }

    public async Task<Cart?> GetByUserId(Guid userId, CancellationToken cancellationToken = default)
    {
        db.Carts.TryGetValue(userId, out var cart);

        return await Task.FromResult(cart);
    }

    public Task<bool> Update(Cart cart, CancellationToken cancellationToken = default)
    {
        if (db.Carts.ContainsKey(cart.UserId) is false)
        {
            return Task.FromResult<bool>(false);
        }

        db.Carts[cart.UserId] = cart;
        return Task.FromResult(true);
    }
}

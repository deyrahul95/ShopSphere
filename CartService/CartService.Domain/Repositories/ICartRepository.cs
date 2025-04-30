using CartService.Domain.Entities;

namespace CartService.Domain.Repositories;

public interface ICartRepository
{
    Task<Cart> Create(Cart cart, CancellationToken cancellationToken = default);
    Task<Cart?> GetById(Guid cartId, CancellationToken cancellationToken = default);
    Task<Cart?> GetByUserId(Guid userId, CancellationToken cancellationToken = default);
    Task<bool> Update(Guid cartId, Cart cart, CancellationToken cancellationToken = default);
}

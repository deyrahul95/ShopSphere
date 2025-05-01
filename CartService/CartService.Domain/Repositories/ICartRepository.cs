using CartService.Domain.Entities;

namespace CartService.Domain.Repositories;

public interface ICartRepository
{
    Task<Cart> Create(Cart cart, CancellationToken cancellationToken = default);
    Task<Cart?> GetByUserId(Guid userId, CancellationToken cancellationToken = default);
    Task<bool> Update(Cart cart, CancellationToken cancellationToken = default);
}

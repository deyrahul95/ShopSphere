using InventoryService.Domain.Entities;

namespace InventoryService.Domain.Repositories;

public interface IInventoryRepository
{
    Task<ProductStock?> GetProductStock(Guid productId, CancellationToken cancellationToken = default);
}

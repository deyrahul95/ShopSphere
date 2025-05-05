using InventoryService.Domain.Entities;
using InventoryService.Domain.Repositories;
using InventoryService.Infrastructure.DB;

namespace InventoryService.Infrastructure.Repositories;

public class InMemoryInventoryRepository(InMemoryDB db) : IInventoryRepository
{
    public async Task<ProductStock?> GetProductStock(Guid productId, CancellationToken cancellationToken = default)
    {
        var productStock = db.ProductStocks
                                        .FirstOrDefault( x => x.ProductId == productId);

        return await Task.FromResult(productStock);
    }
}

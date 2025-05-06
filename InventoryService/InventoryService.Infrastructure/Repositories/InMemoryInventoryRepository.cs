using InventoryService.Domain.Entities;
using InventoryService.Domain.Repositories;
using InventoryService.Infrastructure.DB;

namespace InventoryService.Infrastructure.Repositories;

public class InMemoryInventoryRepository(InMemoryDB db) : IInventoryRepository
{
    public async Task<ProductStock?> GetProductStock(Guid productId, CancellationToken cancellationToken = default)
    {
        var productStock = db.ProductStocks
                                        .FirstOrDefault(x => x.ProductId == productId);

        return await Task.FromResult(productStock);
    }

    public async Task<bool> UpdateProductStock(ProductStock productStock, CancellationToken cancellationToken = default)
    {
        var existingProduct = db.ProductStocks
                                        .FirstOrDefault(x => x.ProductId == productStock.ProductId);

        if (existingProduct == null)
        {
            return await Task.FromResult(false);
        }

        existingProduct.AvailableQuantity = productStock.AvailableQuantity;
        return await Task.FromResult(true);
    }
}

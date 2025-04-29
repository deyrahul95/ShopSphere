using ProductService.Domain.Entities;
using ProductService.Domain.Utils;

namespace ProductService.Domain.Repositories;

public interface IProductRepository
{
    Task<PaginatedResult<Product>> SearchProductsAsync(ProductSearchParams searchParams, CancellationToken cancellationToken = default);
    Task<Product?> FindByIdAsync(Guid id, CancellationToken cancellationToken = default);
}

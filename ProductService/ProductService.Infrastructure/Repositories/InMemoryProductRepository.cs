using ProductService.Domain.Entities;
using ProductService.Domain.Repositories;
using ProductService.Domain.Utils;
using ProductService.Infrastructure.DB;

namespace ProductService.Infrastructure.Repositories;

public class InMemoryProductRepository : IProductRepository
{
    public async Task<Product?> FindByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var product = InMemoryDB.Products.FirstOrDefault(item => item.Id == id);

        return await Task.FromResult(product);
    }

    public async Task<PaginatedResult<Product>> SearchProductsAsync(
        ProductSearchFilters searchFilters,
        CancellationToken cancellationToken = default)
    {
        var query = InMemoryDB.Products
                                            .AsQueryable()
                                            .Where(item => item.InStock);

        if (string.IsNullOrEmpty(searchFilters.Name) is false)
        {
            query = query.Where(item => item.Name.Contains(
                searchFilters.Name,
                StringComparison.OrdinalIgnoreCase));
        }

        if (string.IsNullOrEmpty(searchFilters.Category) is false)
        {
            query = query.Where(item => item.Category.Contains(
                searchFilters.Category,
                StringComparison.OrdinalIgnoreCase));
        }

        if (string.IsNullOrEmpty(searchFilters.Sort) is false)
        {
            switch (searchFilters.Sort.ToLower())
            {
                case SortOptions.PriceAsc:
                    query = query.OrderBy(p => p.Price);
                    break;

                case SortOptions.PriceDesc:
                    query = query.OrderByDescending(p => p.Price);
                    break;
            }
        }

        var totalItems = query.Count();
        var items = query
            .Skip((searchFilters.PageNumber - 1) * searchFilters.PageSize)
            .Take(searchFilters.PageSize)
            .ToList();

        var paginatedResult = new PaginatedResult<Product>(
            pageNumber: searchFilters.PageNumber,
            pageSize: searchFilters.PageSize,
            totalItems: totalItems,
            items: items);

        return await Task.FromResult(paginatedResult);
    }
}

using Microsoft.Extensions.Logging;
using ProductService.Application.DTOs;
using ProductService.Application.Extensions;
using ProductService.Application.Models;
using ProductService.Application.Results;
using ProductService.Application.Services.Interfaces;
using ProductService.Domain.Constants;
using ProductService.Domain.Repositories;
using ProductService.Domain.Utils;

namespace ProductService.Application.Services;

public class ProductsService(
    IProductRepository productRepository,
    ILogger<ProductsService> logger) : IProductsService
{
    public async Task<ServiceResult<ProductDto>> GetProduct(Guid id, CancellationToken cancellationToken = default)
    {
        try
        {
            logger.LogInformation("Start fetching product data. Product Id: {ProductId}", id);
            var product = await productRepository.FindByIdAsync(id: id, cancellationToken: cancellationToken);

            if (product is null)
            {
                logger.LogWarning("No product found. Product Id: {ProductId}", id);
                return ProductResults<ProductDto>.NotFound(id);
            }

            logger.LogInformation("Product fetched successfully. Product Id: {ProductId}", id);

            return ProductResults<ProductDto>.Success(product.ToDto());
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to fetch product data. Id: {ProductID}, Error: {Error}", id, ex.Message);
            return ProductResults<ProductDto>.InternalServerError;
        }
    }

    public async Task<ServiceResult<PaginatedResult<ProductDto>>> SearchProducts(ProductSearchRequest request, CancellationToken cancellationToken = default)
    {
        try
        {
            logger.LogInformation("Start searching products. Name: {Name}, Category: {Category}", request.Name, request.Category);

            var searchFilters = new ProductSearchFilters(
                PageNumber: request.PageNumber ?? DomainConstants.DefaultPageNumber,
                PageSize: request.PageSize ?? DomainConstants.DefaultPageSize,
                Name: request.Name,
                Category: request.Category
            );

            logger.LogInformation("Product search filters. Filters: {@SearchFilters}", searchFilters);

            var paginatedResult = await productRepository.SearchProductsAsync(
                searchFilters: searchFilters,
                cancellationToken: cancellationToken);

            var result = new PaginatedResult<ProductDto>(
                pageNumber: paginatedResult.PageNumber,
                pageSize: paginatedResult.PageSize,
                totalItems: paginatedResult.TotalItems,
                items: paginatedResult.Items.ToDtoList()
            );

            logger.LogInformation("Products fetched successfully. Products Count: {Count}", result.TotalItems);

            return ProductResults<PaginatedResult<ProductDto>>.Success(result);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to search products data. Request: {@Request}, Error: {Error}", request, ex.Message);
            return ProductResults<PaginatedResult<ProductDto>>.InternalServerError;
        }
    }
}

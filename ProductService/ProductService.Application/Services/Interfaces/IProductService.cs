using ProductService.Application.DTOs;
using ProductService.Application.Models;
using ProductService.Application.Results;
using ProductService.Domain.Utils;

namespace ProductService.Application.Services.Interfaces;

public interface IProductService
{
    Task<ServiceResult<PaginatedResult<ProductDto>>> GetProductsAsync(ProductSearchRequest request, CancellationToken cancellationToken = default);
    Task<ServiceResult<ProductDto>> GetProduct(Guid id, CancellationToken cancellationToken = default);
}

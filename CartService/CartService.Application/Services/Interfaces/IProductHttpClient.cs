using CartService.Application.DTOs;
using CartService.Application.Results;

namespace CartService.Application.Services.Interfaces;

public interface IProductHttpClient
{
     Task<ServiceResult<ProductDto>?> GetProduct(Guid productId);
}

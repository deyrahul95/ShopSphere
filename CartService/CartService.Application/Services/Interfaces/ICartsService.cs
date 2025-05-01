using CartService.Application.DTOs;
using CartService.Application.Models;
using CartService.Application.Results;

namespace CartService.Application.Services.Interfaces;

public interface ICartsService
{
    Task<ServiceResult<CartDto>> AddItemToCart(
        Guid userId,
        AddToCartRequest request,
        CancellationToken cancellationToken = default);

    Task<ServiceResult<CartDto>> GetCart(Guid userId, CancellationToken cancellationToken = default);

    Task<ServiceResult> RemoveItemFromCart(
        Guid userId,
        RemovedFromCartRequest request,
        CancellationToken cancellationToken = default);

    Task<ServiceResult> ClearCart(Guid userId, CancellationToken cancellationToken = default);
}
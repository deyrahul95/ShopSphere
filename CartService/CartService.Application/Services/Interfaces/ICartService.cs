using CartService.Application.DTOs;
using CartService.Application.Models;
using CartService.Application.Results;

namespace CartService.Application.Services.Interfaces;

public interface ICartService
{
    Task<ServiceResult> AddToCart(
        Guid userId,
        AddToCartRequest request,
        CancellationToken cancellationToken = default);

    Task<ServiceResult> GetCart(Guid userId, CancellationToken cancellationToken = default);

    Task<ServiceResult> RemovedFromCart(
        Guid userId,
        RemovedFromCartRequest request,
        CancellationToken cancellationToken = default);

    Task<ServiceResult> ClearCart(Guid userId, CancellationToken cancellationToken = default);
}
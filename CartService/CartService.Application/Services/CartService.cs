using CartService.Application.DTOs;
using CartService.Application.Extensions;
using CartService.Application.Models;
using CartService.Application.Results;
using CartService.Application.Services.Interfaces;
using CartService.Domain.Exceptions;
using CartService.Domain.Repositories;
using Microsoft.Extensions.Logging;

namespace CartService.Application.Services;

public class CartService(
    ICartRepository cartRepository,
    ILogger<CartService> logger) : ICartService
{
    public async Task<ServiceResult> AddToCart(
        Guid userId,
        AddToCartRequest request,
        CancellationToken cancellationToken = default)
    {
        try
        {
            throw new NotImplementedException();
        }
        catch (ValidationException ex)
        {
            logger.LogError(
                ex,
                "Domain validation failed.User Id: {UserId}, Error: {Error}",
                userId,
                ex.Message);

            return CartResults<ValidationError>.ValidationFailed(ex.ToError()); 
        }
        catch (Exception ex)
        {
            logger.LogError(
                ex,
                "Failed to add item into cart. User Id: {UserId}, Error: {Error}",
                userId,
                ex.Message);

            return CartResults<CartDto>.InternalServerError;
        }
    }

    public Task<ServiceResult> ClearCart(Guid userId, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<ServiceResult> GetCart(Guid userId, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<ServiceResult> RemovedFromCart(
        Guid userId,
        RemovedFromCartRequest request,
        CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}

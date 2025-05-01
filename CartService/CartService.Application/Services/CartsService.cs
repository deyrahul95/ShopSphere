using System.Net;
using CartService.Application.DTOs;
using CartService.Application.Extensions;
using CartService.Application.Models;
using CartService.Application.Results;
using CartService.Application.Services.Interfaces;
using CartService.Domain.Entities;
using CartService.Domain.Exceptions;
using CartService.Domain.Repositories;
using Microsoft.Extensions.Logging;

namespace CartService.Application.Services;

public class CartsService(
    ICartRepository cartRepository,
    IUserHttpClient userHttpClient,
    IProductHttpClient productHttpClient,
    ILogger<CartsService> logger) : ICartsService
{
    public async Task<ServiceResult<CartDto>> AddToCart(
        Guid userId,
        AddToCartRequest request,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var userResult = await FetchUser(userId);

            if (userResult.isSuccess is false)
            {
                return CartResults<CartDto>.HttpRequestFailed(userResult.result);
            }

            var (isSuccess, result) = await FetchProduct(request.ProductId);

            if (isSuccess is false)
            {
                return CartResults<CartDto>.HttpRequestFailed(result);
            }

            var product = result?.Data;

            if (product is null || product.InStock is false)
            {
                logger.LogWarning("Product is unavailable. Product Id: {ProductId}", request.ProductId);
                return CartResults<CartDto>.OutOfStock(request.ProductId);
            }

            var cart = await FetchCartByUserId(userId: userId, cancellationToken: cancellationToken);

            cart = cart != null
                ? await AddItemToCart(
                    cart: cart,
                    product: product,
                    quantity: request.Quantity,
                    cancellationToken: cancellationToken)
                : await CreateCartWithItem(
                    userId: userId,
                    product: product,
                    quantity: request.Quantity,
                    cancellationToken: cancellationToken);

            return CartResults<CartDto>.CartItemAdded(cart.ToDto());
        }
        catch (ValidationException ex)
        {
            logger.LogError(
                ex,
                "Domain validation failed. User Id: {UserId}, Error: {Error}",
                userId,
                ex.Message);

            return CartResults<CartDto>.ValidationFailed([ex.ToError()]);
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

    public async Task<ServiceResult<CartDto>> GetCart(Guid userId, CancellationToken cancellationToken = default)
    {
        try
        {
            var cart = await FetchCartByUserId(userId: userId, cancellationToken: cancellationToken);

            if (cart is null)
            {
                return CartResults<CartDto>.CartNotFound;
            }
            
            return CartResults<CartDto>.CartFetched(cart.ToDto());
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

    public async Task<ServiceResult> RemovedFromCart(
        Guid userId,
        RemovedFromCartRequest request,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var cart = await FetchCartByUserId(userId: userId, cancellationToken: cancellationToken);

            if (cart is null)
            {
                return CartResults.CartNotFound;
            }

            cart.RemoveItem(request.ProductId);

            await cartRepository.Update(cart: cart, cancellationToken: cancellationToken);

            return CartResults.NoContent;
        }
        catch (ValidationException ex)
        {
            logger.LogError(
                ex,
                "Domain validation failed. User Id: {UserId}, Error: {Error}",
                userId,
                ex.Message);

            return CartResults<CartDto>.ValidationFailed([ex.ToError()]);
        }
        catch (Exception ex)
        {
            logger.LogError(
                ex,
                "Failed to add item into cart. User Id: {UserId}, Error: {Error}",
                userId,
                ex.Message);

            return CartResults.InternalServerError;
        }
    }

    public async Task<ServiceResult> ClearCart(Guid userId, CancellationToken cancellationToken = default)
    {
        try
        {
            var cart = await FetchCartByUserId(userId: userId, cancellationToken: cancellationToken);

            if (cart is null)
            {
                return CartResults.CartNotFound;
            }

            cart.Clear();
            await cartRepository.Update(cart: cart, cancellationToken: cancellationToken);

            return CartResults.NoContent;
        }
        catch (Exception ex)
        {
            logger.LogError(
                ex,
                "Failed to add item into cart. User Id: {UserId}, Error: {Error}",
                userId,
                ex.Message);

            return CartResults.InternalServerError;
        }
    }

    private async Task<(bool isSuccess, ServiceResult<UserDto>? result)> FetchUser(Guid userId)
    {
        logger.LogInformation("Fetching user data. User Id: {UserId}", userId);
        var result = await userHttpClient.GetUser(userId);

        if (result == null || result.StatusCode != HttpStatusCode.OK)
        {
            logger.LogWarning(
                "Failed to fetched user data. User Id: {UserId}, Result: {@Result}",
                userId,
                result);
            return (false, result);
        }

        logger.LogInformation(
            "User data fetched successfully. User Id: {UserId}, Status Code: {StatusCode}",
            userId,
            result.StatusCode);
        return (true, result);
    }

    private async Task<(bool isSuccess, ServiceResult<ProductDto>? result)> FetchProduct(Guid productId)
    {
        logger.LogInformation("Fetching product data. Product Id: {ProductId}", productId);
        var result = await productHttpClient.GetProduct(productId);

        if (result == null || result.StatusCode != HttpStatusCode.OK)
        {
            logger.LogWarning(
                "Failed to fetched product data. Product Id: {ProductId}, Result: {@Result}",
                productId,
                result);
            return (false, result);
        }

        logger.LogInformation(
            "Product data fetched successfully. Product Id: {ProductId}, Status Code: {StatusCode}",
            productId,
            result.StatusCode);
        return (true, result);
    }

    private async Task<Cart?> FetchCartByUserId(Guid userId, CancellationToken cancellationToken)
    {
        logger.LogInformation("Fetching cart data. User Id: {UserId}", userId);
        var cart = await cartRepository.GetByUserId(userId: userId, cancellationToken: cancellationToken);

        if (cart is null)
        {
            logger.LogWarning("Cart does not exist. UserId: {UserId}", userId);
            return null;
        }

        logger.LogInformation("Cart data fetched successfully. User Id: {UserId}, Cart Id: {CartId}", userId, cart.Id);
        return cart;
    }

    private async Task<Cart> CreateCartWithItem(
        Guid userId,
        ProductDto product,
        int quantity,
        CancellationToken cancellationToken)
    {
        var newCart = Cart.Create(userId);

        newCart.AddItem(
            productId: product.Id,
            productName: product.Name,
            quantity: quantity,
            price: product.Price);

        return await cartRepository.Create(cart: newCart, cancellationToken: cancellationToken);
    }

    private async Task<Cart> AddItemToCart(
        Cart cart,
        ProductDto product,
        int quantity,
        CancellationToken cancellationToken)
    {
        cart.AddItem(
            productId: product.Id,
            productName: product.Name,
            quantity: quantity,
            price: product.Price);

        await cartRepository.Update(cart: cart, cancellationToken: cancellationToken);
        return cart;
    }
}

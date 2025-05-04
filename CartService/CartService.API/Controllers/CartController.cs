using System.Net;
using System.Security.Claims;
using CartService.Application.DTOs;
using CartService.Application.Models;
using CartService.Application.Results;
using CartService.Application.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CartService.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class CartController(ICartsService cartsService, ILogger<CartController> logger) : ControllerBase
{
    [HttpPost]
    [Route("add")]
    public async Task<ActionResult<ServiceResult<CartDto>>> AddItemToCart(
        [FromBody] AddToCartRequest request,
        CancellationToken cancellationToken = default)
    {
        var userId = GetLoggedInUserId();

        if (userId.Equals(Guid.Empty))
        {
            return Unauthorized();
        }

        logger.LogInformation(
            "Start processing add to cart item request. User Id: {UserId}, Request: {@Request}",
            userId,
            request);
        var response = await cartsService.AddItemToCart(
            userId: userId,
            request: request,
            cancellationToken: cancellationToken);

        logger.LogInformation(
            "Completed processing add to cart item request. User Id: {UserId}, Status Code: {StatusCode}",
            userId,
            response.StatusCode);
        return StatusCode((int)response.StatusCode, response);
    }

    [HttpGet]
    public async Task<ActionResult<ServiceResult<CartDto>>> GetCart(CancellationToken cancellationToken = default)
    {
        var userId = GetLoggedInUserId();

        if (userId.Equals(Guid.Empty))
        {
            return Unauthorized();
        }

        logger.LogInformation("Start processing get cart request. User Id: {UserId}", userId);
        var response = await cartsService.GetCart(userId: userId, cancellationToken: cancellationToken);

        logger.LogInformation(
            "Completed processing get cart request. User Id: {UserId}, Status Code: {StatusCode}",
            userId,
            response.StatusCode);
        return StatusCode((int)response.StatusCode, response);
    }

    [HttpDelete]
    [Route("removed")]
    public async Task<ActionResult<ServiceResult<CartDto>>> RemoveItemFromCart(
        [FromBody] RemovedFromCartRequest request,
        CancellationToken cancellationToken = default)
    {
        var userId = GetLoggedInUserId();

        if (userId.Equals(Guid.Empty))
        {
            return Unauthorized();
        }

        logger.LogInformation(
            "Start processing removed from cart item request. User Id: {UserId}, Request: {@Request}",
            userId,
            request);
        var response = await cartsService.RemoveItemFromCart(
            userId: userId,
            request: request,
            cancellationToken: cancellationToken);

        logger.LogInformation(
            "Completed processing removed from cart item request. User Id: {UserId}, Status Code: {StatusCode}",
            userId,
            response.StatusCode);

        if (response.StatusCode == HttpStatusCode.NoContent)
        {
            return NoContent();
        }

        return StatusCode((int)response.StatusCode, response);
    }

    [HttpDelete]
    public async Task<ActionResult<ServiceResult<CartDto>>> ClearCart(CancellationToken cancellationToken = default)
    {
        var userId = GetLoggedInUserId();

        if (userId.Equals(Guid.Empty))
        {
            return Unauthorized();
        }

        logger.LogInformation("Start processing clear cart request. User Id: {UserId}", userId);
        var response = await cartsService.ClearCart(userId: userId, cancellationToken: cancellationToken);

        logger.LogInformation(
            "Completed processing clear cart request. User Id: {UserId}, Status Code: {StatusCode}",
            userId,
            response.StatusCode);

        if (response.StatusCode == HttpStatusCode.NoContent)
        {
            return NoContent();
        }

        return StatusCode((int)response.StatusCode, response);
    }

    private Guid GetLoggedInUserId()
    {
        logger.LogInformation("Fetching user name identifier claim");
        var userIdClaim = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier);

        if (userIdClaim is null || string.IsNullOrEmpty(userIdClaim.Value))
        {
            logger.LogWarning("User name identifier claim not found");
            return Guid.Empty;
        }

        if (Guid.TryParse(userIdClaim.Value, out Guid userId) is false)
        {
            logger.LogWarning("Failed to parse user name identifier value to guid. Value: {Value}", userIdClaim.Value);
            return Guid.Empty;
        }

        logger.LogInformation("User name identifier claim fetched successfully. User Id: {UserId}", userId);
        return userId;
    }
}

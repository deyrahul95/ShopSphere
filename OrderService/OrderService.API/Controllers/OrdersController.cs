using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OrderService.Application.DTOs;
using OrderService.Application.Models;
using OrderService.Application.Results;
using OrderService.Application.Services.Interfaces;

namespace OrderService.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class OrdersController(IOrdersService orderService, ILogger<OrdersController> logger) : ControllerBase
{
    [HttpPost]
    public async Task<ActionResult<ServiceResult<OrderDto>>> Create(
        [FromBody] CreateOrderRequest request,
        CancellationToken cancellationToken = default)
    {
        var userId = GetLoggedInUserId();

        if (userId.Equals(Guid.Empty))
        {
            return Unauthorized();
        }

        logger.LogInformation(
            "Start processing create order request. User Id: {UserId}, Request: {@Request}",
            userId,
            request);

        var response = await orderService.CreateOrder(
            userId: userId,
            request: request,
            cancellationToken: cancellationToken);

        logger.LogInformation(
            "Completed processing create order request. User Id: {UserId}, Status Code: {StatusCode}",
            userId,
            response.StatusCode);
        return StatusCode((int)response.StatusCode, response);
    }

    [HttpGet]
    [Route("{id:guid}")]
    public async Task<ActionResult<ServiceResult<OrderDto>>> Get(
        [FromRoute] Guid id,
        CancellationToken cancellationToken = default)
    {
        var userId = GetLoggedInUserId();

        if (userId.Equals(Guid.Empty))
        {
            return Unauthorized();
        }

        logger.LogInformation(
            "Start processing get order request. User Id: {UserId}, Order Id: {OrderId}",
            userId,
            id);

        var response = await orderService.GetOrder(
            orderId: id,
            userId: userId,
            cancellationToken: cancellationToken);

        logger.LogInformation(
            "Completed processing get order request. User Id: {UserId}, Order Id: {OrderId} Status Code: {StatusCode}",
            userId,
            id,
            response.StatusCode);
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

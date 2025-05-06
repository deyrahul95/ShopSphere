using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PaymentService.Application.DTOs;
using PaymentService.Application.Models;
using PaymentService.Application.Results;
using PaymentService.Application.Services.Interfaces;

namespace PaymentService.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class PaymentController(IPaymentService paymentService, ILogger<PaymentController> logger) : ControllerBase
{
    [HttpPost]
    [Route("process")]
    public async Task<ActionResult<ServiceResult<PaymentDto>>> ProcessedPayment(
        [FromBody] ProcessPaymentRequest request,
        CancellationToken cancellationToken = default)
    {
        if (ModelState.IsValid is false)
        {
            return BadRequest(ModelState);
        }
        
        var userId = GetLoggedInUserId();

        if (userId.Equals(Guid.Empty))
        {
            return Unauthorized();
        }

        logger.LogInformation(
           "Start processing payment request. Request: {@Request}",
           request);

        var response = await paymentService.ProcessPayment(
            userId: userId,
            request: request,
            cancellationToken: cancellationToken);

        logger.LogInformation(
            "Completed processing payment request. Status Code: {StatusCode}",
            response.StatusCode);

        return StatusCode(statusCode: (int)response.StatusCode, value: response);
    }

    [HttpGet]
    [Route("{orderId:guid}")]
    public async Task<ActionResult<ServiceResult<PaymentDto>>> GetPaymentDetails(
        [FromRoute] Guid orderId,
        CancellationToken cancellationToken = default)
    {
        logger.LogInformation(
           "Start processing get payment details request. Order Id: {OrderId}",
           orderId);

        var response = await paymentService.GetPaymentDetails(
            orderId: orderId,
            cancellationToken: cancellationToken);

        logger.LogInformation(
            "Completed processing get payment details request. Order Id: {OrderId} Status Code: {StatusCode}",
            orderId,
            response.StatusCode);
        return StatusCode(statusCode: (int)response.StatusCode, value: response);
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

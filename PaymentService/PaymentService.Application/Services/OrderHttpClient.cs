using System.Net.Http.Json;
using Microsoft.Extensions.Logging;
using PaymentService.Application.DTOs;
using PaymentService.Application.Results;
using PaymentService.Application.Services.Interfaces;

namespace PaymentService.Application.Services;

public class OrderHttpClient(HttpClient httpClient, ILogger<OrderHttpClient> logger) : IOrderHttpClient
{
    public async Task<ServiceResult<OrderDto>?> GetOrder(Guid orderId, CancellationToken cancellationToken = default)
    {
         var requestPath = $"/api/orders/{orderId}";
        var uri = $"{httpClient.BaseAddress}{requestPath}";

        logger.LogInformation("Start processing get order request. URI: {URI}", uri);
        var response = await httpClient.GetAsync(requestUri: requestPath, cancellationToken: cancellationToken);

        logger.LogInformation(
            "Completed processing get order request. URI: {URI}, StatusCode: {StatusCode}",
            uri,
            response.StatusCode);

        return await response.Content.ReadFromJsonAsync<ServiceResult<OrderDto>>(cancellationToken: cancellationToken);
    }
}

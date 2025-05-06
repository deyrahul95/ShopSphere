using System.Net.Http.Json;
using Microsoft.Extensions.Logging;
using OrderService.Application.DTOs;
using OrderService.Application.Models;
using OrderService.Application.Results;
using OrderService.Application.Services.Interfaces;

namespace OrderService.Application.Services;

public class PaymentHttpClient(HttpClient httpClient, ILogger<PaymentHttpClient> logger) : IPaymentHttpClient
{
    public async Task<ServiceResult<PaymentDto>?> ProcessPayment(
        ProcessPaymentRequest request,
        CancellationToken cancellationToken = default)
    {
        var requestPath = $"/api/payment/process";
        var uri = $"{httpClient.BaseAddress}{requestPath}";

        logger.LogInformation("Start processing payment request. URI: {URI}", uri);
        var response = await httpClient.GetAsync(
            requestUri: requestPath,
            cancellationToken: cancellationToken);

        logger.LogInformation(
            "Completed processing payment request. URI: {URI}, StatusCode: {StatusCode}",
            uri,
            response.StatusCode);

        return await response.Content.ReadFromJsonAsync<ServiceResult<PaymentDto>>(cancellationToken: cancellationToken);

    }
}

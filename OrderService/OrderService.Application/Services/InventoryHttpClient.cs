using System.Net.Http.Headers;
using System.Net.Http.Json;
using Microsoft.Extensions.Logging;
using OrderService.Application.Models;
using OrderService.Application.Results;
using OrderService.Application.Services.Interfaces;

namespace OrderService.Application.Services;

public class InventoryHttpClient(
    HttpClient httpClient,
    ILogger<InventoryHttpClient> logger) : IInventoryHttpClient
{
    public async Task<ServiceResult<InventoryResponse>?> CheckInventory(
        InventoryRequest request,
        string? token = null,
        CancellationToken cancellationToken = default)
    {
        var requestPath = $"/api/inventory/check";
        var uri = $"{httpClient.BaseAddress}{requestPath}";

        if (string.IsNullOrEmpty(token) is false)
        {
            token = token.Replace("Bearer", "").Trim();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }

        logger.LogInformation("Start processing check inventory request. URI: {URI}", uri);
        var response = await httpClient.PostAsJsonAsync(
            requestUri: requestPath,
            value: request,
            cancellationToken: cancellationToken);

        logger.LogInformation(
            "Completed processing check inventory request. URI: {URI}, StatusCode: {StatusCode}",
            uri,
            response.StatusCode);

        return await response.Content.ReadFromJsonAsync<ServiceResult<InventoryResponse>>(cancellationToken: cancellationToken);
    }
}

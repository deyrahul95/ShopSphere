using System.Net.Http.Json;
using CartService.Application.DTOs;
using CartService.Application.Results;
using CartService.Application.Services.Interfaces;
using Microsoft.Extensions.Logging;

namespace CartService.Application.Services;

public class ProductHttpClient(HttpClient httpClient, ILogger<ProductHttpClient> logger) : IProductHttpClient
{
    public async Task<ServiceResult<ProductDto>?> GetProduct(Guid productId)
    {
        var requestPath = $"/products/{productId}";
        var uri = $"{httpClient.BaseAddress}{requestPath}";

        logger.LogInformation("Start processing get product request. URI: {URI}", uri);
        var response = await httpClient.GetAsync(requestPath);

        if (response.IsSuccessStatusCode is false)
        {
            logger.LogWarning(
                "Failed to process request. URI: {uri}, Response: {@Response}",
                uri,
                response);

            return null;
        }

        logger.LogInformation(
            "Stop processing get product request. URI: {URI}, StatusCode: {StatusCode}",
            uri,
            response.StatusCode);

        return await response.Content.ReadFromJsonAsync<ServiceResult<ProductDto>>();
    }
}

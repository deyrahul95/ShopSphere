using System.Net;
using System.Net.Http.Json;
using Microsoft.Extensions.Logging;
using OrderService.Application.DTOs;
using OrderService.Application.Results;
using OrderService.Application.Services.Interfaces;

namespace OrderService.Application.Services;

public class CartHttpClient(
    HttpClient httpClient,
    ILogger<CartHttpClient> logger) : ICartHttpClient
{
    public async Task<ServiceResult<CartDto>?> GetCart()
    {
        var requestPath = $"/api/cart";
        var uri = $"{httpClient.BaseAddress}{requestPath}";

        logger.LogInformation("Start processing get cart request. URI: {URI}", uri);
        var response = await httpClient.GetAsync(requestPath);

        logger.LogInformation(
            "Completed processing get cart request. URI: {URI}, StatusCode: {StatusCode}",
            uri,
            response.StatusCode);

        return await response.Content.ReadFromJsonAsync<ServiceResult<CartDto>>();
    }

    public async Task<HttpStatusCode> ClearCart()
    {
        var requestPath = $"/api/cart";
        var uri = $"{httpClient.BaseAddress}{requestPath}";

        logger.LogInformation("Start processing clear cart request. URI: {URI}", uri);
        var response = await httpClient.DeleteAsync(requestPath);

        logger.LogInformation(
            "Completed processing clear cart request. URI: {URI}, StatusCode: {StatusCode}",
            uri,
            response.StatusCode);

        return response.StatusCode;
    }
}

using System.Net.Http.Json;
using CartService.Application.DTOs;
using CartService.Application.Results;
using CartService.Application.Services.Interfaces;
using Microsoft.Extensions.Logging;

namespace CartService.Application.Services;

public class UserHttpClient(HttpClient httpClient, ILogger<UserHttpClient> logger) : IUserHttpClient
{
    public async Task<ServiceResult<UserDto>?> GetUser(Guid userId)
    {
        var requestPath = $"/api/users/{userId}";
        var uri = $"{httpClient.BaseAddress}{requestPath}";

        logger.LogInformation("Start processing get user request. URI: {URI}", uri);
        var response = await httpClient.GetAsync(requestPath);

        logger.LogInformation(
            "Completed processing get user request. URI: {URI}, StatusCode: {StatusCode}",
            uri,
            response.StatusCode);

        return await response.Content.ReadFromJsonAsync<ServiceResult<UserDto>>();
    }
}

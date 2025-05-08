using Microsoft.Extensions.Logging;
using UserService.Application.Models;
using UserService.Application.Results;
using UserService.Application.Services.Interfaces;
using UserService.Domain.Entities;
using UserService.Domain.Providers;
using UserService.Domain.Repositories;

namespace UserService.Application.Services;

public class AuthService(
    IUserRepository userRepository,
    ITokenProvider tokenProvider,
    ILogger<AuthService> logger) : IAuthService
{
    public async Task<ServiceResult<LoginResponse>> Login(
        LoginRequest request,
        CancellationToken cancellationToken = default)
    {
        try
        {
            logger.LogInformation("Fetching user data. Username: {username}", request.Username);
            var user = await userRepository.FindByUserNameAsync(
                username: request.Username,
                cancellationToken: cancellationToken);

            if (user is null)
            {
                logger.LogInformation("User not found. Username: {username}", request.Username);
                return AuthResults<LoginResponse>.InvalidCredentials;
            }

            if (IsValidPassword(user, request.Password) is false)
            {
                logger.LogInformation("Invalid password. Username: {username}", request.Username);
                return AuthResults<LoginResponse>.InvalidCredentials;
            }

            var token = tokenProvider.GenerateAccessToken(user);

            return AuthResults<LoginResponse>.LoggedIn(data: new LoginResponse(Token: token));
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to login. Error: {err}", ex.Message);
            return AuthResults<LoginResponse>.InternalServerError;
        }
    }

    private static bool IsValidPassword(User user, string password)
    {
        return user.Password.Equals(password);
    }
}

using Microsoft.Extensions.Logging;
using UserService.Application.Models;
using UserService.Application.Services.Interfaces;
using UserService.Domain.Entities;
using UserService.Domain.Providers;
using UserService.Domain.Repositories;
using UserService.Domain.Results;

namespace UserService.Application.Services;

public class AuthService(
    IUserRepository userRepository,
    ITokenProvider tokenProvider,
    ILogger<UserService> logger) : IAuthService
{
    public async Task<ServiceResult<LoginResponseModel>> Login(
        LoginRequestModel request,
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
                return AuthResults<LoginResponseModel>.InvalidCredentials;
            }

            if (IsValidPassword(user, request.Password) is false)
            {
                logger.LogInformation("Invalid password. Username: {username}", request.Username);
                return AuthResults<LoginResponseModel>.InvalidCredentials;
            }

            var token = await tokenProvider.GenerateAccessToken(user);

            return AuthResults<LoginResponseModel>.LoggedIn(data: new LoginResponseModel(Token: token));
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to login. Error: {err}", ex.Message);
            return AuthResults<LoginResponseModel>.InternalServerError;
        }
    }

    private static bool IsValidPassword(User user, string password)
    {
        return user.Password.Equals(password);
    }
}

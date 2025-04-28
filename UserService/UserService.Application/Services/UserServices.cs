using Microsoft.Extensions.Logging;
using UserService.Application.DTOs;
using UserService.Application.Extensions;
using UserService.Application.Services.Interfaces;
using UserService.Domain.Repositories;
using UserService.Domain.Results;

namespace UserService.Application.Services;

public class UserServices(
    IUserRepository userRepository,
    ILogger<UserServices> logger) : IUserService
{
    public async Task<ServiceResult<UserDto>> GetUserDetails(Guid userId, CancellationToken cancellationToken = default)
    {
        try
        {
            logger.LogInformation("Fetching user details. User Id: {id}", userId);
            var user = await userRepository.FindByIdAsync(
                id: userId,
                cancellationToken: cancellationToken);

            if (user is null)
            {
                logger.LogInformation("User not found. User Id: {id}", userId);
                return UserResults<UserDto>.NotFound(userId);
            }

            return UserResults<UserDto>.Success(user.ToDto());
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to login. Error: {err}", ex.Message);
            return UserResults<UserDto>.InternalServerError;
        }
    }
}

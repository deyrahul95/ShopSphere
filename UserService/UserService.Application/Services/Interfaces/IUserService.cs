using UserService.Application.DTOs;
using UserService.Domain.Results;

namespace UserService.Application.Services.Interfaces;

public interface IUserService
{
    Task<ServiceResult<UserDto>> GetUserDetails(Guid userId, CancellationToken cancellationToken = default);
}

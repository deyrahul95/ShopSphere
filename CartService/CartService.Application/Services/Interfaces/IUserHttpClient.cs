using CartService.Application.DTOs;
using CartService.Application.Results;

namespace CartService.Application.Services.Interfaces;

public interface IUserHttpClient
{
    Task<ServiceResult<UserDto>?> GetUser(Guid userId);
}

using UserService.Application.Models;
using UserService.Application.Results;

namespace UserService.Application.Services.Interfaces;

public interface IAuthService
{
    Task<ServiceResult<LoginResponse>> Login(LoginRequest request, CancellationToken cancellationToken = default);
}

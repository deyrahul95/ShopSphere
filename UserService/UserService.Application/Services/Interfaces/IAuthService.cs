using UserService.Application.Models;
using UserService.Application.Results;

namespace UserService.Application.Services.Interfaces;

public interface IAuthService
{
    Task<ServiceResult<LoginResponseModel>> Login(LoginRequestModel request, CancellationToken cancellationToken = default);
}

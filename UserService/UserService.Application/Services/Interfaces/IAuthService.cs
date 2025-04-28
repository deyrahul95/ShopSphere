using UserService.Application.Models;
using UserService.Domain.Results;

namespace UserService.Application.Services.Interfaces;

public interface IAuthService
{
    Task<ServiceResult<LoginResponseModel>> Login(LoginRequestModel request, CancellationToken cancellationToken = default);
}

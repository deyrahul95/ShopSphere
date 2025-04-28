using UserService.Domain.Entities;

namespace UserService.Domain.Providers;

public interface ITokenProvider
{
    Task<string> GenerateAccessToken(User user);
}

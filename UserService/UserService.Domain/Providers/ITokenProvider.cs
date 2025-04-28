using UserService.Domain.Entities;

namespace UserService.Domain.Providers;

public interface ITokenProvider
{
    string GenerateAccessToken(User user);
}

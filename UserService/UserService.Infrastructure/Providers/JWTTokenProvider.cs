using UserService.Domain.Entities;
using UserService.Domain.Providers;

namespace UserService.Infrastructure.Providers;

public class JWTTokenProvider : ITokenProvider
{
    public Task<string> GenerateAccessToken(User user)
    {
        throw new NotImplementedException();
    }
}

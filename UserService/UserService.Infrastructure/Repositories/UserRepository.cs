using UserService.Domain.Entities;
using UserService.Domain.Repositories;

namespace UserService.Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    public Task<User?> FindByIdAsync(int id)
    {
        throw new NotImplementedException();
    }

    public Task<User?> FindByUserNameAsync(string username)
    {
        throw new NotImplementedException();
    }
}

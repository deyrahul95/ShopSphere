using UserService.Domain.Entities;
using UserService.Domain.Repositories;
using UserService.Infrastructure.DB;

namespace UserService.Infrastructure.Repositories;

public class InMemoryUserRepository : IUserRepository
{
    public async Task<User?> FindByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        await Task.Delay(10, cancellationToken);

        return InMemoryDB.Users.FirstOrDefault(x => x.Id == id);
    }

    public async Task<User?> FindByUserNameAsync(string username, CancellationToken cancellationToken = default)
    {
        await Task.Delay(10, cancellationToken);

        return InMemoryDB.Users.FirstOrDefault(x => x.UserName == username.ToLower());
    }
}

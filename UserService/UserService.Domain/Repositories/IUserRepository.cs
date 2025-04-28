using UserService.Domain.Entities;

namespace UserService.Domain.Repositories;

public interface IUserRepository
{
    Task<User?> FindByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<User?> FindByUserNameAsync(string username, CancellationToken cancellationToken = default);
}

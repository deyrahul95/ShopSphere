using UserService.Domain.Entities;
using UserService.Domain.Enums;

namespace UserService.Infrastructure.DB;

public class InMemoryDB
{
    public readonly static List<User> Users = [
        new User () {
            Id = Guid.NewGuid (),
            Name = "Admin",
            Email = "admin@shopsphere.com",
            UserName = "admin_user",
            Password = "Admin@123456",
            Roles = [nameof(UserRoles.User), nameof(UserRoles.Admin)],
            Address = "12/3, Admin Street, Kolkata, W.B., 700001",
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
        },
        new User () {
            Id = Guid.NewGuid (),
            Name = "Default",
            Email = "default@shopsphere.com",
            UserName = "default_user",
            Password = "Default@123456",
            Roles = [nameof(UserRoles.User)],
            Address = "15/5, Default Street, Kolkata, W.B., 700001",
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
        }
    ];
}

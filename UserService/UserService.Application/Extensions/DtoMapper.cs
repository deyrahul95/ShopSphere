using UserService.Application.DTOs;
using UserService.Domain.Entities;

namespace UserService.Application.Extensions;

public static class DtoMapper
{
    public static UserDto ToDto(this User user)
    {
        return new UserDto(
            Id: user.Id,
            Name: user.Name,
            Email: user.Email,
            Address: user.Address ?? "N/A"
        );
    }
}

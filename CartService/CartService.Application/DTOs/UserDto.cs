namespace CartService.Application.DTOs;

public record UserDto(
    Guid Id,
    string Name,
    string Email,
    string Address
);
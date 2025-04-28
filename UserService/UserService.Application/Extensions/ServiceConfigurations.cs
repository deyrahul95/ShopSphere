using Microsoft.Extensions.DependencyInjection;
using UserService.Application.Services;
using UserService.Application.Services.Interfaces;

namespace UserService.Application.Extensions;

public static class ServiceConfigurations
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<IUserService, UserServices>();
        services.AddScoped<IAuthService, AuthService>();

        return services;
    }
}

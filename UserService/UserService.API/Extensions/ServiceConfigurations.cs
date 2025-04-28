using UserService.Application.Extensions;
using UserService.Infrastructure.Extensions;

namespace UserService.API.Extensions;

public static class ServiceConfigurations
{
    public static IServiceCollection AddApiServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddInfrastructureServices(configuration);
        services.AddApplicationServices();

        return services;
    }

    public static IApplicationBuilder UseApiServices(this IApplicationBuilder app)
    {
        app.UseInfrastructureServices();
        
        return app;
    }
}

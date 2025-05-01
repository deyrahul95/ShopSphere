using CartService.Infrastructure.Extensions;
using CartService.Application.Extensions;
using CartService.API.Middlewares;

namespace CartService.API.Extensions;

public static class ServiceConfigurations
{
     public static IServiceCollection AddApiServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddInfrastructureServices(configuration);
        services.AddApplicationServices(configuration);

        return services;
    }

    public static IApplicationBuilder UseApiServices(this IApplicationBuilder app)
    {
        app.UseMiddleware<ExceptionHandlingMiddleware>();

        app.UseInfrastructureServices();
        
        return app;
    }
}

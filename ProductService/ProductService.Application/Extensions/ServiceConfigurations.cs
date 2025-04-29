using Microsoft.Extensions.DependencyInjection;
using ProductService.Application.Services;
using ProductService.Application.Services.Interfaces;

namespace ProductService.Application.Extensions;

public static class ServiceConfigurations
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<IProductsService, ProductsService>();

        return services;
    }
}

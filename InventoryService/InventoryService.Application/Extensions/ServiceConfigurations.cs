using InventoryService.Application.Services;
using InventoryService.Application.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace InventoryService.Application.Extensions;

public static class ServiceConfigurations
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<IInventoryService, InventoryServiceImpl>();

        return services;
    }
}

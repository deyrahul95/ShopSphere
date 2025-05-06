using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PaymentService.Application.Services;
using PaymentService.Application.Services.Interfaces;

namespace PaymentService.Application.Extensions;

public static class ServiceConfigurations
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IPaymentService, PaymentServiceImpl>();
        
        return services;
    }
}

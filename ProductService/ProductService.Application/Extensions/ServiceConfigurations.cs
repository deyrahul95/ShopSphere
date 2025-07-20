using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.Extensions.DependencyInjection;
using ProductService.Application.Models;
using ProductService.Application.Services;
using ProductService.Application.Services.Interfaces;

namespace ProductService.Application.Extensions;

public static class ServiceConfigurations
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddFluentValidationAutoValidation();
        services.AddFluentValidationClientsideAdapters();
        services.AddValidatorsFromAssemblyContaining<ProductSearchValidator>();

        services.AddScoped<IProductsService, ProductsService>();

        return services;
    }
}

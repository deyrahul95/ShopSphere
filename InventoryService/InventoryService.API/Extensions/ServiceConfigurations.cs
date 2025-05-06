using InventoryService.API.Middlewares;
using OpenTelemetry.Exporter;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using InventoryService.Infrastructure.Extensions;
using InventoryService.Application.Extensions;

namespace InventoryService.API.Extensions;

public static class ServiceConfigurations
{
     public static IServiceCollection AddApiServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddInfrastructureServices(configuration);
        services.AddApplicationServices();

        services.AddOpenTelemetry()
            .ConfigureResource(resource => resource.AddService("ProductService"))
            .WithTracing(tracing =>
            {
                tracing
                    .AddHttpClientInstrumentation()
                    .AddAspNetCoreInstrumentation()
                    .AddOtlpExporter(options => 
                    {
                        options.Endpoint = new Uri(configuration["OTLPEndpoint"] ?? "");
                        options.Protocol = OtlpExportProtocol.HttpProtobuf;
                    });
            });

        return services;
    }

    public static IApplicationBuilder UseApiServices(this IApplicationBuilder app)
    {
        app.UseMiddleware<ExceptionHandlingMiddleware>();

        object value = app.UseInfrastructureServices();
        
        return app;
    }
}

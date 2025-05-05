using OpenTelemetry.Exporter;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using OrderService.API.Middlewares;
using OrderService.Infrastructure.Extensions;
using OrderService.Application.Extensions;
using OrderService.API.Constants;

namespace OrderService.API.Extensions;

public static class ServiceConfigurations
{
    public static IServiceCollection AddApiServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddInfrastructureServices(configuration);
        services.AddApplicationServices(configuration);

        services.AddOpenTelemetry()
            .ConfigureResource(resource => resource.AddService(OpenTelemetryConstants.ServiceName))
            .WithTracing(tracing =>
            {
                tracing
                    .AddHttpClientInstrumentation()
                    .AddAspNetCoreInstrumentation()
                    .AddOtlpExporter(options => 
                    {
                        options.Endpoint = new Uri(configuration[OpenTelemetryConstants.OTLPEndpoint] ?? "");
                        options.Protocol = OtlpExportProtocol.HttpProtobuf;
                    });
            });

        return services;
    }

    public static IApplicationBuilder UseApiServices(this IApplicationBuilder app)
    {
        app.UseMiddleware<ExceptionHandlingMiddleware>();

        app.UseInfrastructureServices();

        return app;
    }
}
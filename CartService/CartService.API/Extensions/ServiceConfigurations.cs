using CartService.Infrastructure.Extensions;
using CartService.Application.Extensions;
using CartService.API.Middlewares;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using OpenTelemetry.Exporter;
using CartService.API.Constants;

namespace CartService.API.Extensions;

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

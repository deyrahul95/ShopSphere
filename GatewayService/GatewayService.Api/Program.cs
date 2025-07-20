using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Ocelot.Cache.CacheManager;
using Serilog;
using OpenTelemetry.Trace;
using OpenTelemetry.Resources;
using OpenTelemetry.Exporter;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((context, loggerConfig) =>
    loggerConfig.ReadFrom.Configuration(context.Configuration));

builder.Configuration.AddJsonFile("ocelot.json", optional: false, reloadOnChange: true);

// builder.Services.AddDiscoveryClient(builder.Configuration);

builder.Services.AddOcelot(builder.Configuration)
    .AddCacheManager(x => x.WithDictionaryHandle());
// .AddConsul()

builder.Services.AddOpenTelemetry()
    .ConfigureResource(resource => resource.AddService("GatewayService"))
    .WithTracing(tracing =>
    {
        tracing
            .AddHttpClientInstrumentation()
            .AddAspNetCoreInstrumentation()
            .AddOtlpExporter(options =>
            {
                options.Endpoint = new Uri(builder.Configuration["OTLPEndpoint"] ?? "");
                options.Protocol = OtlpExportProtocol.HttpProtobuf;
            });
    });

var app = builder.Build();

app.UseSerilogRequestLogging();

app.UseOcelot().Wait();

await app.RunAsync();
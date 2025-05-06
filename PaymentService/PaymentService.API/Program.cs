using PaymentService.API.Extensions;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((context, loggerConfig) => 
    loggerConfig.ReadFrom.Configuration(context.Configuration));

builder.Services.AddHttpContextAccessor();

builder.Services.AddApiServices(builder.Configuration);

builder.Services.AddControllers();

var app = builder.Build();

app.UseSerilogRequestLogging();

app.UseApiServices();

app.MapControllers();

app.Run();


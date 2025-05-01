using CartService.API.Extensions;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((context, loggerConfig) => 
    loggerConfig.ReadFrom.Configuration(context.Configuration));

builder.Services.AddHttpContextAccessor();

builder.Services.AddControllers();

builder.Services.AddApiServices(builder.Configuration);

var app = builder.Build();

app.UseSerilogRequestLogging();

app.UseApiServices();

app.MapControllers();

app.Run();

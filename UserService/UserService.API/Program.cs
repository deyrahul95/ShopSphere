using UserService.API.Extensions;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((context, loggerConfig) => 
    loggerConfig.ReadFrom.Configuration(context.Configuration));

builder.Services.AddControllers();

builder.Services.AddApiServices(builder.Configuration);

var app = builder.Build();

app.UseSerilogRequestLogging();

app.MapControllers();

app.UseApiServices();

app.Run();

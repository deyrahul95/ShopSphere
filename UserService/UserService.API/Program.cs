using UserService.API.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddApiServices(builder.Configuration);

var app = builder.Build();

app.MapControllers();

app.UseApiServices();

app.Run();

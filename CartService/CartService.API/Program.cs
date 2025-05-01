var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpContextAccessor();

builder.Services.AddControllers();

var app = builder.Build();

app.MapControllers();

app.Run();

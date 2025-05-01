using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using UserService.Domain.Providers;
using UserService.Domain.Repositories;
using UserService.Infrastructure.Configs;
using UserService.Infrastructure.DB;
using UserService.Infrastructure.Providers;
using UserService.Infrastructure.Repositories;

namespace UserService.Infrastructure.Extensions;

public static class ServiceConfigurations
{
    public static IServiceCollection AddInfrastructureServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.Configure<JWTConfig>(configuration.GetSection("JWTSettings"));

        var jwtConfig = services.BuildServiceProvider().GetService<IOptions<JWTConfig>>()?.Value;

        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidAudience = jwtConfig?.Audience,
                    ValidIssuer = jwtConfig?.Issuer,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtConfig?.Key!))
                };
            });

        services.AddScoped<ITokenProvider, JWTTokenProvider>();
        
        services.AddSingleton<InMemoryDB>();
        services.AddSingleton<IUserRepository, InMemoryUserRepository>();
         
        return services;
    }

    public static IApplicationBuilder UseInfrastructureServices(this IApplicationBuilder app)
    {
        app.UseAuthentication();
        app.UseAuthorization();

        return app;
    }
}

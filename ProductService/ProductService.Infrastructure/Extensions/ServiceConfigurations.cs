using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using ProductService.Domain.Repositories;
using ProductService.Infrastructure.Configs;
using ProductService.Infrastructure.Constants;
using ProductService.Infrastructure.DB;
using ProductService.Infrastructure.Repositories;

namespace ProductService.Infrastructure.Extensions;

public static class ServiceConfigurations
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<JwtConfig>(configuration.GetSection(JwtConstants.JwtConfigName));

        var jwtConfig = services.BuildServiceProvider().GetService<IOptions<JwtConfig>>()?.Value;

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

        services.AddSingleton<InMemoryDB>();
        services.AddSingleton<IProductRepository, InMemoryProductRepository>();

        return services;
    }

    public static IApplicationBuilder UseInfrastructureServices(this IApplicationBuilder app)
    {
        app.UseAuthentication();
        app.UseAuthorization();

        return app;
    }
}

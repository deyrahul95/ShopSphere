using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using UserService.Domain.Entities;
using UserService.Domain.Providers;
using UserService.Infrastructure.Configs;

namespace UserService.Infrastructure.Providers;

public class JWTTokenProvider(IOptions<JwtConfig> jwtOptions) : ITokenProvider
{
    private readonly JwtConfig _jwtConfig = jwtOptions.Value;
    private const string SecurityAlgorithm = SecurityAlgorithms.HmacSha512;

    public string GenerateAccessToken(User user)
    {
        var key = Encoding.UTF8.GetBytes(_jwtConfig.Key);
        var securityKey = new SymmetricSecurityKey(key);
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithm);
        var expiresIn = DateTime.UtcNow.AddDays(_jwtConfig.AccessTokenExpiresInDays);

        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new(ClaimTypes.Name,user.Name),
            new(ClaimTypes.Email,user.Email),
            new("UserName", user.UserName),
        };

        claims.AddRange(user.Roles.Select(
            role => new Claim(ClaimTypes.Role, role)));

        var token = new JwtSecurityToken(
            issuer: _jwtConfig.Issuer,
            audience: _jwtConfig.Audience,
            claims: claims,
            expires: expiresIn,
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}

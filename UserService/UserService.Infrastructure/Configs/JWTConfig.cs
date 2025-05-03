using UserService.Infrastructure.Constants;

namespace UserService.Infrastructure.Configs;

public class JwtConfig
{
    public string Key { get; set; } = string.Empty;
    public string Issuer { get; set; } = string.Empty;
    public string Audience { get; set; } = string.Empty;
    public int AccessTokenExpiresInDays { get; set; } = JwtConstants.DefaultAccessTokenExpiryInDays;
}

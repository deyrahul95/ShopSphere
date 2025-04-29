using ProductService.Domain.Constants;

namespace ProductService.Infrastructure.Configs;

public class JWTConfig
{
    public string Key { get; set; } = string.Empty;
    public string Issuer { get; set; } = string.Empty;
    public string Audience { get; set; } = string.Empty;
    public int AccessTokenExpiresInDays { get; set; } = DomainConstants.DefaultAccessTokenExpiryInDays;
}

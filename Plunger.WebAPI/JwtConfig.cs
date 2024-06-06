namespace Plunger.WebApi;

public record JwtConfig
{
    public string Issuer { get; init; }
    public string Audience { get; init; }
    public string Key { get; init; }
}
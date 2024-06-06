namespace Plunger.Data;

public record AccessToken()
{
    public Guid TokenId { get; init; }
    public int UserId { get; init; }
    public DateTimeOffset ExpirationTime { get; init; }
}
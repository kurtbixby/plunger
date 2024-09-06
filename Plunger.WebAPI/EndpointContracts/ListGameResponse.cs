namespace Plunger.WebApi.EndpointContracts;

public record ListGameResponse()
{
    public record Game()
    {
        public int Id { get; init; }
        public string Name { get; init; }
        public string CoverUrl { get; init; }
    }
    
    public Game ListGame { get; init; }
    public DateTimeOffset? DateAcquired { get; init; }
    public DateTimeOffset DateStarted { get; init; }
    public int PlatformId { get; init; }
    public int CurrentStatus { get; init; }
    public TimeSpan TimePlayed { get; init; }
}
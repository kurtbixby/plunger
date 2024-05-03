namespace Plunger.WebApi;

public record ListActionMoveGame()
{
    public int GameId { get; init; }
    public int SourceNumber { get; init; }
    public int DestinationNumber { get; init; }
}
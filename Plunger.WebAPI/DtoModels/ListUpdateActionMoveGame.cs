namespace Plunger.WebApi;

public record ListUpdateActionMoveGame()
{
    public int GameId { get; init; }
    public int SourceNumber { get; init; }
    public int DestinationNumber { get; init; }
}
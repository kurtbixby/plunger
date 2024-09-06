namespace Plunger.WebApi.DtoModels;

public class ListDisplayGame
{
    public int Id { get; set; }
    public bool Completed { get; set; }
    public int PlayState { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }
    // public List<PlayStateChange> PlayStateChanges { get; set; }
    public TimeSpan TimePlayed { get; set; }
    public int GameId { get; set; }
}
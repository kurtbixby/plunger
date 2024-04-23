namespace Plunger.Data.DbModels;

public class PlayStateChange
{
    public int Id { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }
    public int NewState { get; set; }
        
    public int GameStatusId { get; set; }
    public GameStatus GameStatus { get; set; }
}
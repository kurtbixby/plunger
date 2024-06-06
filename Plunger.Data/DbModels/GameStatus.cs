namespace Plunger.Data.DbModels;

public class GameStatus : ProtectedEntity
{
    public int Id { get; set; }
    public bool Completed { get; set; }
    public int PlayState { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }
    public List<PlayStateChange> PlayStateChanges { get; set; }
    
    public int UserId { get; set; }
    public User User { get; set; }
    public int GameId { get; set; }
    public Game Game { get; set; }
}
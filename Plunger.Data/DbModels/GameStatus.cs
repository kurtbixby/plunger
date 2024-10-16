using System.Text.Json.Serialization;

namespace Plunger.Data.DbModels;

public class GameStatus
{
    public int Id { get; set; }
    public bool Completed { get; set; }
    public int PlayState { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }
    public List<PlayStateChange> PlayStateChanges { get; set; }
    public TimeSpan TimePlayed { get; set; }
    public DateTimeOffset? TimeStarted { get; set; }
    public Guid VersionId { get; set; }
    
    public int UserId { get; set; }
    [JsonIgnore]
    public User User { get; set; }
    public int GameId { get; set; }
    public Game Game { get; set; }
}
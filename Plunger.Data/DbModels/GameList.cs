namespace Plunger.Data.DbModels;

public class GameList : ProtectedEntity
{
    public int Id { get; set; }
    public string Name { get; set; }
    public bool Unordered { get; set; }
    public List<GameListEntry> GameListEntries { get; set; }
    public Guid VersionId { get; set; }

    public int UserId { get; set; }
    public User User { get; set; }
}
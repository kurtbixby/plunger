namespace Plunger.Data.DbModels;

public class GameList
{
    public int Id { get; set; }
    public string Name { get; set; }
    public bool Unordered { get; set; }
    public List<GameListEntry> GameListEntries { get; set; }

    public int UserId { get; set; }
    public User User { get; set; }
}
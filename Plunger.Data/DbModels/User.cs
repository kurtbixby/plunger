namespace Plunger.Data.DbModels;

public class User
{
    public int Id { get; set; }
    public string Username { get; set; }
    public Collection Collection { get; set; }
    public List<GameStatus> GameStatuses { get; set; }
    public List<GameList> GameLists { get; set; }
}
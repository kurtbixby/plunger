namespace Plunger.Data.DbModels;

public class GameListEntry
{
    public int Id { get; set; }
    public int Number { get; set; }


    public int GameId { get; set; }
    public Game Game { get; set; }
    public int GameListId { get; set; }
    public GameList GameList { get; set; }
}
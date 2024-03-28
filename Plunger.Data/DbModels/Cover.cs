namespace Plunger.Data.DbModels;

public class Cover
{
    public int Id { get; set; }
    public int GameId { get; set; }
    public string ImageId { get; set; }
    public string Url { get; set; }
    public int Height { get; set; }
    public int Width { get; set; }
    public Guid Checksum { get; set; }
    
    public Game Game { get; set; }
};
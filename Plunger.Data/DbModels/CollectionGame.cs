using Plunger.Data.Enums;

namespace Plunger.Data.DbModels;

public class CollectionGame
{
    public int Id { get; set; }
    public DateTimeOffset TimeAcquired { get; set; }
    public Physicality Physicality { get; set; }

    public int CollectionId { get; set; }
    public Collection Collection { get; set; }
    public int GameId { get; set; }
    public Game Game { get; set; }
    public int PlatformId { get; set; }
    public Platform Platform { get; set; }
    public int RegionId { get; set; }
    public RegionName Region { get; set; }
}

public enum Physicality
{
    Unspecified = 0,
    Physical = 1,
    Digital = 2
}
using Plunger.Data.Enums;

namespace Plunger.Data.DbModels;

public class CollectionGame
{
    public int Id { get; set; }
    public DateTimeOffset TimeAdded { get; set; }
    public DateTimeOffset? TimeAcquired { get; set; }
    public ulong? PurchasePrice { get; set; }
    public Physicality Physicality { get; set; }
    
    public int CollectionId { get; set; }
    public int GameId { get; set; }
    public int PlatformId { get; set; }
    public Enums.Region Region { get; set; }
    public Guid VersionId { get; set; }
    
    public int RegionId { get; set; }
    public Game Game { get; set; }
    public Collection Collection { get; set; }
    public Platform Platform { get; set; }
}

public enum Physicality
{
    Unspecified = 0,
    Physical = 1,
    Digital = 2
}
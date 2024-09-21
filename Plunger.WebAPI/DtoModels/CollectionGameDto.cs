using Plunger.Data.Enums;

namespace Plunger.WebApi.DtoModels;

public record CollectionGameDto()
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public DateTimeOffset TimeAdded { get; set; }
    public DateTimeOffset? TimeAcquired { get; set; }
    public ulong? PurchasePrice { get; set; }
    public Physicality Physicality { get; set; }
    public PlatformDto Platform { get; set; }
    public RegionDto Region { get; set; }
}
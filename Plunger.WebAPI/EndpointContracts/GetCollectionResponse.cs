using Plunger.Common.Enums;
using Plunger.WebApi.DtoModels;

namespace Plunger.WebApi.EndpointContracts;

public record GetCollectionResponse()
{
    public IEnumerable<CollectionResponseCollectionGame> Games { get; set; }
}

public record CollectionResponseCollectionGame
{
    public int Id { get; set; }
    public DateTimeOffset TimeAdded { get; set; }
    public DateTimeOffset? TimeAcquired { get; set; }
    public ulong? PurchasePrice { get; set; }
    public Physicality Physicality { get; set; }
    public RegionDto Region { get; set; }
    public PlatformDto Platform { get; set; }
    public GameDto Game { get; set; }
    public GameStatusDto? Status { get; set; }
    public Guid VersionId { get; set; }
}

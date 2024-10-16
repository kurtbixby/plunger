using Plunger.Common.Enums;

namespace Plunger.WebApi.DtoModels;

public record CollectionEntryEdits()
{
    public int Id { get; init; }
    public int Platform { get; init; }
    public Region Region { get; init; }
    public Physicality Physicality { get; init; }
    public int PurchasePrice { get; init; }
    public DateTimeOffset TimeAcquired { get; init; }
    public Guid VersionId { get; init; }
}
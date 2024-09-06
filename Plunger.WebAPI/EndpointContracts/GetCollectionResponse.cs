using Plunger.Data.Enums;

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
    public CollectionResponseRegion Region { get; set; }
    public CollectionResponsePlatform Platform { get; set; }
    public CollectionResponseGame Game { get; set; }
}

public record CollectionResponseGame
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string ShortName { get; set; }
    public string? CoverUrl { get; set; }
}

public record CollectionResponsePlatform
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string? AltName { get; set; }
    public string Abbreviation { get; set; }
}

public record CollectionResponseRegion
{
    public int Id { get; set; }
    public string Name { get; set; }
}


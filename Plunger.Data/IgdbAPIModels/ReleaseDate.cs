using System.Text.Json.Serialization;
using Plunger.Data.Enums;

namespace Plunger.Data.IgdbAPIModels;

public record ReleaseDate
{
    public int Id { get; init; }
    
    [JsonConverter(typeof(Utils.DateTimeOffsetUnixJsonConverter))]
    public DateTimeOffset Date { get; init; }
    
    [JsonPropertyName("category")]
    public DateFormat DateFormat { get; init; }
    
    [JsonPropertyName("platform")]
    public int PlatformId { get; init; }
    
    [JsonPropertyName("region")]
    public RegionName RegionName { get; init; }
    
    [JsonPropertyName("status")]
    public int Status { get; init; }
    
    [JsonPropertyName("updated_at")]
    [JsonConverter(typeof(Utils.DateTimeOffsetUnixJsonConverter))]
    public DateTimeOffset UpdatedAt { get; init; }
    
    public Guid Checksum { get; init; }
}
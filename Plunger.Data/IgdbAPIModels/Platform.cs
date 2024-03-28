using System.Text.Json.Serialization;

namespace Plunger.Data.IgdbAPIModels;

public record Platform
{
    public int Id { get; init; }
    public string Name { get; init; }
    [JsonPropertyName("alternative_name")]
    public string? AlternativeName { get; init; }
    public string Slug { get; init; }
    public int? Generation { get; init; }
    public int? PlatformFamily { get; init; }
    [JsonPropertyName("updated_at")]
    [JsonConverter(typeof(Utils.DateTimeOffsetUnixJsonConverter))]
    public DateTimeOffset UpdatedAt { get; init; }
    public Guid Checksum { get; init; }
}
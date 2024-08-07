using System.Text.Json;
using System.Text.Json.Serialization;
using Plunger.Data.DbModels;

namespace Plunger.Data.IgdbAPIModels;

public record Game
{
    public int Id { get; init; }
    public string Name { get; init; }
    public List<int>? Platforms { get; init; }
    
    [JsonPropertyName("slug")]
    public string ShortName { get; init; }
    
    [JsonPropertyName("first_release_date")]
    [JsonConverter(typeof(Utils.DateTimeOffsetUnixJsonConverter))]
    public DateTimeOffset FirstReleased { get; init; }

    [JsonPropertyName("release_dates")]
    public List<ReleaseDate>? ReleaseDates { get; init; }
    
    [JsonPropertyName("age_ratings")]
    public List<AgeRating>? AgeRatings { get; init; }
    
    [JsonPropertyName("updated_at")]
    [JsonConverter(typeof(Utils.DateTimeOffsetUnixJsonConverter))]
    public DateTimeOffset UpdatedAt { get; init; }
    public Guid Checksum { get; init; }
}
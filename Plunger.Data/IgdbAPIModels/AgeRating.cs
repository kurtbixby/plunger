using System.Text.Json.Serialization;

namespace Plunger.Data.IgdbAPIModels;

public record AgeRating
{
    public int Id { get; init; }
    
    [JsonPropertyName("category")]
    public RatingBoard RatingCategory { get; init; }
}
using System.Text.Json.Serialization;

namespace Plunger.Data.IgdbAPIModels;

public record Cover
{
    public int Id { get; init; }
    public int Game { get; init; }
    [JsonPropertyName("image_id")]
    public string ImageId { get; init; }
    public int Height { get; init; }
    public int Width { get; init; }
    public Guid Checksum { get; init; }
};
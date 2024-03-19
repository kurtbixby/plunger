using System.Text.Json;
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
    [JsonConverter(typeof(DateTimeOffsetUnixJsonConverter))]
    public DateTimeOffset UpdatedAt { get; init; }
    public Guid Checksum { get; init; }
}

public class DateTimeOffsetUnixJsonConverter : JsonConverter<DateTimeOffset>
{
    public override DateTimeOffset Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        return DateTimeOffset.FromUnixTimeSeconds(reader.GetInt32());
    }

    public override void Write(Utf8JsonWriter writer, DateTimeOffset value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.ToUnixTimeSeconds().ToString());
    }
}
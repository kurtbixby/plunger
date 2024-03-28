using System.Text.Json;
using System.Text.Json.Serialization;

namespace Plunger.Data.Utils;

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
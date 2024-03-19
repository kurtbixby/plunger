using System.Text.Json;
using System.Text.Json.Serialization;

namespace Plunger.DbFiller;

public record TwitchOauthToken
{
    [JsonPropertyName("access_token")]
    public string AccessToken { get; init; }
    
    [JsonPropertyName("expires_in")]
    [JsonConverter(typeof(ExpiresInJsonConverter))]
    public DateTimeOffset ExpirationTime { get; init; }
    
    [JsonPropertyName("token_type")]
    public string TokenType { get; init; }
}

public class ExpiresInJsonConverter : JsonConverter<DateTimeOffset>
{
    public override DateTimeOffset Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {        
        return DateTimeOffset.Now + TimeSpan.FromSeconds(reader.GetInt32());
    }

    public override void Write(Utf8JsonWriter writer, DateTimeOffset offset, JsonSerializerOptions options)
    {
        writer.WriteStringValue(((int) (offset - DateTimeOffset.Now).TotalSeconds).ToString());
    }
}
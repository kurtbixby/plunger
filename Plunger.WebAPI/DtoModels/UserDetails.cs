using System.Text.Json.Serialization;

namespace Plunger.WebApi.DtoModels;

public record UserDetails()
{
    [JsonPropertyName("userId")]
    public string UserId { get; init; }
    [JsonPropertyName("userName")]
    public string UserName { get; init; }
}
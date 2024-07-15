using System.Text.Json.Serialization;

namespace Plunger.WebApi.DtoModels;

public record GameSearchRequest()
{
    [JsonPropertyName("gamename")] public string GameName { get; init; }
}
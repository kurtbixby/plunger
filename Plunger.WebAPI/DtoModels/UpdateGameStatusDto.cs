using System.Text.Json.Serialization;
using Plunger.Data.Enums;

namespace Plunger.WebApi.DtoModels;

public record UpdateGameStatusDto()
{
    [JsonPropertyName("timestamp")] public DateTimeOffset TimeStamp { get; init; }
    [JsonPropertyName("playstate")] public PlayState PlayState { get; init; }
};
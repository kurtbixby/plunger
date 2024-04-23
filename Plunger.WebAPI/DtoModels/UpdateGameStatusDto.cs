using System.Text.Json.Serialization;
using Plunger.Data.Enums;

namespace Plunger.WebApi.DtoModels;

public record UpdateGameStatusDto()
{
    [JsonPropertyName("timestamp")] public DateTimeOffset TimeStamp;
    [JsonPropertyName("playstate")] public PlayState PlayState;
};
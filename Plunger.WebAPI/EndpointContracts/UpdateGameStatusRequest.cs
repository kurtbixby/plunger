using System.Text.Json.Serialization;
using Plunger.Common.Enums;

namespace Plunger.WebApi.EndpointContracts;

public record UpdateGameStatusRequest()
{
    [JsonPropertyName("timestamp")] public DateTimeOffset TimeStamp { get; init; }
    [JsonPropertyName("playstate")] public PlayState PlayState { get; init; }
    [JsonPropertyName("timeplayed")] public TimeSpan TimePlayed { get; init; }
};
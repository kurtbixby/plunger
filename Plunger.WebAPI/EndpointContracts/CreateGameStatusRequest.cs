using System.Text.Json.Serialization;
using Plunger.Data.Enums;

namespace Plunger.WebApi.EndpointContracts;

public record CreateGameStatusRequest()
{
    [JsonPropertyName("gameid")] public int GameId { get; init; }
    [JsonPropertyName("timestamp")] public DateTimeOffset TimeStamp  { get; init; }
    [JsonPropertyName("completed")] public bool? Completed  { get; init; }
    [JsonPropertyName("playstate")] public PlayState? PlayState  { get; init; }
    [JsonPropertyName("timeplayed")] public TimeSpan? TimePlayed  { get; init; }
};
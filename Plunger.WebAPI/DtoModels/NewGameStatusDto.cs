using System.Text.Json.Serialization;
using Plunger.Data.Enums;

namespace Plunger.WebApi.DtoModels;

public record NewGameStatusDto()
{
    [JsonPropertyName("gameid")] public int GameId { get; init; }
    [JsonPropertyName("timestamp")] public DateTimeOffset TimeStamp  { get; init; }
    [JsonPropertyName("completed")] public bool? Completed  { get; init; }
    [JsonPropertyName("playstate")] public PlayState? PlayState  { get; init; }
};
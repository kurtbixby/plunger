using System.Text.Json.Serialization;
using Plunger.Data.Enums;

namespace Plunger.WebApi.DtoModels;

public record NewGameStatusDto()
{
    [JsonPropertyName("gameid")] public int GameId;
    [JsonPropertyName("timestamp")] public DateTimeOffset TimeStamp;
    [JsonPropertyName("completed")] public bool? Completed;
    [JsonPropertyName("playstate")] public PlayState? PlayState;
};
using System.Text.Json.Serialization;
using Plunger.Common.Enums;

namespace Plunger.WebApi.EndpointContracts;

public record UpdateGameStatusRequest()
{
    [JsonPropertyName("playstate")] public PlayState PlayState { get; init; }
    [JsonPropertyName("timeplayed")] public TimeSpan TimePlayed { get; init; }
    [JsonPropertyName("versionid")] public Guid VersionId { get; init; }
};
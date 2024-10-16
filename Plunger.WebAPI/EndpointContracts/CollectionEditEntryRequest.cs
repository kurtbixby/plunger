using System.Text.Json.Serialization;
using Plunger.WebApi.DtoModels;

namespace Plunger.WebApi.EndpointContracts;

public record CollectionEditEntryRequest()
{
    [JsonPropertyName("collectionGameEdits")] public CollectionEntryEdits EntryEdits { get; set; }
    [JsonPropertyName("gameStatusEdits")] public GameStatusEdits? StatusEdits { get; set; }
}

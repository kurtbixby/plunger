using System.Text.Json.Serialization;

namespace Plunger.WebApi.DtoModels;

public record NewGameDto()
{
    [JsonPropertyName("id")] int Id;
};
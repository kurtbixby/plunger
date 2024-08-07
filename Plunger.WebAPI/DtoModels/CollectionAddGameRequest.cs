using System.Text.Json.Serialization;
using Plunger.Data.DbModels;
using Region = Plunger.Data.Enums.Region;

namespace Plunger.WebApi.DtoModels;

public record CollectionAddGameRequest()
{
    [JsonPropertyName("gameid")] public int GameId { get; set; }
    [JsonPropertyName("platformid")] public int PlatformId { get; set; }
    [JsonPropertyName("regionid")] public Region Region { get; set; }
    [JsonPropertyName("timeacquired")] public DateTimeOffset? TimeAcquired { get; set; }
    [JsonPropertyName("physicality")] public Physicality Physicality { get; set; }

    public ValidationResult Validate()
    {
        var result = new ValidationResult() { IsValid = true, ValidationErrors = new Dictionary<string, string>() };
        
        return result;
    }
}
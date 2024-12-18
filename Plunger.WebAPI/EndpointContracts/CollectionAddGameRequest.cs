using System.Text.Json.Serialization;
using Plunger.Common.Enums;
using Region = Plunger.Common.Enums.Region;

namespace Plunger.WebApi.EndpointContracts;

public record CollectionAddGameRequest()
{
    [JsonPropertyName("gameid")] public int GameId { get; set; }
    [JsonPropertyName("platformid")] public int PlatformId { get; set; }
    
    [JsonPropertyName("regionid")]
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public Region Region { get; set; }
    [JsonPropertyName("timeacquired")] public DateTimeOffset? TimeAcquired { get; set; }
    [JsonPropertyName("purchaseprice")] public ulong? PurchasePrice { get; set; }
    
    [JsonPropertyName("physicality")]
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public Physicality Physicality { get; set; }

    public ValidationResult Validate()
    {
        var result = new ValidationResult() { IsValid = true, ValidationErrors = new Dictionary<string, string>() };
        
        return result;
    }
}
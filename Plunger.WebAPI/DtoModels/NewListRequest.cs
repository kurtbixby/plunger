using System.Text.Json.Serialization;
namespace Plunger.WebApi.DtoModels;

public class NewListRequest
{
    public class GameEntry
    {
        [JsonPropertyName("gameid")] public int GameId { get; set; }
        [JsonPropertyName("number")] public int Number { get; set; }
    }
    
    [JsonPropertyName("name")]
    public string Name { get; set; }
    [JsonPropertyName("unordered")]
    public bool Unordered { get; set; }
    [JsonPropertyName("entries")]
    public List<GameEntry> Entries { get; set; }

    public ValidationResult Validate()
    {
        var result = new ValidationResult();
        result.IsValid = true;
        if (String.IsNullOrWhiteSpace(Name))
        {
            // invalid username
            result.IsValid = false;
            result.ValidationErrors["name"] = "name is required";
        }

        return result;
    }
}
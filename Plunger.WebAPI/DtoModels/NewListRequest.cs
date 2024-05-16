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

        var entryNumbers = new HashSet<int>();
        var gameIds = new HashSet<int>();
        foreach (var gameEntry in Entries)
        {
            var validEntry = true;
            {
                var num = gameEntry.Number;
                if (!entryNumbers.Add(num))
                {
                    validEntry = false;
                    result.ValidationErrors["entries"] = "duplicate numbers in list";
                }
            }
            {
                var id = gameEntry.GameId;
                if (!gameIds.Add(id))
                {
                    validEntry = false;
                    result.ValidationErrors["entries"] = "duplicate games in list";
                }
            }

            if (!validEntry)
            {
                result.IsValid = false;
                break;
            }
        }

        return result;
    }
}
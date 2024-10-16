using System.Text.Json.Serialization;

namespace Plunger.Data.DbModels;

public class Collection
{
    public int Id { get; set; }

    public int UserId { get; set; }
    [JsonIgnore]
    public User User { get; set; }
    public List<CollectionGame> Games { get; set; }
}
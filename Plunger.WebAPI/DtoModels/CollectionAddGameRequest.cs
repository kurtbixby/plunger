using Plunger.Data.DbModels;
using Plunger.Data.Enums;

namespace Plunger.WebApi.DtoModels;

public record CollectionAddGameRequest()
{
    public int GameId { get; set; }
    
    public int PlatformId { get; set; }
    public int RegionId { get; set; }
    public RegionName Region { get; set; }
    
    public DateTimeOffset? TimeAcquired { get; set; }
    public Physicality Physicality { get; set; }

    public ValidationResult Validate()
    {
        var result = new ValidationResult() { IsValid = true, ValidationErrors = new Dictionary<string, string>() };
        
        return result;
    }
}
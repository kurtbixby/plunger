using Plunger.Data.Enums;

namespace Plunger.WebApi.EndpointContracts;

public record CollectionGamePatchRequest()
{
    public DateTimeOffset? TimeAcquired { get; init; }
    public Physicality? Physicality { get; init; }
    public int? GameId { get; init; }
    public int? PlatformId { get; init; }
    public int? RegionId { get; init; }
    public Guid VersionId { get; init; }

    public ValidationResult Validate()
    {
        var result = new ValidationResult() { IsValid = true, ValidationErrors = new Dictionary<string, string>() }; 
        if (TimeAcquired == null && Physicality == null && GameId == null && PlatformId == null && RegionId == null)
        {
            result.IsValid = false;
            result.ValidationErrors["Error"] = "No field given";
        }

        return result;
    }
}
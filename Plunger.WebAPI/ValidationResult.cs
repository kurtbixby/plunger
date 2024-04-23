namespace Plunger.WebApi;

public record ValidationResult
{
    public bool IsValid { get; set; }
    public Dictionary<string, string> ValidationErrors { get; set; }
}
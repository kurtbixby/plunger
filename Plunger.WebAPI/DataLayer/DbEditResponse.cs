namespace Plunger.WebApi.DataLayer;

public record DbEditResponse()
{
    public bool Success { get; set; }
    public string Message { get; set; }
}
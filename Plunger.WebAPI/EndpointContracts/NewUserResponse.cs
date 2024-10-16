namespace Plunger.WebApi.EndpointContracts;

public record NewUserResponse()
{
    public Dictionary<string, string> Messages { get; set; }
}
namespace Plunger.WebApi.EndpointContracts;

public record LoginResponse()
{
    public bool Login { get; init; }
}
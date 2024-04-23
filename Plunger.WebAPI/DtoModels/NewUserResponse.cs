namespace Plunger.WebApi.DtoModels;

public record NewUserResponse()
{
    public Dictionary<string, string> Messages { get; set; }
}
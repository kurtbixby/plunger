using System.Text.Json.Serialization;

namespace Plunger.WebApi.DtoModels;

public record NewUserDto()
{
    [JsonPropertyName("username")] string Username;
    [JsonPropertyName("password")] string Password;
};
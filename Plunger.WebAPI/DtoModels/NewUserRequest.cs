using System.Text.Json.Serialization;

namespace Plunger.WebApi.DtoModels;

public record NewUserRequest()
{
    [JsonPropertyName("username")] public string Username { get; init; }
    [JsonPropertyName("email")] public string Email { get; init; }
    [JsonPropertyName("password")] public string Password { get; init; }

    #warning TODO: Incomplete validation logic
    public ValidationResult Validate()
    {
        var result = new ValidationResult();
        result.IsValid = true;
        if (Username.Length < 5 || Username.Length > 20)
        {
            // invalid username
            result.IsValid = false;
            result.ValidationErrors["username"] = "username length must be between 5 and 20 characters long";
        }

        if (Password.Length < 8 || Password.Length > 16)
        {
            result.IsValid = false;
            result.ValidationErrors["password"] = "password length must be between 8 and 16 characters";
        }

        return result;
    }
};
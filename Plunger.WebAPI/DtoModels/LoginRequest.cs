using System.Text.Json.Serialization;

namespace Plunger.WebApi.DtoModels;

public record LoginRequest()
{
    [JsonPropertyName("identity")] public string Identity { get; init; }
    [JsonPropertyName("password")] public string Password { get; init; }

    public ValidationResult Validate()
    {
        var result = new ValidationResult();
        result.IsValid = true;
        if (String.IsNullOrWhiteSpace(Identity))
        {
            // invalid username
            result.IsValid = false;
            result.ValidationErrors["identity"] = "identity is required";
        }

        if (String.IsNullOrWhiteSpace(Password))
        {
            // invalid username
            result.IsValid = false;
            result.ValidationErrors["password"] = "password is required";
        }

        return result;
    }
}
using System.Net.Http.Json;
using System.Text.Json;
using Plunger.Common;

namespace Plunger.DbFiller;

public class TwitchOauthConnector(string clientId, string clientSecret)
{
    private const string GrantType = "client_credentials";
    private readonly HttpClient _httpClient = new() { BaseAddress = new Uri("https://id.twitch.tv/")};

    public async Task<TwitchOauthToken> GetToken()
    {
        // builduri with query string
        var endPoint = "oauth2/token" + BuildQueryString();
        var response = await _httpClient.PostAsync(endPoint, null);

        response.EnsureSuccessStatusCode();
        
        var token = await response.Content.ReadFromJsonAsync<TwitchOauthToken>();
        if (token == null)
        {
            throw new Exception("Null response getting Token from Twitch");
        }
        return token;
    }
    
    private T Deserialize<T>(string json)
    {
        return JsonSerializer.Deserialize<T>(json);
    }

    private string BuildQueryString()
    {
        var dict = new Dictionary<string, string>()
        {
            { "client_id", clientId },
            { "client_secret", clientSecret },
            { "grant_type", GrantType }
        };

        return Utils.BuildQueryUrl(dict);
    }
}
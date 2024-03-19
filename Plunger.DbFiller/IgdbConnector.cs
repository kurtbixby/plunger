using System.Net.Http.Headers;
using System.Net.Http.Json;
using Plunger.Data.IgdbAPIModels;

namespace Plunger.DbFiller;

public class IgdbConnector
{
    private readonly HttpClient _httpClient;

    public IgdbConnector(TwitchCredentials credentials)
    {
        _httpClient = new HttpClient() { BaseAddress = new Uri("https://api.igdb.com/v4/") };
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", credentials.OauthToken.AccessToken);
        _httpClient.DefaultRequestHeaders.Add("Client-ID", credentials.ClientId);
    }

    public async Task<List<Platform>> GetPlatforms(DateTimeOffset cutoffTime)
    {
        return await CallApi<List<Platform>>("platforms", "fields *; limit 100;");
    }

    public async Task<List<PlatformFamily>> GetPlatformFamilies()
    {
        return await CallApi<List<PlatformFamily>>("platform_families", "fields *; limit 50;");
    }

    private async Task<T> CallApi<T>(string endpoint, string bodyString)
    {
        using StringContent body = new(bodyString);

        using var response = await _httpClient.PostAsync(endpoint, body);
        response.EnsureSuccessStatusCode();

        var responseObject = await response.Content.ReadFromJsonAsync<T>();
        return responseObject;
    }
}
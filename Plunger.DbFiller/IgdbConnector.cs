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

    public async Task<List<Platform>> GetPlatforms()
    {
        return await CallApi<List<Platform>>("platforms", "fields *; limit 300;");
    }

    public async Task<List<PlatformFamily>> GetPlatformFamilies()
    {
        return await CallApi<List<PlatformFamily>>("platform_families", "fields *; limit 50;");
    }

    public async Task<List<Game>> GetGames(string queryString)
    {
        var fieldsString = "fields id, name, platforms, slug, first_release_date, release_dates.*, updated_at, checksum;";
        var platformsString = "where platforms = (23,99,51,33,24,22,307,137,37,4,416,20,159,18,21,131,130,7,8,9,48,167,38,46,165,390,441,166,306,30,78,35,64,29,339,32,84,58,19,87,47,440,5,41,11,12,49,169);";
        var amountString = "limit 500;";
        var fullQuery = string.Join(" ", new [] {fieldsString, platformsString, amountString, queryString});
        Console.WriteLine($"Full query: {fullQuery}");
        return await CallApi<List<Game>>("games", fullQuery);
        // return await CallApi<List<Game>>("games", "fields *, release_dates.*; limit 500;" + queryString);
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
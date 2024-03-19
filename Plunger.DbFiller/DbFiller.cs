namespace Plunger.DbFiller;

public class DbFiller
{
    static async Task Main(string[] args)
    {
        // Establish token
        var twitchConnector = new TwitchOauthConnector(Utils.GetEnvironmentVariable("ClientId"),
            Utils.GetEnvironmentVariable("ClientSecret"));
        var credentials = new TwitchCredentials(Utils.GetEnvironmentVariable("ClientId"), await twitchConnector.GetToken());
        var igdbConnector = new IgdbConnector(credentials);
        // Load Platforms
        var now = DateTimeOffset.Now;
        var platforms = await igdbConnector.GetPlatforms(now);
        platforms.ForEach(delegate(Data.IgdbAPIModels.Platform platform)
        {
            Console.WriteLine(platform);
        });
        // Load Regions
        // Load Games
    }
}
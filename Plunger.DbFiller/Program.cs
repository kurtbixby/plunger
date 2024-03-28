using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Plunger.Common;
using Plunger.Data;
using Plunger.Data.Utils;

namespace Plunger.DbFiller;

public class Program
{
    public static IConfigurationRoot Configuration;
    static async Task Main(string[] args)
    {
        var environmentName = Utils.GetEnvironmentVariable("Environment");
        Configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json", optional: false, reloadOnChange: true).AddJsonFile($"appsettings.{environmentName}.json", optional: true, reloadOnChange: true)
            .Build();
        // Establish token
        var twitchConnector = new TwitchOauthConnector(Utils.GetEnvironmentVariable("ClientId"),
            Utils.GetEnvironmentVariable("ClientSecret"));
        var credentials = new TwitchCredentials(Utils.GetEnvironmentVariable("ClientId"), await twitchConnector.GetToken());
        var igdbConnector = new IgdbConnector(credentials);
        
        // Create Db connection
        var connString = Configuration["ConnectionStrings:DefaultConnection"];
        var contextOptions = new DbContextOptionsBuilder<PlungerDbContext>().UseNpgsql(connString).Options;

        // LoadPlatforms(igdbConnector, contextOptions).Wait();
        // Load Regions
        // Load Games
        // LoadGames(igdbConnector, contextOptions, new DateTimeOffset(2024, 1, 1, 0, 0, 0, TimeSpan.Zero), 1000).Wait();
        LoadCovers(igdbConnector, contextOptions, new DateTimeOffset(2024, 1, 1, 0, 0, 0, TimeSpan.Zero), 0).Wait();
    }

    private static async Task LoadPlatforms(IgdbConnector igdbConnector, DbContextOptions<PlungerDbContext> dbContextOptions)
    {
        await using var db = new PlungerDbContext(dbContextOptions);
        
        // Load Platforms
        var platforms = await igdbConnector.GetPlatforms();
        db.Platforms.AddRange(platforms.Select(platform => platform.ToDbModel()));
        var res = await db.SaveChangesAsync();
        Console.WriteLine($"{res} games inserted");
    }

    private static async Task LoadGames(IgdbConnector igdbConnector, DbContextOptions<PlungerDbContext> dbContextOptions, DateTimeOffset cutoffTime, int offset = 0)
    {
        var gamesRetrieved = 0;
        do
        {
            await using var db = new PlungerDbContext(dbContextOptions);

            var unixTimeCutoff = cutoffTime.ToUnixTimeSeconds();
            // Load Games
            Console.WriteLine($"Querying at offset {offset}");
            var games = await igdbConnector.GetGames($"where updated_at < {unixTimeCutoff}; offset {offset}; sort id asc;");
            gamesRetrieved = games.Count;
            Console.WriteLine($"{gamesRetrieved} game retrieved at offset {offset}");
            db.Games.AddRange(games.Select(game => game.ToDbModel(db)));
            var res = await db.SaveChangesAsync();
            Console.WriteLine($"{res} rows inserted");
            offset += gamesRetrieved;
            Console.WriteLine($"New offset is {offset}");
        } while (gamesRetrieved >= 500);
    }
    
    private static async Task LoadCovers(IgdbConnector igdbConnector, DbContextOptions<PlungerDbContext> dbContextOptions, DateTimeOffset cutoffTime, int offset = 0)
    {
        var retrieved = 0;
        do
        {
            await using var db = new PlungerDbContext(dbContextOptions);

            var unixTimeCutoff = cutoffTime.ToUnixTimeSeconds();
            // Load Games
            Console.WriteLine($"Querying at offset {offset}");
            var apiObjects = await igdbConnector.GetCovers($"where game.updated_at < {unixTimeCutoff}; offset {offset}; sort game.id asc;");
            retrieved = apiObjects.Count;
            Console.WriteLine($"{retrieved} covers retrieved at offset {offset}");
            db.Covers.AddRange(apiObjects.Select(e => e.ToDbModel(db)));
            var res = await db.SaveChangesAsync();
            Console.WriteLine($"{res} rows inserted");
            offset += retrieved;
            Console.WriteLine($"New offset is {offset}");
        } while (retrieved >= 500);
    }

    private static async Task LoadRegions()
    {
        
    }
}
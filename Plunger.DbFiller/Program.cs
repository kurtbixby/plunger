using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Plunger.Common;
using Plunger.Common.Enums;
using Plunger.Data;
using Plunger.Data.Utils;

namespace Plunger.DbFiller;

public class Program
{
    private static IConfigurationRoot Configuration;
    static async Task Main(string[] args)
    {
        var environmentName = Utils.GetEnvironmentVariable("Environment");
        Configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json", optional: false, reloadOnChange: true).AddJsonFile($"appsettings.{environmentName}.json", optional: true, reloadOnChange: true)
            .Build();
        
        // Create Db connection
        var connString = Configuration["ConnectionStrings:DefaultConnection"];
        var contextOptions = new DbContextOptionsBuilder<PlungerDbContext>().UseNpgsql(connString).Options;

        var connector = await CreateIgdbConnector(contextOptions);
        // LoadRegions(contextOptions).Wait();
        // LoadPlatforms(connector, contextOptions).Wait();
        //
        var cutoffTime = new DateTimeOffset(2024, 10, 25, 18, 0, 0, TimeSpan.Zero);
        
        LoadGamesFresh(connector, contextOptions, cutoffTime);
        
        // LoadGames(connector, contextOptions, cutoffTime, 0).Wait();
        // BackfillCovers(connector, contextOptions, cutoffTime);
    }

    private static async Task<IgdbConnector> CreateIgdbConnector(DbContextOptions<PlungerDbContext> contextOptions)
    {
        // Establish token
        var twitchConnector = new TwitchOauthConnector(Utils.GetEnvironmentVariable("ClientId"),
            Utils.GetEnvironmentVariable("ClientSecret"));
        var credentials = new TwitchCredentials(Utils.GetEnvironmentVariable("ClientId"), await twitchConnector.GetToken());
        var igdbConnector = new IgdbConnector(credentials);

        return igdbConnector;
    }

    private static async Task LoadPlatforms(IgdbConnector igdbConnector, DbContextOptions<PlungerDbContext> dbContextOptions)
    {
        await using var db = new PlungerDbContext(dbContextOptions);
        
        // Load Platforms
        var platforms = await igdbConnector.GetPlatforms();
        db.Platforms.AddRange(platforms.Select(platform => platform.ToDbModel()));
        var res = await db.SaveChangesAsync();
        Console.WriteLine($"{res} platforms inserted");
    }
    
    private static void LoadGamesFresh(IgdbConnector igdbConnector, DbContextOptions<PlungerDbContext> dbContextOptions, DateTimeOffset cutoffTime, int offset = 0)
    {
        const int batchSize = 500;
        
        var totalRetrieved = 0;
        var gamesRetrieved = 0;
        using var db = new PlungerDbContext(dbContextOptions);
        
        do
        {
            var unixTimeCutoff = cutoffTime.ToUnixTimeSeconds();
            // Load Games
            Console.WriteLine($"Querying at offset {offset}");

            var queryOptions = new QueryOptions();
            queryOptions.AddWhereOption("updated_at", $"< {unixTimeCutoff}");
            queryOptions.AddWhereOption("version_parent", "= null"); // Excludes editions
            queryOptions.AddWhereOption("category", "!= (5, 12, 13, 14)"); // Excludes mods, forks, packs, updates
            queryOptions.AddOtherOption("offset", $"{offset}");
            queryOptions.AddOtherOption("sort", "id asc");

            var games = igdbConnector.GetGamesWithCovers(queryOptions, batchSize).Result;
            gamesRetrieved = games.Count;
            totalRetrieved += gamesRetrieved;
            Console.WriteLine($"{gamesRetrieved} game retrieved at offset {offset}");
            db.Games.AddRange(games.Select(game => game.ToDbModel(db)));
            var res = db.SaveChanges();
            Console.WriteLine($"{res} rows inserted");
            offset += gamesRetrieved;
            Console.WriteLine($"New offset is {offset}");
        } while (gamesRetrieved >= batchSize);
    }

    private static async Task LoadGames(IgdbConnector igdbConnector, DbContextOptions<PlungerDbContext> dbContextOptions, DateTimeOffset cutoffTime, int offset = 0)
    {
        var totalRetrieved = 0;
        var gamesRetrieved = 0;
        await using var db = new PlungerDbContext(dbContextOptions);
        
        do
        {
            var unixTimeCutoff = cutoffTime.ToUnixTimeSeconds();
            // Load Games
            Console.WriteLine($"Querying at offset {offset}");

            var queryOptions = new QueryOptions();
            queryOptions.AddWhereOption("updated_at", $"< {unixTimeCutoff}");
            queryOptions.AddWhereOption("version_parent", "= null"); // Excludes editions
            queryOptions.AddWhereOption("category", "!= (5, 12, 13, 14)"); // Excludes mods, forks, packs, updates
            queryOptions.AddOtherOption("offset", $"{offset}");
            queryOptions.AddOtherOption("sort", "id asc");

            var games = await igdbConnector.GetGames(queryOptions);
            gamesRetrieved = games.Count;
            totalRetrieved += gamesRetrieved;
            Console.WriteLine($"{gamesRetrieved} game retrieved at offset {offset}");
            db.Games.AddRange(games.Select(game => game.ToDbModel(db)));
            var res = await db.SaveChangesAsync();
            Console.WriteLine($"{res} rows inserted");
            offset += gamesRetrieved;
            Console.WriteLine($"New offset is {offset}");
        } while (gamesRetrieved >= 500 && totalRetrieved <= 1500);
    }
    
    private static void BackfillCovers(IgdbConnector igdbConnector, DbContextOptions<PlungerDbContext> dbContextOptions, DateTimeOffset cutoffTime, int offset = 0)
    {
        var retrieved = 0;
        using var db = new PlungerDbContext(dbContextOptions);
        do
        {

            var unixTimeCutoff = cutoffTime.ToUnixTimeSeconds();
            // Load Games
            Console.WriteLine($"Querying at offset {offset}");
            // var apiObjects = await igdbConnector.GetCovers($"where game.updated_at < {unixTimeCutoff}; offset {offset}; sort game.id asc;");
            
            var queryOptions = new QueryOptions();
            queryOptions.AddWhereOption("game.updated_at", $"< {unixTimeCutoff}");
            queryOptions.AddWhereOption("game.version_parent", "= null"); // Excludes editions
            queryOptions.AddWhereOption("game.category", "!= (5, 12, 13, 14)"); // Excludes mods, forks, packs, updates
            queryOptions.AddOtherOption("offset", $"{offset}");
            queryOptions.AddOtherOption("sort", "game.id asc");

            var apiObjects = igdbConnector.GetCovers(queryOptions).Result;
            retrieved = apiObjects.Count;
            Console.WriteLine($"{retrieved} covers retrieved at offset {offset}");
            db.Covers.AddRange(apiObjects.Select(e =>
            {
                return e.ToDbModel();
            }));
            var res = db.SaveChanges();
            Console.WriteLine($"{res} rows inserted");
            offset += retrieved;
            Console.WriteLine($"New offset is {offset}");
        } while (retrieved >= 500);
    }

    private static async Task LoadRegions(DbContextOptions<PlungerDbContext> dbContextOptions)
    {
        await using var db = new PlungerDbContext(dbContextOptions);
        var regions = EnumStrings.RegionNames.Slice(1, EnumStrings.RegionNames.Count - 1)
            .Select((e, index) => new Data.DbModels.Region() { Id = index + 1, Name = e });
        db.Regions.AddRange(regions);
        await db.SaveChangesAsync();
    }
}
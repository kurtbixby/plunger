using Microsoft.EntityFrameworkCore;
using Plunger.Data.DbModels;

namespace Plunger.Data;

public class PlungerDb : DbContext
{
    public PlungerDb(DbContextOptions<PlungerDb> options) : base(options) { }
    
    public DbSet<User> Users { get; set; }
    public DbSet<GameList> GameLists { get; set; }
    public DbSet<GameListEntry> GameListEntries { get; set; }
    public DbSet<GameStatus> GameStatuses { get; set; }
    public DbSet<PlayStateChange> PlayStateChanges { get; set; }
    public DbSet<Collection> Collections { get; set; }
    public DbSet<CollectionGame> CollectionGames { get; set; }
    public DbSet<Game> Games { get; set; }
    public DbSet<Platform> Platforms { get; set; }
    public DbSet<Region> Regions { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseNpgsql("Host=localhost;Database=plunger-db-local;Username=localuser;Password=localuserpassword");
}
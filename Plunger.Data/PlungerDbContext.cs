using Microsoft.EntityFrameworkCore;
using Plunger.Data.DbModels;

namespace Plunger.Data;

public class PlungerDbContext : DbContext
{
    public PlungerDbContext(DbContextOptions<PlungerDbContext> options) : base(options)
    {
    }
    
    public DbSet<User> Users { get; set; }
    public DbSet<GameList> GameLists { get; set; }
    public DbSet<GameListEntry> GameListEntries { get; set; }
    public DbSet<GameStatus> GameStatuses { get; set; }
    public DbSet<PlayStateChange> PlayStateChanges { get; set; }
    public DbSet<Collection> Collections { get; set; }
    public DbSet<CollectionGame> CollectionGames { get; set; }
    public DbSet<Game> Games { get; set; }
    public DbSet<Cover> Covers { get; set; }
    public DbSet<Platform> Platforms { get; set; }
    public DbSet<Region> Regions { get; set; }

    // protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    //     => optionsBuilder.UseNpgsql(_connectionString);
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Game>()
            .HasMany(e => e.Platforms)
            .WithMany();
    }
}
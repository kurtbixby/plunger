using Microsoft.EntityFrameworkCore;

namespace Plunger.Data.DbModels;

public class Game
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string ShortName { get; set; }
    public DateTimeOffset FirstReleased { get; set; }
    public List<Platform> Platforms { get; set; }
    public List<ReleaseDate> ReleaseDates { get; set; }
    public Cover? Cover { get; set; }
    public List<Region> Regions { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }
    public Guid Checksum { get; set; }
}
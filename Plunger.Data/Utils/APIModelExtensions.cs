using Microsoft.EntityFrameworkCore;
using Plunger.Data.DbModels;

namespace Plunger.Data.Utils;

public static class GameExtensions
{
    public static Data.DbModels.Game ToDbModel(this Data.IgdbAPIModels.Game game, PlungerDbContext dbContext)
    {
        try
        {
            var releaseDates = (game.ReleaseDates != null) ? game.ReleaseDates.Select(apiDate => apiDate.ToDbModel()) : new List<ReleaseDate>();
            var platforms = (game.Platforms != null) ? dbContext.Platforms.Where(e => game.Platforms.Contains(e.Id)) : new List<Platform>().AsQueryable();
            return new Data.DbModels.Game
            {
                Id = game.Id, Name = game.Name, ShortName = game.ShortName, FirstReleased = game.FirstReleased,
                Platforms = platforms.ToList(), ReleaseDates = releaseDates.ToList(), UpdatedAt = game.UpdatedAt,
                Checksum = game.Checksum
            };
        }
        catch (Exception e)
        {
            return null;
        }
    }
}

public static class DateFormatExtensions
{
    public static Enums.DateFormat ToCommonEnum(this Data.IgdbAPIModels.DateFormat format) =>
        Data.IgdbAPIModels.DateFormat.TryParse(format.ToString(), out Enums.DateFormat eFormat) ? eFormat : Enums.DateFormat.TBD;
}

public static class PlatformExtensions
{
    public static Data.DbModels.Platform ToDbModel(this Data.IgdbAPIModels.Platform platform) => new Data.DbModels.Platform {Abbreviation = platform.Slug, AltName = platform.AlternativeName, Checksum = platform.Checksum, Id = platform.Id, Name = platform.Name, Generation = platform.Generation, UpdatedAt = platform.UpdatedAt};
}

public static class RegionNameExtensions
{
    public static Enums.RegionName ToCommonEnum(this Data.IgdbAPIModels.RegionName region)
    {
        var isDefined = Enum.IsDefined(typeof(Enums.RegionName), (int)region);
        return isDefined ? (Enums.RegionName)region : Enums.RegionName.Unknown;
    }
}

public static class ReleaseDateExtensions
{
    public static Data.DbModels.ReleaseDate ToDbModel(this Data.IgdbAPIModels.ReleaseDate releaseDate) => new Data.DbModels.ReleaseDate {Id = releaseDate.Id, Date = releaseDate.Date, DateFormat = releaseDate.DateFormat.ToCommonEnum(), PlatformId = releaseDate.PlatformId, RegionName = releaseDate.RegionName.ToCommonEnum(), UpdatedAt = releaseDate.UpdatedAt, Checksum = releaseDate.Checksum};
}
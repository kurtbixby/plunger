using Platform = Plunger.Data.DbModels.Platform;
using Region = Plunger.Common.Enums.Region;
using ReleaseDate = Plunger.Data.DbModels.ReleaseDate;

namespace Plunger.Data.Utils;

public static class GameExtensions
{
    public static Data.DbModels.Game ToDbModel(this Data.IgdbAPIModels.Game game, PlungerDbContext dbContext)
    {
        try
        {
            // rd.Status == 5 is "Cancelled"
            var releaseDates = (game.ReleaseDates != null) ? game.ReleaseDates.Where(rd => rd.Status != 5).Select(apiDate => apiDate.ToDbModel()) : new List<ReleaseDate>();
            var platforms = (game.Platforms != null) ? dbContext.Platforms.Where(e => game.Platforms.Contains(e.Id)) : new List<Platform>().AsQueryable();
            var gameRatings = (game.AgeRatings != null)
                ? game.AgeRatings.Select(ar => (int)RegionUtils.RegionForRatingBoard(ar.RatingCategory))
                : new List<int>();
            var regions = dbContext.Regions.Where(r => gameRatings.Contains(r.Id));
            return new Data.DbModels.Game
            {
                Id = game.Id, Name = game.Name, ShortName = game.ShortName, FirstReleased = game.FirstReleased,
                Platforms = platforms.ToList(), ReleaseDates = releaseDates.ToList(), Regions = regions.ToList(), UpdatedAt = game.UpdatedAt,
                Checksum = game.Checksum
            };
        }
        catch (Exception e)
        {
            return null;
        }
    }
}

public static class CoverExtensions
{
    public static Data.DbModels.Cover ToDbModel(this Data.IgdbAPIModels.Cover cover, PlungerDbContext dbContext)
    {
        try
        {
            if (cover.ImageId == null)
            {
                throw new Exception();
            }
            return new Data.DbModels.Cover { Id = cover.Id, GameId = cover.Game, ImageId = cover.ImageId, Url = cover.Url, Height = cover.Height, Width = cover.Width, Checksum = cover.Checksum };
        }
        catch (Exception e)
        {
            return null;
        }
        
    }
}

public static class DateFormatExtensions
{
    public static Common.Enums.DateFormat ToCommonEnum(this Data.IgdbAPIModels.DateFormat format) =>
        Enum.TryParse(format.ToString(), out Common.Enums.DateFormat eFormat) ? eFormat : Common.Enums.DateFormat.TBD;
}

public static class PlatformExtensions
{
    public static Data.DbModels.Platform ToDbModel(this Data.IgdbAPIModels.Platform platform) => new Data.DbModels.Platform {Abbreviation = platform.Slug, AltName = platform.AlternativeName, Checksum = platform.Checksum, Id = platform.Id, Name = platform.Name, Generation = platform.Generation, UpdatedAt = platform.UpdatedAt};
}

public static class RegionNameExtensions
{
    public static Region ToCommonEnum(this Data.IgdbAPIModels.RegionName region)
    {
        var isDefined = Enum.IsDefined(typeof(Region), (int)region);
        return isDefined ? RegionUtils.RegionForRegionName(region) : Region.Unknown;
    }
}

public static class ReleaseDateExtensions
{
    public static Data.DbModels.ReleaseDate ToDbModel(this Data.IgdbAPIModels.ReleaseDate releaseDate) => new Data.DbModels.ReleaseDate {Id = releaseDate.Id, Date = releaseDate.Date, DateFormat = releaseDate.DateFormat.ToCommonEnum(), PlatformId = releaseDate.PlatformId, Region = releaseDate.RegionName.ToCommonEnum(), UpdatedAt = releaseDate.UpdatedAt, Checksum = releaseDate.Checksum};
}
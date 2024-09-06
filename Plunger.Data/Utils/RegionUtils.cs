using Plunger.Common.Enums;

namespace Plunger.Data.Utils;

public static class RegionUtils
{
    public static Region RegionForReleaseDate(IgdbAPIModels.ReleaseDate releaseDate)
    {
        return RegionForRegionName(releaseDate.RegionName);
    }

    public static Region RegionForRegionName(IgdbAPIModels.RegionName regionName)
    {
        return ExtRegionMapping[regionName];
    }
    
    public static Region RegionForRatingBoard(IgdbAPIModels.RatingBoard ratingBoard)
    {
        return ExtRatingBoardMapping[ratingBoard];
    }

    private static readonly Dictionary<IgdbAPIModels.RegionName, Region> ExtRegionMapping = new()
    {
        { IgdbAPIModels.RegionName.europe, Region.Europe },
        { IgdbAPIModels.RegionName.north_america, Region.NorthAmerica },
        { IgdbAPIModels.RegionName.australia, Region.Australia },
        { IgdbAPIModels.RegionName.new_zealand, Region.NewZealand },
        { IgdbAPIModels.RegionName.japan, Region.Japan },
        { IgdbAPIModels.RegionName.china, Region.China },
        { IgdbAPIModels.RegionName.asia, Region.Asia },
        { IgdbAPIModels.RegionName.worldwide, Region.Unknown },
        { IgdbAPIModels.RegionName.korea, Region.Korea },
        { IgdbAPIModels.RegionName.brazil, Region.Brazil }
    };
    
    private static readonly Dictionary<IgdbAPIModels.RatingBoard, Region> ExtRatingBoardMapping = new()
    {
        { IgdbAPIModels.RatingBoard.ESRB, Region.NorthAmerica },
        { IgdbAPIModels.RatingBoard.PEGI, Region.Europe },
        { IgdbAPIModels.RatingBoard.CERO, Region.Japan },
        { IgdbAPIModels.RatingBoard.USK, Region.Germany },
        { IgdbAPIModels.RatingBoard.GRAC, Region.Korea },
        { IgdbAPIModels.RatingBoard.CLASS_IND, Region.Brazil },
        { IgdbAPIModels.RatingBoard.ACB, Region.Australia },
    };
}

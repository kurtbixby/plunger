namespace Plunger.Data.Utils;

public static class RegionUtils
{
    public static Enums.Region RegionForReleaseDate(IgdbAPIModels.ReleaseDate releaseDate)
    {
        return RegionForRegionName(releaseDate.RegionName);
    }

    public static Enums.Region RegionForRegionName(IgdbAPIModels.RegionName regionName)
    {
        return ExtRegionMapping[regionName];
    }
    
    public static Enums.Region RegionForRatingBoard(IgdbAPIModels.RatingBoard ratingBoard)
    {
        return ExtRatingBoardMapping[ratingBoard];
    }

    private static readonly Dictionary<IgdbAPIModels.RegionName, Enums.Region> ExtRegionMapping = new()
    {
        { IgdbAPIModels.RegionName.europe, Enums.Region.Europe },
        { IgdbAPIModels.RegionName.north_america, Enums.Region.NorthAmerica },
        { IgdbAPIModels.RegionName.australia, Enums.Region.Australia },
        { IgdbAPIModels.RegionName.new_zealand, Enums.Region.NewZealand },
        { IgdbAPIModels.RegionName.japan, Enums.Region.Japan },
        { IgdbAPIModels.RegionName.china, Enums.Region.China },
        { IgdbAPIModels.RegionName.asia, Enums.Region.Asia },
        { IgdbAPIModels.RegionName.worldwide, Enums.Region.Unknown },
        { IgdbAPIModels.RegionName.korea, Enums.Region.Korea },
        { IgdbAPIModels.RegionName.brazil, Enums.Region.Brazil }
    };
    
    private static readonly Dictionary<IgdbAPIModels.RatingBoard, Enums.Region> ExtRatingBoardMapping = new()
    {
        { IgdbAPIModels.RatingBoard.ESRB, Enums.Region.NorthAmerica },
        { IgdbAPIModels.RatingBoard.PEGI, Enums.Region.Europe },
        { IgdbAPIModels.RatingBoard.CERO, Enums.Region.Japan },
        { IgdbAPIModels.RatingBoard.USK, Enums.Region.Germany },
        { IgdbAPIModels.RatingBoard.GRAC, Enums.Region.Korea },
        { IgdbAPIModels.RatingBoard.CLASS_IND, Enums.Region.Brazil },
        { IgdbAPIModels.RatingBoard.ACB, Enums.Region.Australia },
    };
}

namespace Plunger.Data.Enums;

public static class EnumStrings
{
    public static List<string> RegionNames = new List<string>()
    {
        "Unknown",
        "Europe",
        "North America",
        "Australia",
        "New Zealand",
        "Japan",
        "China",
        "Asia",
        "Global",
        "Korea",
        "Brazil"
    };
    
    public static List<string> PlayStates = new List<string>()
    {
        "Unplayed",
        "In Progress",
        "Completed",
        "Paused"
    };
    
    public static List<string> DateFormats = new List<string>()
    {
        "YYYYMMMMDD",
        "YYYYMMMM",
        "YYYY",
        "YYYYQ1",
        "YYYYQ2",
        "YYYYQ3",
        "YYYYQ4",
        "TBD"
    };
}
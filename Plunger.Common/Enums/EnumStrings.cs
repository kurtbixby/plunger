namespace Plunger.Data.Enums;

public static class EnumStrings
{
    public static List<string> RegionNames = new List<string>()
    {
        "Unknown",
        "North America",
        "Japan",
        "Europe",
        "Asia",
        "Australia",
        "Brazil",
        "China",
        "Germany",
        "Korea",
        "New Zealand",
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
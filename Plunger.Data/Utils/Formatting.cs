namespace Plunger.Data.Utils;

public static class Formatting
{
    public static DateTimeOffset GenerateTimeStamp()
    {
        return DateTimeOffset.UtcNow;
    }
}
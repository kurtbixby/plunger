namespace Plunger.Data.DbModels;

public class ReleaseDate
{
    public int Id { get; set; }
    public DateTimeOffset Date { get; set; }
    public Common.Enums.DateFormat DateFormat { get; set; }
    public int PlatformId { get; set; }
    public Platform Platform { get; set; }
    public Common.Enums.Region Region { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }
    public Guid Checksum { get; set; }
}
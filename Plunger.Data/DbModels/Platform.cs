namespace Plunger.Data.DbModels;

public class Platform
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string? AltName { get; set; }
    public string Abbreviation { get; set; }
    public int? Generation { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }
    public Guid Checksum { get; set; }
}
namespace Plunger.Data.DbModels;

public class Platform
{
    public int Id { get; set; }
    public string Abbreviation { get; set; }
    public string AltName { get; set; }
    public int Checksum { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public int Generation { get; set; }
    public string Name { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }
}
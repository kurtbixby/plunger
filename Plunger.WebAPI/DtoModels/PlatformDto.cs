namespace Plunger.WebApi.DtoModels;

public record PlatformDto()
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string? AltName { get; set; }
    public string Abbreviation { get; set; }
}
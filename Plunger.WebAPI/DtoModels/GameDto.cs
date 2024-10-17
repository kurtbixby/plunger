namespace Plunger.WebApi.DtoModels;

public record GameDto()
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string ShortName { get; set; }
    public string? CoverImageId { get; set; }
    public List<PlatformDto> Platforms { get; set; }
    public Guid VersionId { get; set; }
}
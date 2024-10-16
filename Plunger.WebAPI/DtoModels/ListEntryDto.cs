namespace Plunger.WebApi.DtoModels;

public record ListEntryDto
{
    public int Id { get; set; }
    public GameDto Game { get; set; }
    public List<CollectionGameDto> CollectionEntries { get; set; }
    public GameStatusDto? Status { get; set; }
}
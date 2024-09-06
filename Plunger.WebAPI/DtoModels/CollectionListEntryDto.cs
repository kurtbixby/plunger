namespace Plunger.WebApi.DtoModels;

public class CollectionListEntryDto
{
    public GameDto Game { get; set; }
    public CollectionGameDto CollectionEntry { get; set; }
    public GameStatusDto Status { get; set; }
}
using Plunger.Data.Enums;

namespace Plunger.WebApi.DtoModels;

public record GameStatusListEntryDto
{
    public GameDto Game { get; set; }
    public List<CollectionGameDto> CollectionEntries { get; set; }
    public GameStatusDto? Status { get; set; } 
}
using Plunger.Common.Enums;

namespace Plunger.WebApi.DtoModels;

public record UserListDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public ListType Type { get; set; }
    public List<ListEntryDto> ListEntries { get; set; }
}
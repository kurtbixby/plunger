using Plunger.Common.Enums;

namespace Plunger.WebApi.DtoModels;

public record GameStatusEdits()
{
    public int Id { get; init; }
    public DateTimeOffset TimeStamp { get; init; }
    public PlayState PlayState { get; init; }
    public TimeSpan TimePlayed { get; init; }
    public bool Completed { get; init; }
    public Guid VersionId { get; init; }
}
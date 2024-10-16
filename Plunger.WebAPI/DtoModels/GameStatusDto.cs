namespace Plunger.WebApi.DtoModels;

public record GameStatusDto()
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public bool Completed { get; set; }
    public int PlayState { get; set; }
    public TimeSpan TimePlayed { get; set; }
    public DateTimeOffset? DateStarted { get; set; }
    public Guid VersionId { get; set; }
}
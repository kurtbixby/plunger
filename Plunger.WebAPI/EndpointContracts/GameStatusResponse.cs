namespace Plunger.WebApi.EndpointContracts;

public record GameStatusResponse()
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public bool Completed { get; set; }
    public int PlayState { get; set; }
    public TimeSpan TimePlayed { get; set; }
    public DateTimeOffset? TimeStarted { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }
    
    public string Name { get; set; }
    public string ShortName { get; set; }
}
namespace Plunger.WebApi.DtoModels;

public record GameStatusResponse()
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public bool Completed { get; set; }
    public int PlayState { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }
    
    public string Name { get; set; }
    public string ShortName { get; set; }
}
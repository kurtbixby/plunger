namespace Plunger.Data.DbModels;

public class Collection
{
    public int Id { get; set; }

    public int UserId { get; set; }
    public User User { get; set; }
}
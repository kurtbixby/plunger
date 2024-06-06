namespace Plunger.Data.DbModels;

public class Collection : ProtectedEntity
{
    public int Id { get; set; }

    public int UserId { get; set; }
    public User User { get; set; }
    public List<CollectionGame> Games { get; set; }
}
namespace Plunger.Data;

using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

public class PlungerUserDbContext : IdentityDbContext<IdentityUser>
{
    public PlungerUserDbContext(DbContextOptions<PlungerUserDbContext> options) :
        base(options)
    { }
    
    public DbSet<AccessToken> Sessions { get; set; }
}
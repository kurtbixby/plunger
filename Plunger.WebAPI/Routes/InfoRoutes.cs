using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Plunger.Data;

namespace Plunger.WebApi.Routes;

public static class InfoRoutes
{
    public static RouteGroupBuilder MapInfoRoutes(this RouteGroupBuilder group)
    {
        group.MapGet("/info/platforms", RetrievePlatforms);

        return group;
    }

    private static IQueryable RetrievePlatforms([FromServices] PlungerDbContext db)
    {
        #warning TODO: SQL Injection Possible?
        return db.Platforms.Select(p => new { p.Id, p.Name, p.AltName, p.Abbreviation, p.Generation });
    }
}
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Plunger.Data;

namespace Plunger.WebApi.Routes;

public static class GameRoutes
{
    public static RouteGroupBuilder MapGameRoutes(this RouteGroupBuilder group)
    {
        group.MapGet("/games", SearchGame);
        // group.MapPost("/users/{userid}/collection", AddToCollection).RequireAuthorization();
        // group.MapPatch("/users/{userid}/collection/{itemid}", EditCollection).RequireAuthorization();
        // group.MapDelete("/users/{userid}/collection/{itemid}", DeleteFromCollection).RequireAuthorization();

        return group;
    }

    private static IQueryable SearchGame([FromQuery(Name = "name")] string name, [FromServices] PlungerDbContext db)
    {
        #warning TODO: SQL Injection Possible?
        return db.Games.Include(g => g.Platforms).Include(g => g.Cover).Include(g => g.ReleaseDates)
            .Select(g => new {
                g.Id, 
                g.Name,
                Platforms = g.Platforms.Select(p => new { p.Id, p.Name, p.AltName }).ToList(),
                CoverImageId = g.Cover != null ? g.Cover.ImageId : "",
                Regions = g.ReleaseDates.Select(r => r.Region).Distinct().Select(regionId => new { Id = regionId, Name = regionId.ToString() })
            })
            .Where(g => EF.Functions.ILike(g.Name, $"%{name}%")).Take(20);
    }
}
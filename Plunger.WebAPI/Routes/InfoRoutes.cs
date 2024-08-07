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
        return db.Games.Include(g => g.Platforms).Include(g => g.Cover).Select(g => new { g.Id, g.Name, Platforms = g.Platforms.Select(p => new { p.Id, p.Name, p.AltName }).ToList(), CoverUrl = g.Cover != null ? g.Cover.Url : "" }).Where(g => EF.Functions.ILike(g.Name, $"%{name}%")).Take(20);
    }
}
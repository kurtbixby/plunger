using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Plunger.Data;
using Plunger.Data.DbModels;
using Plunger.Data.Enums;
using Plunger.WebApi.DtoModels;
using Plunger.WebApi.EndpointContracts;

namespace Plunger.WebApi.Routes;

public static class CollectionRoutes
{
    public static RouteGroupBuilder MapCollectionRoutes(this RouteGroupBuilder group)
    {
        group.MapPost("/users/{userid}/collection", AddToCollection).RequireAuthorization();
        group.MapGet("/users/{username}/collection", GetCollection);
        group.MapPatch("/users/{userid}/collection/{itemid}", EditCollection).RequireAuthorization();
        group.MapDelete("/users/{userid}/collection/{itemid}", DeleteFromCollection).RequireAuthorization();

        return group;
    }

    private static GetCollectionResponse GetCollection([FromRoute] string username, [FromServices] PlungerDbContext db)
    {
        var collections = db.Collections.Select(
            c => new
            {
                c.Id, c.User, Games = c.Games.Select(g => new CollectionResponseCollectionGame()
                {
                    Id = g.Id, TimeAdded = g.TimeAdded, TimeAcquired = g.TimeAcquired, Physicality = g.Physicality, PurchasePrice = g.PurchasePrice,
                    Region = new RegionDto()
                    {
                        Id = g.RegionId,
                        // Technically this name is not correct and it should grab from the database
                        Name = EnumStrings.RegionNames[(int)g.Region]
                    },
                    Platform = new PlatformDto()
                    {
                        Id = g.PlatformId,
                        Name = g.Platform.Name,
                        AltName = g.Platform.AltName,
                        Abbreviation = g.Platform.Abbreviation
                    },
                    Game = new GameDto()
                    {
                        Id = g.Game.Id,
                        Name = g.Game.Name,
                        ShortName = g.Game.ShortName,
                        CoverUrl = g.Game.Cover != null ? g.Game.Cover.Url : null
                    }
                })
            }).Where(collection => collection.User.Username == username);

        var foo = collections.ToList();

        return new GetCollectionResponse() { Games = collections.First().Games };
        // return db.Collections.Select(
        //     c => new
        //     {
        //         c.Id, c.UserId, Games = c.Games.Select(g => new
        //         {
        //             g.Id, g.TimeAdded, g.TimeAcquired, g.Physicality, g.GameId, g.PlatformId, g.RegionId, g.VersionId
        //         })
        //     }).Where(collection => collection.UserId == username);
    }

    private static async Task<IResult> AddToCollection(HttpContext httpContext, [FromRoute] int userId,
        [FromServices] PlungerDbContext db, [FromBody] CollectionAddGameRequest req)
    {
        var validRes = req.Validate();

        if (!validRes.IsValid)
        {
            return Results.BadRequest(validRes.ValidationErrors);
        }

        {
            // Token fingerprint verification
            var fingerprintHash = IdUtils.GetFingerprint(httpContext.User);
            var fingerprint = httpContext.Request.Cookies[Constants.TokenFingerprint];
            var goodToken = TokenUtils.VerifyTokenFingerprint(fingerprintHash, fingerprint);
            if (!goodToken)
            {
                return Results.BadRequest(new { Error = "Invalid token fingerprint" });
            }
        }

        // Check ownership
        if (!IdUtils.CheckUserOwnership(httpContext.User, userId.ToString()))
        {
            return Results.Unauthorized();
        }

        try
        {
            var collection = await db.Collections.SingleAsync(c => c.UserId == userId);

            var dateAcquired = req.TimeAcquired?.UtcDateTime.Date ?? req.TimeAcquired;

            var collectionGame = new CollectionGame()
            {
                Collection = collection, GameId = req.GameId, PlatformId = req.PlatformId, RegionId = (int)req.Region,
                Region = req.Region, TimeAdded = DateTimeOffset.UtcNow, TimeAcquired = dateAcquired, PurchasePrice = req.PurchasePrice, Physicality = req.Physicality, VersionId = Guid.NewGuid()
            };

            await db.CollectionGames.AddAsync(collectionGame);
            await db.SaveChangesAsync();

            return Results.Ok(new
            {
                collectionGame.Id, collectionGame.GameId, collectionGame.PlatformId, collectionGame.RegionId,
                collectionGame.TimeAdded, collectionGame.TimeAcquired, collectionGame.PurchasePrice, collectionGame.Physicality, collectionGame.VersionId
            });
        }
        catch (InvalidOperationException e)
        {
            return Results.Problem();
        }
    }

    private static async Task<IResult> EditCollection([FromRoute] int userId, [FromRoute] int itemId,
        [FromServices] PlungerDbContext db, [FromBody] CollectionGamePatchRequest req)
    {
        var validRes = req.Validate();

        if (!validRes.IsValid)
        {
            return Results.BadRequest(validRes.ValidationErrors);
        }

        try
        {
            var item = await db.CollectionGames.SingleAsync(cg => cg.Id == itemId);

            if (item.VersionId.CompareTo(req.VersionId) != 0)
            {
                return Results.BadRequest(new { Message = "Incorrect VersionId" });
            }

            item.TimeAcquired = req.TimeAcquired ?? item.TimeAcquired;
            item.Physicality = req.Physicality ?? item.Physicality;
            if (req.GameId != null)
            {
                var game = await db.Games.FindAsync(req.GameId);
                item.GameId = game == null ? (int)req.GameId : item.GameId;
            }
            if (req.PlatformId != null)
            {
#warning TODO: Check that PlatformID is valid for the game
                var platform = await db.Platforms.FindAsync(req.PlatformId);
                item.PlatformId = platform == null ? (int)req.PlatformId : item.PlatformId;
            }
            if (req.RegionId != null)
            {
#warning TODO: Check that RegionID is valid for the game
                var region = await db.Regions.FindAsync(req.RegionId);
                item.RegionId = region == null ? (int)req.RegionId : item.RegionId;
            }
            item.VersionId = Guid.NewGuid();

            await db.SaveChangesAsync();
        
            return Results.Ok(item);
        }
        catch (InvalidOperationException e)
        {
            Console.WriteLine(e);
            return Results.Problem();
        }
    }

    private static async Task<IResult> DeleteFromCollection([FromRoute] int userId, [FromRoute] int itemId,
        [FromServices] PlungerDbContext db)
    {
        var item = await db.CollectionGames.FindAsync(itemId);
        if (item == null)
        {
            return Results.BadRequest("Invalid itemid");
        }
        db.CollectionGames.Remove(item);
        await db.SaveChangesAsync();

        return Results.Ok();
    }
}
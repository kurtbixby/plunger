using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic.CompilerServices;
using Plunger.Data;
using Plunger.Data.DbModels;
using Plunger.Common.Enums;
using Plunger.WebApi.DataLayer;
using Plunger.WebApi.DtoModels;
using Plunger.WebApi.EndpointContracts;

namespace Plunger.WebApi.Routes;

public static class CollectionRoutes
{
    public static RouteGroupBuilder MapCollectionRoutes(this RouteGroupBuilder group)
    {
        group.MapPost("/users/{userid}/collection", AddToCollection).RequireAuthorization();
        group.MapGet("/users/{username}/collection", GetCollection);
        group.MapPatch("/users/{userid}/collection", EditCollectionEntry).RequireAuthorization();
        group.MapPatch("/users/{userid}/collection/{itemid}", EditCollection).RequireAuthorization();
        group.MapDelete("/users/{userid}/collection/{itemid}", DeleteFromCollection).RequireAuthorization();

        return group;
    }

    private static GetCollectionResponse GetCollection([FromRoute] string username, [FromServices] PlungerDbContext db)
    {
        var userId = db.Users.First(u => string.Equals(u.Username, username)).Id;
        var collections = db.Collections.Include(c => c.Games).ThenInclude(cg => cg.Game).ThenInclude(g => g.Platforms)
            .Select(
                c => new
                {
                    c.Id, c.User, Games = c.Games.Select(g => new CollectionResponseCollectionGame()
                    {
                        Id = g.Id, TimeAdded = g.TimeAdded, TimeAcquired = g.TimeAcquired, Physicality = g.Physicality,
                        PurchasePrice = g.PurchasePrice, VersionId = g.VersionId,
                        Region = new RegionDto()
                        {
                            Id = g.RegionId,
                            // Technically this name is not correct and it should grab from the database
                            Name = EnumStrings.RegionNames[g.RegionId]
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
                            CoverImageId = g.Game.Cover != null ? g.Game.Cover.ImageId : null,
                            Platforms = g.Game.Platforms.Select(p => new PlatformDto()
                            {
                                Id = p.Id,
                                Name = p.Name,
                                AltName = p.AltName,
                                Abbreviation = p.Abbreviation
                            }).ToList()
                        }
                    })
                }).Where(collection => collection.User.Username == username);

        var games = collections.First().Games.ToList();

        var gameIds = games.Select(g => g.Game.Id);

        var statuses = DataLayer.Utils.GameStatusesForGameIds(db, userId, gameIds);
        
        foreach (var entry in games.Where(entry => statuses.ContainsKey(entry.Game.Id)))
        {
            entry.Status = statuses[entry.Game.Id];
        }
        
        // var gameStatuses = db.Users.Include(u => u.GameStatuses).Where(u => string.Equals(u.Username, username))
        //     .Select(u => u.GameStatuses.Select(gs => new
        //     {
        //         GameId = gs.GameId,
        //         GameStatus = new GameStatusDto()
        //         {
        //             Id = gs.Id,
        //             UserId = gs.UserId,
        //             Completed = gs.Completed,
        //             PlayState = gs.PlayState,
        //             TimePlayed = gs.TimePlayed,
        //             DateStarted = gs.TimeStarted
        //         }
        //     }).Where(gs => gameIds.Contains(gs.GameId))).ToDictionary(gs => gs.GameId, gs => gs.);

        return new GetCollectionResponse() { Games = games };
        // return new GetCollectionResponse() { Games = collections.First().Games };
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
                TimeAdded = DateTimeOffset.UtcNow, TimeAcquired = dateAcquired, PurchasePrice = req.PurchasePrice, Physicality = req.Physicality, VersionId = Guid.NewGuid()
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

    private static async Task<IResult> EditCollectionEntry([FromRoute] int userId,
        [FromServices] PlungerDbContext db, [FromBody] CollectionEditEntryRequest req)
    {
        // var validRes = req.Validate();
        //
        // if (!validRes.IsValid)
        // {
        //     return Results.BadRequest(validRes.ValidationErrors);
        // }

        try
        {
            var results = new List<DbEditResponse>();
            if (req.EntryEdits != null)
            {
                results.Add(await CollectionLayer.EditCollectionEntry(db, req.EntryEdits));
            }
            if (req.StatusEdits != null)
            {
                results.Add(await GameStatusLayer.EditGameStatus(db, req.StatusEdits));
            }

            if (results.Any(e => !e.Success))
            {
                return Results.Problem();
            }
            
            await db.SaveChangesAsync();
        
            return Results.Ok();
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
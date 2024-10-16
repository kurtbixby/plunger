using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Plunger.Data;
using Plunger.Data.DbModels;
using Plunger.Common.Enums;
using Plunger.WebApi.DtoModels;
using Plunger.WebApi.EndpointContracts;

namespace Plunger.WebApi.Routes;

public static class GameStateRoutes
{
    public static RouteGroupBuilder MapGameStateRoutes(this RouteGroupBuilder group)
    {
        group.MapPost("/users/{userid}/games/", CreateGameState).RequireAuthorization();
        group.MapGet("/users/{userId}/games/{gameId}", GetGameState);
        group.MapGet("/users/{username}/games/", GetGameStates);
        group.MapPatch("/users/{userId}/games/{gameId}", EditGameState).RequireAuthorization();

        return group;
    }

#warning TODO: ImplementAuthentication & Authorization
#warning TODO: Handle multiple "create" requests for the same game
    private static IQueryable GetGameState([FromServices] PlungerDbContext dbContext, [FromRoute] int userId,
        [FromRoute] int gameId)
    {
        return dbContext.GameStatuses.Where(e => e.UserId == userId && e.GameId == gameId).OrderByDescending(e => e.UpdatedAt).Select(e => new {Id = e.Id, GameId = e.GameId, UserId = e.UserId, Completed = e.Completed, PlayState = e.PlayState, UpdatedAt = e.UpdatedAt, PlayStateChanges = e.PlayStateChanges, VersionId = e.VersionId});
    }

    private static async Task<IResult> CreateGameState([FromServices] PlungerDbContext dbContext,
        [FromRoute] int userId, [FromBody] CreateGameStatusRequest createGameReq)
    {
#warning TODO: Check for valid userid
        var user = await dbContext.Users.FindAsync(userId);
        if (user == null)
        {
            // invalid user
            return Results.BadRequest(new {Message = "Invalid user"});
        }
#warning TODO: Check for valid game
        var game = await dbContext.Games.FindAsync(createGameReq.GameId);
        if (game == null)
        {
            // invalid game
            return Results.BadRequest(new {Message = "Invalid game"});
        }

        var existingGame = await dbContext.GameStatuses.AnyAsync(e => e.UserId == userId && e.GameId == createGameReq.GameId);
        if (existingGame)
        {
            return Results.Conflict(new {Message = $"Game status already exists for {game.Name} ({createGameReq.GameId})"});
        }
    
        var gameId = createGameReq.GameId;
        var completed = createGameReq.Completed ?? false;
    
        if (!createGameReq.PlayState.HasValue)
        {
#warning TODO: Add logic for invalid playstate
            return Results.BadRequest();
        }

        var editTime = Data.Utils.Formatting.GenerateTimeStamp();

        var timePlayed = createGameReq.TimePlayed ?? TimeSpan.Zero; 
        var timeStarted = createGameReq.PlayState == PlayState.InProgress ? editTime : (DateTimeOffset?)null;
        var status = new GameStatus() { UserId = userId,  GameId = gameId, Completed = completed, PlayState = (int)createGameReq.PlayState.Value, UpdatedAt = editTime, TimePlayed = timePlayed, TimeStarted = timeStarted, VersionId = Guid.NewGuid()};
        var stateChange = new PlayStateChange() { UpdatedAt = status.UpdatedAt, NewState = (int)createGameReq.PlayState.Value, GameStatus = status, TimePlayed = timePlayed, Completed = completed };
        dbContext.GameStatuses.Add(status);
        dbContext.PlayStateChanges.Add(stateChange);
        await dbContext.SaveChangesAsync();
    
        return Results.Ok(new GameStatusResponse()
        {
            Id = status.Id, UserId = status.UserId, Completed = status.Completed, PlayState = status.PlayState,
            UpdatedAt = status.UpdatedAt, Name = status.Game.Name, ShortName = status.Game.ShortName, TimePlayed = status.TimePlayed, TimeStarted = status.TimeStarted, VersionId = status.VersionId
        });
    }

    private static IResult GetGameStates([FromServices] PlungerDbContext db, [FromRoute] string username)
    {
        var userId = db.Users.First(u => string.Equals(u.Username, username)).Id;
        var gameStatuses = db.GameStatuses.Include(gs => gs.Game).ThenInclude(g => g.Cover).Where(gs => gs.UserId == userId).Include(gs => gs.PlayStateChanges).Select(gs => new
        {
            gs.Id, gs.Completed, gs.PlayState, gs.TimePlayed, gs.TimeStarted, gs.PlayStateChanges, gs.VersionId,
            Game = new
            {
                gs.Game.Id, gs.Game.Name, gs.Game.ShortName, coverUrl = gs.Game.Cover != null ? gs.Game.Cover.Url : null
            },
            CollectionEntries = gs.User.Collection.Games.Where(cg => cg.GameId == gs.GameId).Select(cg => new
            {
                cg.Id, cg.Physicality, cg.TimeAcquired, cg.TimeAdded, cg.PurchasePrice, cg.VersionId, Platform = new PlatformDto
                {
                    Id = cg.Platform.Id,
                    Name = cg.Platform.Name,
                    AltName = cg.Platform.AltName,
                    Abbreviation = cg.Platform.Abbreviation
                },
                Region = new RegionDto
                {
                    Id = cg.RegionId,
                    Name = EnumStrings.RegionNames[cg.RegionId]
                }
            })
        });
        
        return Results.Ok(gameStatuses.ToList());
    }

    private static async Task<IResult> EditGameState(HttpContext httpContext, [FromServices] PlungerDbContext dbContext,
        [FromRoute] int userId, [FromRoute] int gameId, [FromBody] UpdateGameStatusRequest updateGameReq)
    {
        var status = await dbContext.GameStatuses.Include(e => e.Game).FirstAsync(e => e.UserId == userId && e.GameId == gameId);
        if (status.VersionId.CompareTo(updateGameReq.VersionId) != 0)
        {
            return Results.BadRequest(new { Message = "Incorrect Version Id" });
        }

        var editTime = Data.Utils.Formatting.GenerateTimeStamp();

        if ((status.PlayState == (int)PlayState.Unplayed || status.PlayState == (int)PlayState.Unspecified) && updateGameReq.PlayState == PlayState.InProgress)
        {
            status.TimeStarted = editTime;
        }
        status.PlayState = (int)updateGameReq.PlayState;
        status.VersionId = Guid.NewGuid();
        status.UpdatedAt = editTime;
        status.TimePlayed = updateGameReq.TimePlayed;
        var newStateChange = new PlayStateChange()
            { UpdatedAt = editTime, NewState = (int)updateGameReq.PlayState, GameStatusId = status.Id, TimePlayed = updateGameReq.TimePlayed};
        await dbContext.PlayStateChanges.AddAsync(newStateChange);
        await dbContext.SaveChangesAsync();
    
        return Results.Ok(new GameStatusResponse()
        {
            Id = status.Id, UserId = status.UserId, Completed = status.Completed, PlayState = status.PlayState, TimePlayed = status.TimePlayed, TimeStarted = status.TimeStarted,
            UpdatedAt = status.UpdatedAt, Name = status.Game.Name, ShortName = status.Game.ShortName, VersionId = status.VersionId
        });
    }
}
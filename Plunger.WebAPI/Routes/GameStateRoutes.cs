using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Plunger.Data;
using Plunger.Data.DbModels;
using Plunger.Data.Enums;
using Plunger.WebApi.DtoModels;
using Plunger.WebApi.EndpointContracts;

namespace Plunger.WebApi.Routes;

public static class GameStateRoutes
{
    public static RouteGroupBuilder MapGameStateRoutes(this RouteGroupBuilder group)
    {
        group.MapPost("/users/{userid}/games/", CreateGameState).RequireAuthorization();
        group.MapGet("/users/{userId}/games/{gameId}", GetGameState);
        group.MapPatch("/users/{userId}/games/{gameId}", EditGameState).RequireAuthorization();

        return group;
    }

#warning TODO: ImplementAuthentication & Authorization
#warning TODO: Handle multiple "create" requests for the same game
    private static IQueryable GetGameState([FromServices] PlungerDbContext dbContext, [FromRoute] int userId,
        [FromRoute] int gameId)
    {
        return dbContext.GameStatuses.Where(e => e.UserId == userId && e.GameId == gameId).OrderByDescending(e => e.UpdatedAt).Select(e => new {Id = e.Id, GameId = e.GameId, UserId = e.UserId, Completed = e.Completed, PlayState = e.PlayState, UpdatedAt = e.UpdatedAt, PlayStateChanges = e.PlayStateChanges});
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

        var timeStarted = createGameReq.PlayState == PlayState.InProgress ? createGameReq.TimeStamp : (DateTimeOffset?)null;
        var status = new GameStatus() { UserId = userId,  GameId = gameId, Completed = completed, PlayState = (int)createGameReq.PlayState.Value, UpdatedAt = createGameReq.TimeStamp, TimePlayed = TimeSpan.Zero, TimeStarted = timeStarted};
        var stateChange = new PlayStateChange() { UpdatedAt = createGameReq.TimeStamp, NewState = (int)createGameReq.PlayState.Value, GameStatus = status, TimePlayed = TimeSpan.Zero};
        dbContext.GameStatuses.Add(status);
        dbContext.PlayStateChanges.Add(stateChange);
        await dbContext.SaveChangesAsync();
    
        return Results.Ok(new GameStatusResponse()
        {
            Id = status.Id, UserId = status.UserId, Completed = status.Completed, PlayState = status.PlayState,
            UpdatedAt = status.UpdatedAt, Name = status.Game.Name, ShortName = status.Game.ShortName, TimePlayed = status.TimePlayed, TimeStarted = status.TimeStarted
        });
    }

    private static async Task<IResult> EditGameState(HttpContext httpContext, [FromServices] PlungerDbContext dbContext,
        [FromRoute] int userId, [FromRoute] int gameId, [FromBody] UpdateGameStatusRequest updateGameReq)
    {
        var status = await dbContext.GameStatuses.Include(e => e.Game).FirstAsync(e => e.UserId == userId && e.GameId == gameId);
        if (updateGameReq.TimeStamp < status.UpdatedAt)
        {
            return Results.BadRequest(new { Message = "Time stamp older than current time stamp" });
        }

        if (updateGameReq.PlayState == PlayState.InProgress)
        {
            status.TimeStarted = updateGameReq.TimeStamp;
        }
        status.PlayState = (int)updateGameReq.PlayState;
        status.UpdatedAt = updateGameReq.TimeStamp;
        status.TimePlayed = updateGameReq.TimePlayed;
        var newStateChange = new PlayStateChange()
            { UpdatedAt = updateGameReq.TimeStamp, NewState = (int)updateGameReq.PlayState, GameStatusId = status.Id, TimePlayed = updateGameReq.TimePlayed};
        await dbContext.PlayStateChanges.AddAsync(newStateChange);
        await dbContext.SaveChangesAsync();
    
        return Results.Ok(new GameStatusResponse()
        {
            Id = status.Id, UserId = status.UserId, Completed = status.Completed, PlayState = status.PlayState, TimePlayed = status.TimePlayed, TimeStarted = status.TimeStarted,
            UpdatedAt = status.UpdatedAt, Name = status.Game.Name, ShortName = status.Game.ShortName
        });
    }
}
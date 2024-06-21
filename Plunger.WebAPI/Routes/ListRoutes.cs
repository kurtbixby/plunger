using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Plunger.Data;
using Plunger.Data.DbModels;
using Plunger.WebApi.DtoModels;

namespace Plunger.WebApi.Routes;

public static class ListRoutes
{
    public static RouteGroupBuilder MapListRoutes(this RouteGroupBuilder group)
    {
        group.MapPost("/lists/", CreateList).RequireAuthorization();
        group.MapGet("/lists/{listId}", GetList);
        group.MapPatch("/lists/{listId}", EditList).RequireAuthorization();

        return group;
    }

    private static async Task<IResult> CreateList([FromBody] NewListRequest req, [FromServices] PlungerDbContext db)
    {
        var validation = req.Validate();

        if (!validation.IsValid)
        {
            return Results.BadRequest(validation.ValidationErrors);
        }
    
#warning TODO: IMPLEMENT REAL USER LOGIC
        var userId = 1;
    
        // Checks for entries that reference a valid, existing game
        var entryIds = req.Entries.Select(e => e.GameId);
        var validGameIds = db.Games.Where(e => entryIds.Contains(e.Id)).Select(e => e.Id);
        var entries = req.Entries.Where(e => validGameIds.Contains(e.GameId)).Select(e => new GameListEntry() { Number = e.Number, GameId = e.GameId });
        var gameListEntries = req.Unordered ? entries.ToList() : entries.OrderBy(e => e.Number).ToList();
    
        var list = new GameList() { Name = req.Name, Unordered = req.Unordered, UserId = userId, GameListEntries = gameListEntries.ToList() };
        await db.GameLists.AddRangeAsync(list);
        var res = await db.SaveChangesAsync();

        return Results.Json(new {Id = list.Id, Name = list.Name});
    }

    private static async Task<List<GameList>> GetList([FromRoute] int listId, [FromServices] PlungerDbContext db)
    {
        return await db.GameLists.Where(list => list.Id == listId).ToListAsync();
    }

    private static async Task<IResult> EditList([FromBody] ListUpdateRequest request,
        [FromServices] PlungerDbContext dbContext, [FromRoute] int listId)
    {
        var validation = request.Validate();
    if (!validation.IsValid)
    {
        return Results.BadRequest(validation.ValidationErrors);
    }

    var fetchList = await dbContext.GameLists.Include(e => e.GameListEntries).Where(e => e.Id == listId).ToListAsync();
    if (fetchList.Count == 0)
    {
        return Results.NotFound(new { Message = "List not found" });
    }

    var list = fetchList[0];

    if (request.VersionId.CompareTo(list.VersionId) != 0)
    {
        return Results.BadRequest(new { Message = "Invalid version of the list" });
    }
    
    var error = false;
    request.Updates.ForEach((e) =>
    {
        if (!error)
        {
            switch (e.Action)
            {
                case ListUpdateRequest.ListUpdateAction.ChangeName:
                    list.Name = e.Payload;
                    break;
                case ListUpdateRequest.ListUpdateAction.ChangeOrdered:
                    list.Unordered = Convert.ToBoolean(e.Payload);
                    break;
                case ListUpdateRequest.ListUpdateAction.AddGame:
                    var gameId = Convert.ToInt32(e.Payload);

                    var listEntry = new GameListEntry()
                        { GameId = gameId, GameList = list, Number = list.GameListEntries.Count };
                    list.GameListEntries.Add(listEntry);
                    break;
                case ListUpdateRequest.ListUpdateAction.RemoveGame:
                    var gameNumber = Convert.ToInt32(e.Payload);
                    if (list.GameListEntries.Count <= gameNumber)
                    {
                        error = error || true;
                        break;
                    }
                    list.GameListEntries.RemoveAt(gameNumber);
                    break;
                case ListUpdateRequest.ListUpdateAction.MoveGame:
                    var moveAction = JsonSerializer.Deserialize<ListActionMoveGame>(e.Payload);

                    if (list.GameListEntries.Count <= moveAction.SourceNumber)
                    {
                        error = error || true;
                        break;
                    }

                    if (list.GameListEntries[moveAction.SourceNumber].GameId != moveAction.GameId)
                    {
                        error = error || true;
                        break;
                    }

                    var entry = list.GameListEntries[moveAction.SourceNumber];
                    list.GameListEntries.RemoveAt(moveAction.SourceNumber);
                    list.GameListEntries.Insert(moveAction.DestinationNumber, entry);
                    break;
            }
        }
    });
    if (error)
    {
        return Results.BadRequest(new { Message = "Error processing updates list"});
    }

    list.VersionId = Guid.NewGuid();
    await dbContext.SaveChangesAsync();
    
    return Results.Ok(new { });
    }
}
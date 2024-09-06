using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Plunger.Data;
using Plunger.Data.DbModels;
using Plunger.Data.Enums;
using Plunger.WebApi.DtoModels;

namespace Plunger.WebApi.DataLayer;

public static class ListFetching
{
    public static async Task<List<GameStatusListEntryDto>> RetrieveNowPlayingList(int userId, PlungerDbContext dbContext)
    {
        var incompleteResponses =
            (await dbContext.Users.Include(user => user.GameStatuses).ThenInclude(gs => gs.Game)
                .ThenInclude(game => game.Cover)
                .FirstAsync(user => user.Id == userId)).GameStatuses.Where(gs => gs.PlayState == (int)PlayState.InProgress)
            .OrderByDescending(gs => gs.UpdatedAt).Take(10).Select(gs => new GameStatusListEntryDto()
            {
                Game = new GameDto()
                {
                    Id = gs.Game.Id,
                    Name = gs.Game.Name,
                    ShortName = gs.Game.ShortName,
                    CoverUrl = gs.Game.Cover?.Url ?? "",
                },
                Status = new GameStatusDto()
                {
                    Id = gs.Id,
                    Completed = gs.Completed,
                    PlayState = gs.PlayState,
                    TimePlayed = gs.TimePlayed,
                    DateStarted = gs.TimeStarted
                }
            });
        
        // Find CollectionGames from GameStatus
        var filledResponses = FillResponseWithCollectionGames(incompleteResponses.ToList(), userId, dbContext);
        
        return filledResponses.ToList();
    }

    // Change this to return a list of game statuses
    // Use top 10 ids to join on gamestatus.gameid
    // create default status for those without any???
    public static async Task<List<CollectionListEntryDto>> RetrieveRecentlyAcquiredList(int userId, PlungerDbContext dbContext)
    {
        var collectionGames = (await dbContext.Users.Include(u => u.Collection).ThenInclude(c => c.Games).ThenInclude(g => g.Game)
            .FirstAsync(user => user.Id == userId)).Collection.Games.OrderByDescending(g => g.TimeAcquired).ThenByDescending(g => g.TimeAdded).Take(10).Select(cg => new CollectionListEntryDto
        {
            Game = new GameDto
            {
                Id = cg.GameId,
                Name = cg.Game.Name,
                ShortName = cg.Game.ShortName,
                CoverUrl = cg.Game.Cover?.Url ?? ""
            },
            CollectionEntry = new CollectionGameDto()
            {
                Id = cg.Id,
                TimeAdded = cg.TimeAdded,
                TimeAcquired = cg.TimeAcquired,
                PurchasePrice = cg.PurchasePrice,
                Physicality = cg.Physicality,
                PlatformId = cg.PlatformId,
                RegionId = cg.RegionId,
            },
        }).ToList();
        
        // Find GameStatus from CollectionGame
        var gameIds = collectionGames.Select(g => g.Game.Id);
        
        var statuses = dbContext.GameStatuses.Include(gs => gs.Game).ThenInclude(g => g.Cover).Where(gs => gameIds.Contains(gs.GameId)).Select(gs => new
        {
            GameId = gs.GameId,
            GameStatus = new GameStatusDto()
            {
                Id = gs.Id,
                Completed = gs.Completed,
                PlayState = gs.PlayState,
                TimePlayed = gs.TimePlayed,
                DateStarted = gs.TimeStarted
            }
        }).ToDictionary(gs => gs.GameId, gs => gs.GameStatus);

        foreach (var entry in collectionGames.Where(entry => statuses.ContainsKey(entry.Game.Id)))
        {
            entry.Status = statuses[entry.Game.Id];
        }
        
        return collectionGames.ToList();
    }

    #warning TODO: Refactor this into a single SQL query
    public static async Task<List<GameStatusListEntryDto>> RetrieveRecentlyStartedList(int userId, PlungerDbContext dbContext)
    {
        var incompleteResponses = (await dbContext.Users.Include(u => u.GameStatuses).ThenInclude(gs => gs.Game)
                .ThenInclude(g => g.Cover).FirstAsync(u => u.Id == userId))
            .GameStatuses.Where(gs => gs.PlayState == (int)PlayState.InProgress).Take(10).Select(gs => new GameStatusListEntryDto()
            {
                Game = new GameDto()
                {
                    Id = gs.Game.Id,
                    Name = gs.Game.Name,
                    ShortName = gs.Game.ShortName,
                    CoverUrl = gs.Game.Cover?.Url ?? ""
                },
                Status = new GameStatusDto()
                {
                    Id = gs.Id,
                    Completed = gs.Completed,
                    PlayState = gs.PlayState,
                    TimePlayed = gs.TimePlayed,
                    DateStarted = gs.TimeStarted
                }
            });

        var filledResponses = FillResponseWithCollectionGames(incompleteResponses.ToList(), userId, dbContext);
        
        return filledResponses.ToList();
    }

    private static List<GameStatusListEntryDto> FillResponseWithCollectionGames(List<GameStatusListEntryDto> partiallyFiledResponses, int userId, PlungerDbContext dbContext)
    {
        var gameIds = partiallyFiledResponses.Select(gs => gs.Game.Id);
        var collectionGameDict = dbContext.CollectionGames.Include(cg => cg.Collection)
            .Where(cg => cg.Collection.UserId == userId && gameIds.Contains(cg.GameId)).GroupBy(gs => gs.GameId)
            .ToDictionary(g => g.Key, g => g.Select(cg => new CollectionGameDto
            {
                Id = cg.Id,
                TimeAdded = cg.TimeAdded,
                TimeAcquired = cg.TimeAcquired,
                PurchasePrice = cg.PurchasePrice,
                Physicality = cg.Physicality,
                PlatformId = cg.PlatformId,
                RegionId = cg.RegionId
            }).ToList());
        
        foreach (var response in partiallyFiledResponses)
        {
            var id = response.Game.Id;
            if (!collectionGameDict.ContainsKey(id))
            {
                continue;
            }
            var collectionGames = collectionGameDict[id];
            response.CollectionEntries = collectionGames;
        }

        return partiallyFiledResponses;
    }
}
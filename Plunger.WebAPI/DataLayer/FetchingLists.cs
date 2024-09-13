using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Plunger.Common.Enums;
using Plunger.Data;
using Plunger.Data.DbModels;
using Plunger.Data.Enums;
using Plunger.WebApi.DtoModels;

namespace Plunger.WebApi.DataLayer;

public static class ListFetching
{
    public static async Task<UserListDto> RetrieveNowPlayingList(int userId, PlungerDbContext dbContext)
    {
        var incompleteResponses =
            (await dbContext.Users.Include(user => user.GameStatuses).ThenInclude(gs => gs.Game)
                .ThenInclude(game => game.Cover)
                .FirstAsync(user => user.Id == userId)).GameStatuses.Where(gs => gs.PlayState == (int)PlayState.InProgress)
            .OrderByDescending(gs => gs.UpdatedAt).Take(10).Select(gs => new ListEntryDto()
            {
                Id = gs.Game.Id,
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

        var list = new UserListDto
        {
            Id = (int)ListType.NowPlaying,
            Name = "Now Playing",
            Type = ListType.NowPlaying,
            ListEntries = filledResponses,
        };
        
        return list;
    }

    // Change this to return a list of game statuses
    // Use top 10 ids to join on gamestatus.gameid
    // create default status for those without any???
    public static async Task<UserListDto> RetrieveRecentlyAcquiredList(int userId, PlungerDbContext dbContext)
    {
        var collectionGames = (await dbContext.Users.Include(u => u.Collection).ThenInclude(c => c.Games).ThenInclude(g => g.Game)
            .FirstAsync(user => user.Id == userId)).Collection.Games.OrderByDescending(g => g.TimeAcquired).ThenByDescending(g => g.TimeAdded).Take(10).Select(cg => new ListEntryDto
        {
            Id = cg.Id,
            Game = new GameDto
            {
                Id = cg.GameId,
                Name = cg.Game.Name,
                ShortName = cg.Game.ShortName,
                CoverUrl = cg.Game.Cover?.Url ?? ""
            },
            CollectionEntries =
            [
                new CollectionGameDto
                {
                    Id = cg.Id,
                    TimeAdded = cg.TimeAdded,
                    TimeAcquired = cg.TimeAcquired,
                    PurchasePrice = cg.PurchasePrice,
                    Physicality = cg.Physicality,
                    PlatformId = cg.PlatformId,
                    RegionId = cg.RegionId,
                }
            ],
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

        var list = new UserListDto
        {
            Id = (int)ListType.RecentlyAcquired,
            Name = "Recently Acquired",
            Type = ListType.RecentlyAcquired,
            ListEntries = collectionGames.ToList(),
        };
        
        return list;
    }

    #warning TODO: Refactor this into a single SQL query
    public static async Task<UserListDto> RetrieveRecentlyStartedList(int userId, PlungerDbContext dbContext)
    {
        var incompleteResponses = (await dbContext.Users.Include(u => u.GameStatuses).ThenInclude(gs => gs.Game)
                .ThenInclude(g => g.Cover).FirstAsync(u => u.Id == userId))
            .GameStatuses.Where(gs => gs.PlayState == (int)PlayState.InProgress).Take(10).Select(gs => new ListEntryDto()
            {
                Id = gs.Game.Id,
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

        var list = new UserListDto
        {
            Id = (int)ListType.RecentlyStarted,
            Name = "Recently Started",
            Type = ListType.RecentlyStarted,
            ListEntries = filledResponses,
        };
        
        return list;
    }

    private static List<ListEntryDto> FillResponseWithCollectionGames(List<ListEntryDto> partiallyFiledResponses, int userId, PlungerDbContext dbContext)
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
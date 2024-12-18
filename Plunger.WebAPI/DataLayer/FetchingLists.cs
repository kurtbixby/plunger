using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Plunger.Common.Enums;
using Plunger.Data;
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
                    CoverImageId = gs.Game.Cover?.ImageId,
                },
                Status = new GameStatusDto()
                {
                    Id = gs.Id,
                    UserId = gs.UserId,
                    Completed = gs.Completed,
                    PlayState = gs.PlayState,
                    TimePlayed = gs.TimePlayed,
                    DateStarted = gs.TimeStarted,
                    VersionId = gs.VersionId
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
        var collectionGames = (await dbContext.Users.Include(u => u.Collection).ThenInclude(c => c.Games).ThenInclude(cg => cg.Game)
            .Include(u => u.Collection).ThenInclude(c => c.Games).ThenInclude(cg => cg.Platform)
            .FirstAsync(user => user.Id == userId)).Collection.Games.OrderByDescending(g => g.TimeAcquired).ThenByDescending(g => g.TimeAdded).Take(10).Select(cg => new ListEntryDto
        {
            Id = cg.Id,
            Game = new GameDto
            {
                Id = cg.GameId,
                Name = cg.Game.Name,
                ShortName = cg.Game.ShortName,
                CoverImageId = cg.Game.Cover?.ImageId ?? ""
            },
            CollectionEntries =
            [
                new CollectionGameDto
                {
                    Id = cg.Id,
                    UserId = userId,
                    TimeAdded = cg.TimeAdded,
                    TimeAcquired = cg.TimeAcquired,
                    PurchasePrice = cg.PurchasePrice,
                    Physicality = cg.Physicality,
                    Platform = new PlatformDto()
                    {
                        Id = cg.PlatformId,
                        Name = cg.Platform.Name,
                        AltName = cg.Platform.AltName,
                        Abbreviation = cg.Platform.Abbreviation
                    },
                    Region = new RegionDto()
                    {
                        Id = cg.RegionId,
                        // Technically this name is not correct and it should grab from the database
                        Name = EnumStrings.RegionNames[cg.RegionId]
                    },
                }
            ],
        }).ToList();
        
        // Find GameStatus from CollectionGame
        var gameIds = collectionGames.Select(g => g.Game.Id);
        
        // This has a bug. This DOES NOT filter on userId
        var statuses = dbContext.GameStatuses.Include(gs => gs.Game).ThenInclude(g => g.Cover).Where(gs => gameIds.Contains(gs.GameId)).Select(gs => new
        {
            GameId = gs.GameId,
            GameStatus = new GameStatusDto()
            {
                Id = gs.Id,
                UserId = gs.UserId,
                Completed = gs.Completed,
                PlayState = gs.PlayState,
                TimePlayed = gs.TimePlayed,
                DateStarted = gs.TimeStarted,
                VersionId = gs.VersionId
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
                    CoverImageId = gs.Game.Cover?.ImageId ?? ""
                },
                Status = new GameStatusDto()
                {
                    Id = gs.Id,
                    UserId = gs.UserId,
                    Completed = gs.Completed,
                    PlayState = gs.PlayState,
                    TimePlayed = gs.TimePlayed,
                    DateStarted = gs.TimeStarted,
                    VersionId = gs.VersionId
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
        var collectionGameDict = dbContext.CollectionGames.Include(cg => cg.Collection).Include(cg => cg.Platform)
            .Where(cg => cg.Collection.UserId == userId && gameIds.Contains(cg.GameId)).GroupBy(gs => gs.GameId)
            .ToDictionary(g => g.Key, g => g.Select(cg => new CollectionGameDto
            {
                Id = cg.Id,
                UserId = userId,
                TimeAdded = cg.TimeAdded,
                TimeAcquired = cg.TimeAcquired,
                PurchasePrice = cg.PurchasePrice,
                Physicality = cg.Physicality,
                Platform = new PlatformDto()
                {
                    Id = cg.PlatformId,
                    Name = cg.Platform.Name,
                    AltName = cg.Platform.AltName,
                    Abbreviation = cg.Platform.Abbreviation
                },
                Region = new RegionDto()
                {
                    Id = cg.RegionId,
                    // Technically this name is not correct and it should grab from the database
                    Name = EnumStrings.RegionNames[cg.RegionId]
                },
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
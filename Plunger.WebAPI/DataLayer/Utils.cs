using Microsoft.EntityFrameworkCore;
using Plunger.Data;
using Plunger.WebApi.DtoModels;

namespace Plunger.WebApi.DataLayer;

public class Utils
{
    public static Dictionary<int, GameStatusDto> GameStatusesForGameIds(PlungerDbContext db, int userId, IEnumerable<int> gameIds)
    {
        var gameStatuses = db.GameStatuses.Include(gs => gs.Game).ThenInclude(g => g.Cover)
            .Where(gs => gameIds.Contains(gs.GameId) && gs.UserId == userId).Select(gs => new
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

        return gameStatuses;
    }
}
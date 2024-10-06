using Microsoft.EntityFrameworkCore;
using Plunger.Common.Enums;
using Plunger.Data;
using Plunger.Data.DbModels;
using Plunger.WebApi.DtoModels;

namespace Plunger.WebApi.DataLayer;

public static class GameStatusLayer
{
    public static async Task<DbEditResponse> EditGameStatus(PlungerDbContext dbContext, GameStatusEdits statusEdits)
    {
        // var status = await dbContext.GameStatuses.Include(e => e.Game).FirstAsync(e => e.UserId == userId && e.GameId == gameId);
        var status = await dbContext.GameStatuses.Include(gs => gs.Game).FirstAsync(gs => gs.Id == statusEdits.Id);
        if (statusEdits.TimeStamp < status.UpdatedAt)
        {
            return new DbEditResponse()
            {
                Success = false,
                Message = "Time stamp older than current time stamp"
            };
        }

        if ((status.PlayState == (int)PlayState.Unplayed || status.PlayState == (int)PlayState.Unspecified) && statusEdits.PlayState == PlayState.InProgress)
        {
            status.TimeStarted = statusEdits.TimeStamp;
        }
        status.PlayState = (int)statusEdits.PlayState;
        status.UpdatedAt = statusEdits.TimeStamp;
        status.TimePlayed = statusEdits.TimePlayed;
        status.Completed = statusEdits.Completed;
        var newStateChange = new PlayStateChange()
            { UpdatedAt = statusEdits.TimeStamp, NewState = (int)statusEdits.PlayState, GameStatusId = status.Id, TimePlayed = statusEdits.TimePlayed, Completed = statusEdits.Completed };
        await dbContext.PlayStateChanges.AddAsync(newStateChange);

        return new DbEditResponse()
        {
            Success = true,
            Message = ""
        };

        // return Results.Ok(new GameStatusResponse()
        // {
        //     Id = status.Id, UserId = status.UserId, Completed = status.Completed, PlayState = status.PlayState, TimePlayed = status.TimePlayed, TimeStarted = status.TimeStarted,
        //     UpdatedAt = status.UpdatedAt, Name = status.Game.Name, ShortName = status.Game.ShortName
        // });
    }
}
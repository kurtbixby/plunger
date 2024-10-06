using Microsoft.EntityFrameworkCore;
using Plunger.Data;
using Plunger.WebApi.DtoModels;

namespace Plunger.WebApi.DataLayer;

public static class CollectionLayer
{
    public static async Task<DbEditResponse> EditCollectionEntry(PlungerDbContext dbContext, CollectionEntryEdits entryEdits)
    {
        var item = await dbContext.CollectionGames.SingleAsync(cg => cg.Id == entryEdits.Id);

        if (item.VersionId.CompareTo(entryEdits.VersionId) != 0)
        {
            return new DbEditResponse()
            {
                Success = false,
                Message = "Incorrect VersionId"
            };
        }

        item.TimeAcquired = entryEdits.TimeAcquired; // ?? item.TimeAcquired;
        item.Physicality = entryEdits.Physicality; // ?? item.Physicality;
        // if (entryEdits.Platform != null)
        // {
#warning TODO: Check that PlatformID is valid for the game
            var platform = await dbContext.Platforms.FindAsync(entryEdits.Platform);
            item.PlatformId = platform == null ? entryEdits.Platform : item.PlatformId;
        // }
        // if (entryEdits.Region != null)
        // {
#warning TODO: Check that RegionID is valid for the game
            var region = await dbContext.Regions.FindAsync((int)entryEdits.Region);
            item.RegionId = region?.Id ?? item.RegionId;
        // }
        item.VersionId = Guid.NewGuid();
        return new DbEditResponse()
        {
            Success = true,
            Message = ""
        };
    } 
}
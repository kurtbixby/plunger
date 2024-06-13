using System.Security.Claims;
using Plunger.Data;

namespace Plunger.WebApi;

public class IdUtils
{
    public static string GetUserId(ClaimsPrincipal user)
    {
        var identity = user.Identity as ClaimsIdentity;
        return identity.FindFirst("sub").Value;
    }
    
    public static string GetFingerprint(ClaimsPrincipal user)
    {
        if (user == null)
        {
            #warning TODO Add a proper exception type
            throw new Exception("User Identity null");
        }
        var identity = user.Identity as ClaimsIdentity;
        return identity.FindFirst(Constants.TokenFingerprint).Value;
    }

    public static bool CheckUserOwnership(ClaimsPrincipal user, string resourceOwnerId)
    {
        return string.Equals(resourceOwnerId, GetUserId(user));
    }
}
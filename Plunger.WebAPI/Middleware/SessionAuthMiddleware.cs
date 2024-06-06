using System.Text.Json;
using Plunger.Data;

namespace Plunger.WebApi.Middleware;

public class SessionAuthMiddleware
{
    private readonly RequestDelegate _next;
    
    public SessionAuthMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext httpContext, PlungerUserDbContext userDbContext)
    {
        var sessionString = httpContext.Session.GetString("session");
        if (sessionString == "")
        {
            // bad
            httpContext.Response.StatusCode = 403;
        }
        var sessionId = JsonSerializer.Deserialize<Guid>(sessionString);
        // Check session
        var session = await userDbContext.Sessions.FindAsync(sessionId);
        if (session == null)
        {
            // bad;
            httpContext.Response.StatusCode = 403;
        }
        if (DateTimeOffset.UtcNow < session.ExpirationTime)
        {
            // bad; expired session
            httpContext.Response.StatusCode = 403;
        }
        
        await _next(httpContext);
    }
}

public static class SessionAuthMiddlewareExtensions
{
    public static IApplicationBuilder UseSessionAuthMiddleware(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<SessionAuthMiddleware>();
    }
}
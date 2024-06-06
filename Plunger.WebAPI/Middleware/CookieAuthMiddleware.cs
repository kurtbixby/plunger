using Plunger.Data;

namespace Plunger.WebApi.Middleware;

public class CookieAuthMiddleware
{
    private readonly RequestDelegate _next;

    public CookieAuthMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext httpContext, PlungerUserDbContext userDbContext)
    {
        // retrieve cookie
        var token = "";
        //check validity
        // 
        var userid = "";
        httpContext.Items["userid"] = userid;
        await _next(httpContext);
    }
}

public static class CookieAuthMiddlewareExtensions
{
    public static IApplicationBuilder UseCookieAuthMiddleware(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<CookieAuthMiddleware>();
    }
}
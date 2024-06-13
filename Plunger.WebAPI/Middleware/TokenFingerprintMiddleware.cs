using System.Net;

namespace Plunger.WebApi.Middleware;

public class TokenFingerprintMiddleware
{
    private readonly RequestDelegate _next;

    public TokenFingerprintMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext httpContext)
    {
        if (!httpContext.User.Identity.IsAuthenticated)
        {
            await _next(httpContext);
        }
        else
        {
            var fingerprintHash = IdUtils.GetFingerprint(httpContext.User);
            var fingerprint = httpContext.Request.Cookies[Constants.TokenFingerprint];
            if (!string.IsNullOrEmpty(fingerprint) && TokenUtils.VerifyTokenFingerprint(fingerprintHash, fingerprint))
            {
                await _next(httpContext);
            }
            else
            {
                httpContext.Response.Clear();
                httpContext.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                await httpContext.Response.WriteAsync("Unauthorized");
            }
        }
    }
}

public static class TokenFingerprintMiddlewareExtensions
{
    public static IApplicationBuilder UseTokenFingerprintMiddleware(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<TokenFingerprintMiddleware>();
    }
}
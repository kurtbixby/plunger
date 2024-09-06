using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Plunger.Common;
using Plunger.Data;
using Plunger.Data.DbModels;
using Plunger.WebApi.DtoModels;
using Plunger.WebApi.EndpointContracts;

namespace Plunger.WebApi.Routes;

public static class UsersRoutes
{
    public static RouteGroupBuilder MapUsersRoutes(this RouteGroupBuilder group)
    {
        group.MapPost("/users/", CreateUser);
        group.MapPost("/users/login", LoginUser);
        group.MapGet("/users/tokenlogin", TokenLogin).RequireAuthorization();

        return group;
    }
    
    private static async Task<IResult> CreateUser([FromServices] PlungerDbContext dbContext, [FromBody] NewUserRequest newUserReq)
    {
        #warning TODO: Validate username & password format
    
        var validationRes = newUserReq.Validate();
        if (!validationRes.IsValid)
        {
            // early return
            // invalid request
            return Results.BadRequest(validationRes.ValidationErrors);
        }
    
        #warning TODO: Skip this logic that is invalid for bad requests
        // Check for existing user
        var existingUser = await dbContext.Users.AnyAsync(e => e.Username == newUserReq.Username);
        if (existingUser)
        {
            // invalid request
            return Results.BadRequest(new { Message = "user already exists" });
        }
        var pwHash = PasswordCrypto.ComputePasswordHash(newUserReq.Password);

        var newUser = new User() { Username = newUserReq.Username, Password = pwHash, Email = newUserReq.Email };
        var userRes = await dbContext.Users.AddAsync(newUser);
        var collectionRes = await dbContext.Collections.AddAsync(new Collection() { User = newUser });
        await dbContext.SaveChangesAsync();

        return Results.Ok(new
            { Success = $"created user with id {userRes.Entity.Id}", Debug = $"collection id: {collectionRes.Entity.Id}" });
    }

    private static async Task<IResult> LoginUser(HttpContext httpContext, [FromServices] PlungerDbContext dbContext,
        [FromServices] JwtConfig jwtConfig, [FromBody] LoginRequest loginRequest)
    {
        var validationRes = loginRequest.Validate();
        if (!validationRes.IsValid)
        {
            // early return
            // invalid request
            return Results.BadRequest(new NewUserResponse() { Messages = validationRes.ValidationErrors });
        }
    
        var userExists = await dbContext.Users.AnyAsync(e =>
            String.Equals(e.Email, loginRequest.Identity) || string.Equals(e.Username, loginRequest.Identity));

        if (!userExists)
        {
            // no user;
            return Results.BadRequest(new { Message = "invalid credentials" });
        }

        var user = loginRequest.Identity.Contains('@')
            ? await dbContext.Users.FirstAsync(e => String.Equals(e.Email, loginRequest.Identity))
            : await dbContext.Users.FirstAsync(e => String.Equals(e.Username, loginRequest.Identity));

        // Compare password
        var success = PasswordCrypto.CheckPassword(loginRequest.Password, user.Password);

        var token = TokenUtils.CreateToken(jwtConfig, user, out var randomString);

        var fingerprintOptions = new CookieOptions()
        {
            HttpOnly = true,
            Secure = true,
            #warning CHANGE IN PRODUCTION
            SameSite = SameSiteMode.None
        };

        httpContext.Response.Cookies.Append(Constants.TokenFingerprint, randomString, fingerprintOptions);
        return Results.Ok(new {UserDetails = new {userId = user.Id, userName = user.Username}, Token = token});
    }

    private static IResult TokenLogin(HttpContext httpContext, [FromServices] PlungerDbContext dbContext)
    {
        var userDetails = TokenUtils.GetUserDetailsFromClaims(httpContext);
        if (userDetails.UserId == "" || userDetails.UserName == "")
        {
            return Results.Problem();
        }

        return Results.Ok(userDetails);
    }
}
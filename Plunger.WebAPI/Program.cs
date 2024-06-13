using System.Security.Claims;
using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Plunger.Common;
using Plunger.Data;
using Plunger.Data.DbModels;
using Plunger.WebApi;
using Plunger.WebApi.DtoModels;
using Plunger.WebApi.Middleware;

var builder = WebApplication.CreateBuilder(args);
var appConnString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<PlungerDbContext>(options => options.UseNpgsql(appConnString));
{
    var jwtConfig = new JwtConfig();
    builder.Configuration.GetSection("Jwt").Bind(jwtConfig);
    builder.Services.AddSingleton(jwtConfig);
}

// var userConnString = builder.Configuration.GetConnectionString("UserDefaultConnection");
// builder.Services.AddDbContext<PlungerUserDbContext>(options => options.UseNpgsql(userConnString));
// builder.Services.AddAuthentication();
// builder.Services.AddAuthorization();

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(o =>
{
    o.MapInboundClaims = false;
    o.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey
            (Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])),
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = false,
        ValidateIssuerSigningKey = true
    };
});
builder.Services.AddAuthorization();

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

var app = builder.Build();
app.UseAuthentication();
app.UseTokenFingerprintMiddleware();
app.UseAuthorization();

app.MapGet("/", () => "Hello World!");

app.MapGet("/testidentity", (HttpContext context) =>
{
    var rawIdentity = context.User.Identity;
    var identity = context.User.Identity as ClaimsIdentity;
    Console.WriteLine(identity);
    if (identity != null)
    {
        IEnumerable<Claim> claims = identity.Claims; 
        // or
        var foo = identity.FindFirst("sub").Value;
        Console.WriteLine(claims);
        return Results.Ok(claims);
    }

    return Results.Problem();
});

app.MapGet("/games/{gameid}", async ([FromRoute] int gameId, [FromServices] PlungerDbContext db) =>
{
    return await db.Games.Where(g => g.Id == gameId).Include(g => g.Platforms).Include(g => g.ReleaseDates).ToListAsync();
});

#warning TODO: Add pagination to results
app.MapGet("/users/{userid}/lists", async ([FromRoute] int userId, [FromServices] PlungerDbContext db) => await db.GameLists.Where(gl => gl.UserId == userId).ToListAsync());

app.MapGet("/users/{userid}/collection", async ([FromRoute] int userId, [FromServices] PlungerDbContext db) => await db.Collections.Select(
    c => new
    {
        c.Id, c.UserId, Games = c.Games.Select(g => new
        {
            g.Id, g.TimeAdded, g.TimeAcquired, g.Physicality, g.GameId, g.PlatformId, g.RegionId, g.VersionId
        })
    }).Where(collection => collection.UserId == userId).ToListAsync());

app.MapPost("/users/{userid}/collection", async (HttpContext httpContext, [FromRoute] int userId, [FromServices] PlungerDbContext db, [FromBody] CollectionAddGameRequest req) =>
{
    var validRes = req.Validate();

    if (!validRes.IsValid)
    {
        return Results.BadRequest(validRes.ValidationErrors);
    }

    {
        // Token fingerprint verification
        var fingerprintHash = IdUtils.GetFingerprint(httpContext.User);
        var fingerprint = httpContext.Request.Cookies[Constants.TokenFingerprint];
        var goodToken = TokenUtils.VerifyTokenFingerprint(fingerprintHash, fingerprint);
        if (!goodToken)
        {
            return Results.BadRequest(new { Error = "Invalid token fingerprint" });
        }
    }

    // Check ownership
    if (!IdUtils.CheckUserOwnership(httpContext.User, userId.ToString()))
    {
        return Results.Unauthorized();
    }

    try
    {
        var collection = await db.Collections.SingleAsync(c => c.UserId == userId);

        var collectionGame = new CollectionGame()
        {
            Collection = collection, GameId = req.GameId, PlatformId = req.PlatformId, RegionId = (int)req.Region,
            Region = req.Region, TimeAdded = DateTimeOffset.UtcNow, TimeAcquired = req.TimeAcquired, Physicality = req.Physicality, VersionId = Guid.NewGuid()
        };

        await db.CollectionGames.AddAsync(collectionGame);
        await db.SaveChangesAsync();

        return Results.Ok(new
        {
            collectionGame.Id, collectionGame.GameId, collectionGame.PlatformId, collectionGame.RegionId,
            collectionGame.TimeAdded, collectionGame.TimeAcquired, collectionGame.Physicality, collectionGame.VersionId
        });
    }
    catch (InvalidOperationException e)
    {
        return Results.Problem();
    }
}).RequireAuthorization();

app.MapPatch("/users/{userid}/collection/{itemid}", async ([FromRoute] int userId, [FromRoute] int itemId, [FromServices] PlungerDbContext db, [FromBody] CollectionGamePatchRequest req) =>
{
    var validRes = req.Validate();

    if (!validRes.IsValid)
    {
        return Results.BadRequest(validRes.ValidationErrors);
    }

    try
    {
        var item = await db.CollectionGames.SingleAsync(cg => cg.Id == itemId);

        if (item.VersionId.CompareTo(req.VersionId) != 0)
        {
            return Results.BadRequest(new { Message = "Incorrect VersionId" });
        }

        item.TimeAcquired = req.TimeAcquired ?? item.TimeAcquired;
        item.Physicality = req.Physicality ?? item.Physicality;
        if (req.GameId != null)
        {
            var game = await db.Games.FindAsync(req.GameId);
            item.GameId = game == null ? (int)req.GameId : item.GameId;
        }
        if (req.PlatformId != null)
        {
            #warning TODO: Check that PlatformID is valid for the game
            var platform = await db.Platforms.FindAsync(req.PlatformId);
            item.PlatformId = platform == null ? (int)req.PlatformId : item.PlatformId;
        }
        if (req.RegionId != null)
        {
            #warning TODO: Check that RegionID is valid for the game
            var region = await db.Regions.FindAsync(req.RegionId);
            item.RegionId = region == null ? (int)req.RegionId : item.RegionId;
        }
        item.VersionId = Guid.NewGuid();

        await db.SaveChangesAsync();
        
        return Results.Ok(item);
    }
    catch (InvalidOperationException e)
    {
        Console.WriteLine(e);
        return Results.Problem();
    }
});

app.MapDelete("/users/{userid}/collection/{itemid}",
    async ([FromRoute] int userId, [FromRoute] int itemId, [FromServices] PlungerDbContext db) =>
    {
        var item = await db.CollectionGames.FindAsync(itemId);
        if (item == null)
        {
            return Results.BadRequest("Invalid itemid");
        }
        db.CollectionGames.Remove(item);
        await db.SaveChangesAsync();

        return Results.Ok();
    });
// app.MapGet("/users/{userid}/nowPlaying", async ([FromRoute] int userId, PlungerDb db) => await );
// app.MapGet("/users/{userid}/playHistory", async ([FromRoute] int userId, PlungerDb db) => await db.);

app.MapPost("/lists/", async ([FromBody] NewListRequest req, [FromServices] PlungerDbContext db) =>
{
    var validation = req.Validate();

    if (!validation.IsValid)
    {
        return Results.BadRequest(validation.ValidationErrors);
    }
    
    #warning TODO: IMPLEMENT REAL USER LOGIC
    var userId = 1;
    
    // Checks for entries that reference a valid, existing game
    var entryIds = req.Entries.Select(e => e.GameId);
    var validGameIds = db.Games.Where(e => entryIds.Contains(e.Id)).Select(e => e.Id);
    var entries = req.Entries.Where(e => validGameIds.Contains(e.GameId)).Select(e => new GameListEntry() { Number = e.Number, GameId = e.GameId });
    var gameListEntries = req.Unordered ? entries.ToList() : entries.OrderBy(e => e.Number).ToList();
    
    var list = new GameList() { Name = req.Name, Unordered = req.Unordered, UserId = userId, GameListEntries = gameListEntries.ToList() };
    await db.GameLists.AddRangeAsync(list);
    var res = await db.SaveChangesAsync();

    return Results.Json(new {Id = list.Id, Name = list.Name});
});

app.MapGet("/lists/{listId}", async ([FromRoute] int listId, [FromServices] PlungerDbContext db) => await db.GameLists.Where(list => list.Id == listId).ToListAsync());

app.MapPatch("/lists/{listId}", async ([FromBody] ListUpdateRequest request, [FromServices] PlungerDbContext dbContext, [FromRoute] int listId) =>
{
    var validation = request.Validate();
    if (!validation.IsValid)
    {
        return Results.BadRequest(validation.ValidationErrors);
    }

    var fetchList = await dbContext.GameLists.Include(e => e.GameListEntries).Where(e => e.Id == listId).ToListAsync();
    if (fetchList.Count == 0)
    {
        return Results.NotFound(new { Message = "List not found" });
    }

    var list = fetchList[0];

    if (request.VersionId.CompareTo(list.VersionId) != 0)
    {
        return Results.BadRequest(new { Message = "Invalid version of the list" });
    }
    
    var error = false;
    request.Updates.ForEach((e) =>
    {
        if (!error)
        {
            switch (e.Action)
            {
                case ListUpdateRequest.ListUpdateAction.ChangeName:
                    list.Name = e.Payload;
                    break;
                case ListUpdateRequest.ListUpdateAction.ChangeOrdered:
                    list.Unordered = Convert.ToBoolean(e.Payload);
                    break;
                case ListUpdateRequest.ListUpdateAction.AddGame:
                    var gameId = Convert.ToInt32(e.Payload);

                    var listEntry = new GameListEntry()
                        { GameId = gameId, GameList = list, Number = list.GameListEntries.Count };
                    list.GameListEntries.Add(listEntry);
                    break;
                case ListUpdateRequest.ListUpdateAction.RemoveGame:
                    var gameNumber = Convert.ToInt32(e.Payload);
                    if (list.GameListEntries.Count <= gameNumber)
                    {
                        error = error || true;
                        break;
                    }
                    list.GameListEntries.RemoveAt(gameNumber);
                    break;
                case ListUpdateRequest.ListUpdateAction.MoveGame:
                    var moveAction = JsonSerializer.Deserialize<ListActionMoveGame>(e.Payload);

                    if (list.GameListEntries.Count <= moveAction.SourceNumber)
                    {
                        error = error || true;
                        break;
                    }

                    if (list.GameListEntries[moveAction.SourceNumber].GameId != moveAction.GameId)
                    {
                        error = error || true;
                        break;
                    }

                    var entry = list.GameListEntries[moveAction.SourceNumber];
                    list.GameListEntries.RemoveAt(moveAction.SourceNumber);
                    list.GameListEntries.Insert(moveAction.DestinationNumber, entry);
                    break;
            }
        }
    });
    if (error)
    {
        return Results.BadRequest(new { Message = "Error processing updates list"});
    }

    list.VersionId = Guid.NewGuid();
    await dbContext.SaveChangesAsync();
    
    return Results.Ok(new { });
});

#warning TODO: ImplementAuthentication & Authorization
#warning TODO: Handle multiple "create" requests for the same game
app.MapPost("/users/{userid}/games/", async ([FromServices] PlungerDbContext dbContext, [FromRoute] int userId, [FromBody] NewGameStatusDto newGameReq) =>
{
    #warning TODO: Check for valid userid
    var user = await dbContext.Users.FindAsync(userId);
    if (user == null)
    {
        // invalid user
        return Results.BadRequest(new {Message = "Invalid user"});
    }
    #warning TODO: Check for valid game
    var game = await dbContext.Games.FindAsync(newGameReq.GameId);
    if (game == null)
    {
        // invalid game
        return Results.BadRequest(new {Message = "Invalid game"});
    }

    var existingGame = await dbContext.GameStatuses.AnyAsync(e => e.UserId == userId && e.GameId == newGameReq.GameId);
    if (existingGame)
    {
        return Results.Conflict(new {Message = $"Game status already exists for {game.Name} ({newGameReq.GameId})"});
    }
    
    var gameId = newGameReq.GameId;
    var completed = newGameReq.Completed ?? false;
    
    if (!newGameReq.PlayState.HasValue)
    {
        #warning TODO: Add logic for invalid playstate
        return Results.BadRequest();
    }
    var status = new GameStatus() { UserId = userId,  GameId = gameId, Completed = completed, PlayState = (int)newGameReq.PlayState.Value, UpdatedAt = newGameReq.TimeStamp };
    var stateChange = new PlayStateChange() { UpdatedAt = newGameReq.TimeStamp, NewState = (int)newGameReq.PlayState.Value, GameStatus = status};
    dbContext.GameStatuses.Add(status);
    dbContext.PlayStateChanges.Add(stateChange);
    await dbContext.SaveChangesAsync();
    
    return Results.Ok(new GameStatusResponse()
    {
        Id = status.Id, UserId = status.UserId, Completed = status.Completed, PlayState = status.PlayState,
        UpdatedAt = status.UpdatedAt, Name = status.Game.Name, ShortName = status.Game.ShortName
    });
});

app.MapGet("/users/{userId}/games/{gameId}", ([FromServices] PlungerDbContext dbContext, [FromRoute] int userId, [FromRoute] int gameId) =>
{
    return dbContext.GameStatuses.Where(e => e.UserId == userId && e.GameId == gameId).OrderByDescending(e => e.UpdatedAt).Select(e => new {Id = e.Id, GameId = e.GameId, UserId = e.UserId, Completed = e.Completed, PlayState = e.PlayState, UpdatedAt = e.UpdatedAt, PlayStateChanges = e.PlayStateChanges});
});

app.MapPatch("/users/{userId}/games/{gameId}",
    async (HttpContext httpContext, [FromServices] PlungerDbContext dbContext, [FromRoute] int userId, [FromRoute] int gameId, [FromBody] UpdateGameStatusDto updateGameReq) =>
{
    // httpContext.User.Identity
    
    var status = await dbContext.GameStatuses.Include(e => e.Game).FirstAsync(e => e.UserId == userId && e.GameId == gameId);
    if (updateGameReq.TimeStamp < status.UpdatedAt)
    {
        return Results.BadRequest(new { Message = "Time stamp older than current time stamp" });
    }

    status.PlayState = (int)updateGameReq.PlayState;
    status.UpdatedAt = updateGameReq.TimeStamp;
    var newStateChange = new PlayStateChange()
        { UpdatedAt = updateGameReq.TimeStamp, NewState = (int)updateGameReq.PlayState, GameStatusId = status.Id };
    await dbContext.PlayStateChanges.AddAsync(newStateChange);
    await dbContext.SaveChangesAsync();
    
    return Results.Ok(new GameStatusResponse()
    {
        Id = status.Id, UserId = status.UserId, Completed = status.Completed, PlayState = status.PlayState,
        UpdatedAt = status.UpdatedAt, Name = status.Game.Name, ShortName = status.Game.ShortName
    });
}).RequireAuthorization();

app.MapPost("/users/", async ([FromServices] PlungerDbContext dbContext, [FromBody] NewUserRequest newUserReq) =>
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
});

app.MapPost("/users/login/", async (HttpContext httpContext, [FromServices] PlungerDbContext dbContext, [FromServices] JwtConfig jwtConfig, [FromBody] LoginRequest loginRequest) =>
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

    var options = new CookieOptions()
    {
        HttpOnly = true,
        Secure = true,
        SameSite = Microsoft.AspNetCore.Http.SameSiteMode.Strict,
        MaxAge = TimeSpan.FromMinutes(10)
    };
    // var cookie = new SetCookieHeaderValue("fingerprint", randomString)
    // {
    //     HttpOnly = true,
    //     Secure = true,
    //     SameSite = SameSiteMode.Strict,
    //     MaxAge = TimeSpan.FromMinutes(10)
    // };

    httpContext.Response.Cookies.Append(Constants.TokenFingerprint, randomString, options);
    return Results.Ok(new {Token = token});
});

app.Run();

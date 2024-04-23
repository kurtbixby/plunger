using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Plunger.Common;
using Plunger.Data;
using Plunger.Data.DbModels;
using Plunger.WebApi.DtoModels;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<PlungerDbContext>(options => options.UseNpgsql(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();
var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.MapGet("/users/{userid}/lists", async ([FromRoute] int userId, PlungerDbContext db) => await db.GameLists.Where(gl => gl.UserId == userId).ToListAsync());
app.MapGet("/users/{userid}/collection", async ([FromRoute] int userId, PlungerDbContext db) => await db.Collections.Where(collection => collection.UserId == userId).ToListAsync());
// app.MapGet("/users/{userid}/nowPlaying", async ([FromRoute] int userId, PlungerDb db) => await );
// app.MapGet("/users/{userid}/playHistory", async ([FromRoute] int userId, PlungerDb db) => await db.);

app.MapGet("/lists/{listid}", async ([FromRoute] int listId, PlungerDbContext db) => await db.GameLists.Where(list => list.Id == listId).ToListAsync());

#warning TODO: ImplementAuthentication & Authorization
app.MapPost("/users/{userid}/games/", async (PlungerDbContext dbContext, [FromRoute] int userId, NewGameStatusDto newGameReq) =>
{
    #warning TODO: Check for valid userid
    #warning TODO: Check for valid game
    var gameId = newGameReq.GameId;
    
    var completed = newGameReq.Completed.HasValue ? newGameReq.Completed.Value : false;
    
    if (!newGameReq.PlayState.HasValue)
    {
        #warning TODO: Add logic for invalid playstate 
    }
    var status = new GameStatus() { UserId = userId,  GameId = gameId, Completed = completed, PlayState = (int)newGameReq.PlayState.Value };
    dbContext.GameStatuses.Add(status);
    return dbContext.SaveChangesAsync();
});

app.MapGet("/users/{userid}/games/{gameid}", async (PlungerDbContext dbContext, [FromRoute] int userId, [FromRoute] int gameId) =>
{
    return dbContext.GameStatuses.Where(e => e.UserId == userId && e.GameId == gameId).OrderByDescending(e => e.Id);
});
app.MapPatch("/users/{userid}/games/{gameid}",
    async (PlungerDbContext dbContext, [FromRoute] int userId, [FromRoute] int gameId, UpdateGameStatusDto updateGameReq) =>
    {
        var status = await dbContext.GameStatuses.FirstAsync(e => e.UserId == userId && e.GameId == gameId);
        if (updateGameReq.TimeStamp < status.UpdatedAt)
        {
            #warning TODO: Handle invalid status update
        }

        status.PlayState = (int)updateGameReq.PlayState;
        status.UpdatedAt = updateGameReq.TimeStamp;
        var newStateChange = new PlayStateChange()
            { UpdatedAt = updateGameReq.TimeStamp, NewState = (int)updateGameReq.PlayState, GameStatusId = status.Id };
        await dbContext.PlayStateChanges.AddAsync(newStateChange);
        return await dbContext.SaveChangesAsync();
    });

app.MapPost("/users/", async (PlungerDbContext dbContext, NewUserRequest newUserReq) =>
{
    #warning TODO: Validate username & password format
    var response = new NewUserResponse() { Messages = new Dictionary<string, string>()};
    
    var validationRes = newUserReq.Validate();
    if (!validationRes.IsValid)
    {
        // early return
        // invalid request
        response.Messages = validationRes.ValidationErrors;
        return response;
    }
    
    #warning TODO: Skip this logic that is invalid for bad requests
    // Check for existing user
    var existingUser = await dbContext.Users.AnyAsync(e => e.Username == newUserReq.Username);
    if (existingUser)
    {
        // invalid request
        response.Messages["username"] = "user already exists";
        return response;
    }
    var pwHash = Utils.ComputePasswordHash(newUserReq.Password);

    var newUser = new User() { Username = newUserReq.Username, Password = pwHash, Email = newUserReq.Email };
    var res = await dbContext.Users.AddAsync(newUser);
    await dbContext.SaveChangesAsync();

    response.Messages["success"] = $"created user with id {res.Entity.Id}";
    return response;
});

app.MapPost("/users/login/", async (PlungerDbContext dbContext, LoginRequest loginRequest) =>
{
    var validationRes = loginRequest.Validate();
    if (!validationRes.IsValid)
    {
        // early return
        // invalid request
        response.Messages = validationRes.ValidationErrors;
        return response;
    }
    
    var userExists = await dbContext.Users.AnyAsync(e =>
        String.Equals(e.Email, loginRequest.Identity) || string.Equals(e.Username, loginRequest.Identity));

    var success = false;
    if (!userExists)
    {
        // no user;
        return new LoginResponse() { Login = success };
    }

    var user = new User();
    if (loginRequest.Identity.Contains('@'))
    {
        // is email
        user = await dbContext.Users.FirstAsync(e => String.Equals(e.Email, loginRequest.Identity));
    }
    else
    {
        // is username
        user = await dbContext.Users.FirstAsync(e => String.Equals(e.Username, loginRequest.Identity));
    }

    // Compare password
    success = Utils.CheckPassword(loginRequest.Password, user.Password);
    return new LoginResponse() { Login = success };
});

app.Run();

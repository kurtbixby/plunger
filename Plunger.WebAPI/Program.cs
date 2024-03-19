using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Plunger.Data;
using Plunger.WebApi.DtoModels;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<PlungerDb>();
builder.Services.AddDatabaseDeveloperPageExceptionFilter();
var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.MapGet("/users/{userid}/lists", async ([FromRoute] int userId, PlungerDb db) => await db.GameLists.Where(gl => gl.UserId == userId).ToListAsync());
app.MapGet("/users/{userid}/collection", async ([FromRoute] int userId, PlungerDb db) => await db.Collections.Where(collection => collection.UserId == userId).ToListAsync());
// app.MapGet("/users/{userid}/nowPlaying", async ([FromRoute] int userId, PlungerDb db) => await );
// app.MapGet("/users/{userid}/playHistory", async ([FromRoute] int userId, PlungerDb db) => await db.);

app.MapGet("/lists/{listid}", async ([FromRoute] int listId, PlungerDb db) => await db.GameLists.Where(list => list.Id == listId).ToListAsync());

app.MapPost("/games/", (NewGameDto newGameReq) => "Add new game");

app.MapPost("/users/", (NewUserDto newUserReq) => "New User");

app.Run();

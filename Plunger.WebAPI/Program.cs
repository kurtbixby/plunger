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
using Plunger.WebApi.Routes;

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

builder.Services.AddCors(options =>
{
    options.AddPolicy("Local",
        policy =>
        {
            // policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
            // policy.SetIsOriginAllowed(origin => new Uri(origin).IsLoopback);
            policy.WithOrigins("http://localhost:5173", "https://localhost:5173").AllowAnyHeader()
                .AllowAnyMethod().AllowCredentials();
        });
    // options.AddDefaultPolicy(
    //     policy =>
    //     {
    //         // policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
    //         // policy.WithOrigins("http://localhost:5173").AllowAnyHeader()
    //         //     .AllowAnyMethod().AllowCredentials();
    //     });
});

var app = builder.Build();
app.UseCors("Local");
app.UseAuthentication();
app.UseTokenFingerprintMiddleware();
app.UseAuthorization();

app.MapGet("/", () => "Hello World!");

app.MapGet("/games/{gameid}", async ([FromRoute] int gameId, [FromServices] PlungerDbContext db) =>
{
    return await db.Games.Where(g => g.Id == gameId).Include(g => g.Platforms).Include(g => g.ReleaseDates).ToListAsync();
});

#warning TODO: Add pagination to results
app.MapGet("/users/{userid}/lists", async ([FromRoute] int userId, [FromServices] PlungerDbContext db) => await db.GameLists.Where(gl => gl.UserId == userId).ToListAsync());

app.MapGroup("/api").MapCollectionRoutes();
app.MapGroup("/api").MapListRoutes();
app.MapGroup("/api").MapGameStateRoutes();
app.MapGroup("/api").MapUsersRoutes();

app.Run();

using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Plunger.Data;
using Plunger.WebApi;
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
    // options.AddPolicy("Local",
    //     policy =>
    //     {
    //         policy.WithOrigins("http://localhost:5173", "https://localhost:5173", "http://localhost", "https://localhost").AllowAnyHeader()
    //             .AllowAnyMethod().AllowCredentials();
    //     });
    
    var frontendUrl = builder.Configuration["FrontendURL"] ?? "https://localhost:5173";
    // options.AddDefaultPolicy(policy =>
    //     {
    //         policy.WithOrigins(frontendUrl).AllowAnyHeader()
    //             .AllowAnyMethod().AllowCredentials();
    //     });
    
    options.AddPolicy("Azure", policy =>
    {
        policy.WithOrigins(frontendUrl).AllowAnyHeader()
            .AllowAnyMethod().AllowCredentials();
    });
});

builder.Services.AddTransient<ILogger>(p =>
{
    var loggerFactory = p.GetRequiredService<ILoggerFactory>();
    return loggerFactory.CreateLogger("SomeLogger");
});

builder.Services.AddHttpLogging(options =>
{
    options.RequestBodyLogLimit = 4096;
    options.ResponseBodyLogLimit = 4096;
});

var app = builder.Build();

app.UseHttpLogging();

app.UseCors("Azure");
// app.UseCors();
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

app.MapGroup("/api").MapGameRoutes();
app.MapGroup("/api").MapInfoRoutes();
app.MapGroup("/api").MapCollectionRoutes();
app.MapGroup("/api").MapListRoutes();
app.MapGroup("/api").MapGameStateRoutes();
app.MapGroup("/api").MapUsersRoutes();

app.Run();

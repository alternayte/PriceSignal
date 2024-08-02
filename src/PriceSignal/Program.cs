using System.Collections.Concurrent;
using System.IdentityModel.Tokens.Jwt;
using Application;
using Application.Common.Interfaces;
using Application.Price;
using Application.Rules;
using Application.Services.Binance;
using Domain.Models.Instruments;
using HotChocolate.Types.Pagination;
using HotChocolate.Utilities;
using Infrastructure;
using Infrastructure.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using PriceSignal.BackgroundServices;
using PriceSignal.Services;

var builder = WebApplication.CreateBuilder(args);
builder.Logging.AddConsole();
// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSpaStaticFiles(configuration =>
{
    configuration.RootPath = "wwwroot";
});

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.WithOrigins("http://localhost:5173") // Adjust the port if necessary
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

builder.Services
    .AddMemoryCache()
    .AddApplication()
    .AddInfrastructure(builder.Configuration, builder.Environment.IsDevelopment())
    .AddGraphQLServer()
    .AddType(new UuidType('D'))
    .AddTypeConverter<Guid,string>(from => from.ToString("D"))
    .AddTypeConverter<string,Guid>(Guid.Parse)
    .SetPagingOptions(new PagingOptions
    {
        MaxPageSize = 500,
        IncludeTotalCount = true,
        DefaultPageSize = 10,
        RequirePagingBoundaries = true

    })
    .InitializeOnStartup()
    .RegisterDbContext<AppDbContext>(DbContextKind.Synchronized)
    .AddQueryType()
    .AddMutationType()
    .AddSubscriptionType()
    .AddInMemorySubscriptions()
    .AddTypes()
    .AddFiltering()
    .AddSorting()
    .AddProjections()
    .UseAutomaticPersistedQueryPipeline()
    .AddInMemoryQueryStorage();

builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<IUser, UserService>();


builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.Authority = "https://securetoken.google.com/nxt-spec";
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidIssuer = "https://securetoken.google.com/nxt-spec",
        ValidateAudience = true,
        ValidAudience = "nxt-spec",
        ValidateLifetime = true
    };
});
builder.Services.AddAuthorization();

if (builder.Configuration.GetSection("Binance:Enabled").Get<bool>())
{
    ConcurrentBag<string> symbols = ["BTCUSDT", "ETHUSDT"];
    builder.Services.AddSingleton(symbols);
    builder.Services.AddHostedService<BinancePairUpdateService>();
    builder.Services.AddSingleton<BinancePriceFetcherService>();
    builder.Services.AddHostedService<BinanceBackfillService>();
    builder.Services.AddHostedService<BinancePriceFetcherService>(provider=>
    {
        var ruleCache = provider.GetRequiredService<RuleCache>();
        var priceHistoryCache = provider.GetRequiredService<PriceHistoryCache>();
        
        
        using var scope = provider.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        var ruleEngine = scope.ServiceProvider.GetRequiredService<RuleEngine>();
        
        ruleCache.LoadRules(dbContext.PriceRules.Where(r=>r.IsEnabled).Include(pr=>pr.Conditions).Include(pr=>pr.Instrument).ToList());
        return provider.GetRequiredService<BinancePriceFetcherService>();
    });
    builder.Services.AddHostedService<BinanceProcessingService>();
}

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<AppDbContext>();
    if (!builder.Environment.IsDevelopment())
    {
        DbSeeder.Initialize(context);
    }
    
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseSpaStaticFiles();
app.UseCors();


var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

var g = app.MapGroup("/api");

g.MapGet("/weatherforecast", () =>
    {
        var forecast = Enumerable.Range(1, 5).Select(index =>
                new WeatherForecast
                (
                    DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                    Random.Shared.Next(-20, 55),
                    summaries[Random.Shared.Next(summaries.Length)]
                ))
            .ToArray();
        return forecast;
    })
    .WithName("GetWeatherForecast")
    .WithOpenApi();

g.MapGet("/weatherforecast/{date}", (string date) =>
    {
        var parsedDate = DateOnly.Parse(date);
        var rng = new Random();
        var forecast = new WeatherForecast
        (
            parsedDate,
            rng.Next(-20, 55),
            summaries[rng.Next(summaries.Length)]
        );
        return forecast;
    })
    .WithName("GetWeatherForecastByDate")
    .WithOpenApi();

g.MapGet("/exchange-info", async (IBinanceApi binanceApi) =>
    {
        var exchangeInfo = await binanceApi.GetTradingPairs();
        var symbols = exchangeInfo.TakeLast(19);
        return symbols;
    })
    .WithName("GetExchangeInfo")
    .WithOpenApi();

g.MapPost("/api/login", async (IUser user, HttpResponse response) =>
{
    var token = user.Name;
    var cookieOptions = new CookieOptions
    {
        HttpOnly = true,
        Secure = true,
        SameSite = SameSiteMode.Strict
    };
    response.Cookies.Append("access_token", token, cookieOptions);
    return Results.Ok();
});

app.UseSpa(spa =>
{
    spa.Options.SourcePath = "wwwroot";
});

app.UseWebSockets();
app.MapGraphQL();

app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int) (TemperatureC / 0.5556);
}
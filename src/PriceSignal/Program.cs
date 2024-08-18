using System.Collections.Concurrent;
using System.IdentityModel.Tokens.Jwt;
using Application;
using Application.Common.Interfaces;
using Application.Price;
using Application.Rules;
using Application.Services.Binance;
using Domain.Models.Instruments;
using Domain.Models.NotificationChannel;
using Domain.Models.User;
using HotChocolate.Types.Pagination;
using HotChocolate.Utilities;
using Infrastructure;
using Infrastructure.Data;
using Infrastructure.PubSub;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using NATS.Client.Core;
using NATS.Client.JetStream;
using NATS.Client.JetStream.Models;
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
    .ModifyOptions(opts =>
    {
        opts.EnableDefer = true;
    })
    .InitializeOnStartup()
    .RegisterDbContext<AppDbContext>(DbContextKind.Pooled)
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
    options.Authority = "https://securetoken.google.com/nxtspec";
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidIssuer = "https://securetoken.google.com/nxtspec",
        ValidateAudience = true,
        ValidAudience = "nxtspec",
        ValidateLifetime = true
    };
    options.Events = new JwtBearerEvents
    {
        OnMessageReceived = context =>
        {
            var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

            if (string.IsNullOrEmpty(token))
            {
                token = context.Request.Cookies["access_token"];
            }

            if (!string.IsNullOrEmpty(token))
            {
                context.Token = token;
            }

            return Task.CompletedTask;
        },
        // OnTokenValidated = context =>
        // {
        //     // Logging to verify if token is validated
        //     Console.WriteLine("Token validated");
        //     return Task.CompletedTask;
        // },
        // OnAuthenticationFailed = context =>
        // {
        //     // Logging if authentication fails
        //     Console.WriteLine($"Authentication failed: {context.Exception.Message}");
        //     return Task.CompletedTask;
        // }
        
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

// app.MapWhen(r=>!r.Request.Path.StartsWithSegments("/api"), appBuilder =>
// {
//     appBuilder.UseSpa(spa=>
//     {
//         spa.Options.SourcePath = "wwwroot";
//     });
// });

app.UseSpa(spa=>
{
    spa.Options.SourcePath = "wwwroot";
});

app.UseWebSockets();
app.MapGraphQL();

g.MapPost("/login", async (IUser user, HttpRequest request, HttpResponse response, IAppDbContext dbContext) =>
{
    if (string.IsNullOrEmpty(user.UserIdentifier))
    {
        return Task.FromResult(Results.BadRequest());
    }
    var existingUser = await dbContext.Users.FindAsync(user.UserIdentifier);
    if (existingUser == null)
    {
        var newUser = new User
        {
            Id = user.UserIdentifier,
            Email = user.Email,
        };
        await dbContext.Users.AddAsync(newUser);
        await dbContext.SaveChangesAsync();
    }
    
    var token = request.Headers["Authorization"].ToString().Replace("Bearer ", "");
    response.Cookies.Append("access_token",token, new CookieOptions
    {
        HttpOnly = true,
        Secure = true,
        SameSite = SameSiteMode.Strict
    });
    return Task.FromResult(Results.Ok());
});


g.MapPost("/logout", async (HttpResponse response) =>
{
    response.Cookies.Delete("access_token");
    return Results.Ok();
});


g.MapPost("/message", async (Messageinput message, IPubSub pubsub) =>
{
    await pubsub.PublishAsync("notifications.test", message);
    return Results.Ok();
});

var natsService = app.Services.GetRequiredService<IPubSub>();

natsService.Subscribe<TelegramInit>("notifications", message =>
{
    using (var scope = app.Services.CreateScope())
    {
        var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        var user = dbContext.Users.FirstOrDefault(u => u.Id == message.User_Id && u.NotificationChannels.FirstOrDefault(nc=>nc.TelegramChatId == message.Chat_Id) == null);
        if (user != null)
        {
            var channel = new UserNotificationChannel
            {
                User = user,
                ChannelType = NotificationChannelType.telegram,
                TelegramChatId = message.Chat_Id,
                TelegramUsername = message.Username
            };
            dbContext.UserNotificationChannels.Add(channel);
            dbContext.SaveChanges();
        }
    }
    Console.WriteLine($"Received message: {message}");
    return Task.CompletedTask;
}, "notifications.init.telegram");

app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int) (TemperatureC / 0.5556);
}

record Messageinput(Int64 Chat_Id, string Message);
record TelegramInit(Int64 Chat_Id, string Username, string User_Id);
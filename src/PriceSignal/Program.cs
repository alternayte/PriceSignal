using System.Collections.Concurrent;
using Application;
using Application.Common.Interfaces;
using Application.Price;
using Application.Rules.Common;
using Domain.Models.NotificationChannel;
using Domain.Models.User;
using HotChocolate.Types.Pagination;
using Infrastructure;
using Infrastructure.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using PriceSignal.BackgroundServices;
using PriceSignal.Services;


try
{
    var builder = WebApplication.CreateBuilder(args);
    builder.Logging.AddConsole();
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();
    builder.Services.AddSpaStaticFiles(configuration => { configuration.RootPath = "wwwroot"; });
    builder.Services.AddCors(options =>
    {
        options.AddDefaultPolicy(policy =>
        {
            policy.WithOrigins("http://localhost:5173","http://localhost:5125") // Adjust the port if necessary
                .AllowAnyHeader()
                .AllowAnyMethod();
        });
    });

    builder.Services
        .AddMemoryCache()
        .AddApplication(builder.Configuration)
        .AddInfrastructure(builder.Configuration, builder.Environment.IsDevelopment())
        .AddGraphQLServer()
        .AddType(new UuidType('D'))
        .AddTypeConverter<Guid, string>(from => from.ToString("D"))
        .AddTypeConverter<string, Guid>(Guid.Parse)
        .SetPagingOptions(new PagingOptions
        {
            MaxPageSize = 500,
            IncludeTotalCount = true,
            DefaultPageSize = 10,
            RequirePagingBoundaries = true
        })
        .ModifyOptions(opts => { opts.EnableDefer = true; })
        .InitializeOnStartup()
        .RegisterDbContext<AppDbContext>(DbContextKind.Resolver)
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
        builder.Services.AddHostedService<BinancePriceFetcherService>(provider =>
        {
            var ruleCache = provider.GetRequiredService<RuleCache>();
            var priceHistoryCache = provider.GetRequiredService<PriceHistoryCache>();


            using var scope = provider.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<IAppDbContext>();
            var ruleEngine = scope.ServiceProvider.GetRequiredService<RuleEngine>();

            ruleCache.LoadRules(dbContext.PriceRules.Where(r => r.IsEnabled).Include(pr => pr.Conditions)
                .Include(pr => pr.Instrument).ToList());
            return provider.GetRequiredService<BinancePriceFetcherService>();
        });
        builder.Services.AddHostedService<BinanceProcessingService>();
    }

    builder.Configuration.AddEnvironmentVariables();

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

    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseHttpsRedirection();
    // app.UseDefaultFiles();
    app.UseStaticFiles();
    // app.UseSpaStaticFiles();
    app.UseCors();
    // app.UseSpa(spa => { spa.Options.SourcePath = "wwwroot"; });
    app.UseRouting();
    app.MapFallbackToFile("index.html");
    app.UseWebSockets();
    app.MapGraphQL();

    var g = app.MapGroup("/api");
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
        response.Cookies.Append("access_token", token, new CookieOptions
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
// g.MapPost("/message", async (Messageinput message, IPubSub pubsub) =>
// {
//     await pubsub.PublishAsync("notifications.test", message);
//     return Results.Ok();
// });

    var natsService = app.Services.GetRequiredService<IPubSub>();

    natsService.Subscribe<TelegramInit>("notifications", message =>
    {
        using (var scope = app.Services.CreateScope())
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            var user = dbContext.Users.FirstOrDefault(u =>
                u.Id == message.User_Id &&
                u.NotificationChannels.FirstOrDefault(nc => nc.TelegramChatId == message.Chat_Id) == null);
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


}
catch (Exception e)
{
    Console.WriteLine(e);
    throw;
}
record Messageinput(Int64 Chat_Id, string Message);

record TelegramInit(Int64 Chat_Id, string Username, string User_Id);
using HotChocolate.Types.Pagination;
using HotChocolate.Utilities;
using Infrastructure;
using Infrastructure.Data;
using Infrastructure.Providers.Binance;
using PriceSignal.BackgroundServices;

var builder = WebApplication.CreateBuilder(args);

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
    .AddAuthorization()
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

if (builder.Configuration.GetSection("Binance:Enabled").Get<bool>())
{
    builder.Services.AddHostedService<BinancePairUpdateService>();
    builder.Services.AddSingleton<BinancePriceFetcherService>();
    builder.Services.AddHostedService<BinancePriceFetcherService>(provider=>provider.GetRequiredService<BinancePriceFetcherService>());
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
using Application.Common.Interfaces;
using Application.Services.Binance;
using Domain.Models.NotificationChannel;
using Infrastructure.Data;
using Infrastructure.Data.Interceptors;
using Infrastructure.Providers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Npgsql;
using Refit;

namespace Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration,
        bool isDevelopment)
    {
        services.AddSingleton(TimeProvider.System);

        string connectionString;
        if (isDevelopment)
        {
            connectionString = configuration.GetConnectionString("PriceSignalDB") ??
                             throw new InvalidOperationException("Connection string not found");
        }
        else
        {
            connectionString = ConvertToNpgsqlConnectionString(File.ReadAllText("/app/secrets/uri"));
        }
        
        var dataSourceBuilder = new NpgsqlDataSourceBuilder(connectionString);
        dataSourceBuilder.MapEnum<NotificationChannelType>();
        var dataSource = dataSourceBuilder.Build();
        services.AddDbContext<IAppDbContext,AppDbContext>((sp,options) =>
        {
            options.AddInterceptors(sp.GetServices<ISaveChangesInterceptor>());
            options.UseNpgsql(dataSource).UseSnakeCaseNamingConvention();
        });
        services.AddScoped<ISaveChangesInterceptor, AuditableEntityInterceptor>();

        
        var binanceWebsocketUrl = configuration.GetSection("Binance:WebsocketUrl").Value ?? throw new InvalidOperationException();
        services.AddSingleton<IWebsocketClientProvider>(sp =>
        {
            var logger = sp.GetRequiredService<ILogger<WebsocketClientProvider>>();
            return new WebsocketClientProvider(binanceWebsocketUrl, logger);
        });
        
        services.AddRefitClient<IBinanceApi>()
            .ConfigureHttpClient(c =>
                c.BaseAddress = new Uri(configuration.GetSection("Binance:ApiUrl").Value ??
                                        throw new InvalidOperationException()))
            .AddPolicyHandler(HttpPolicyExtensions.GetRateLimitPolicy());


        return services;
    }
    
    public static string ConvertToNpgsqlConnectionString(string postgresUri)
    {
        var uri = new Uri(postgresUri);

        string host = uri.Host;
        int port = uri.Port;
        string database = uri.AbsolutePath.Trim('/');
        string username = uri.UserInfo.Split(':')[0];
        string password = uri.UserInfo.Split(':')[1];

        return $"Host={host};Port={port};Database={database};Username={username};Password={password}";
    }
}
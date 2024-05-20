using Application.Common.Interfaces;
using Infrastructure.Data;
using Infrastructure.Data.Interceptors;
using Infrastructure.Providers;
using Infrastructure.Providers.Binance;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Refit;

namespace Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton(TimeProvider.System);

        services.AddScoped<ISaveChangesInterceptor, AuditableEntityInterceptor>();
        services.AddDbContextFactory<AppDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("PriceSignalDB")).UseSnakeCaseNamingConvention());
        

        var binanceWebsocketUrl = configuration.GetSection("Binance:WebsocketUrl").Value ?? throw new InvalidOperationException();

        services.AddSingleton<IWebsocketClientProvider>(sp =>
        {
            var logger = sp.GetRequiredService<ILogger<WebsocketClientProvider>>();
            return new WebsocketClientProvider(binanceWebsocketUrl, logger);
        });
        
        services.AddRefitClient<IBinanceApi>()
            .ConfigureHttpClient(c => c.BaseAddress = new Uri(configuration.GetSection("Binance:ApiUrl").Value ?? throw new InvalidOperationException()));

        return services;
    }
}
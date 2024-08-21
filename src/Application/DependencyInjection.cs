using System.Reflection;
using Application.Common;
using Application.Common.Interfaces;
using Application.Notifications;
using Application.Price;
using Application.Rules;
using Application.Rules.Common;
using Application.Services.Alpaca;
using Application.Services.Binance;
using Domain.Models.PriceRule.Events;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<AlpacaSettings>(configuration.GetSection(AlpacaSettings.Alpaca));
        services.Configure<NatsSettings>(configuration.GetSection(NatsSettings.Nats));
        services.Configure<BinanceSettings>(configuration.GetSection(BinanceSettings.Binance));

        services.AddMediatR(config =>
        {
            config.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
        });
        //services.AddTransient<INotificationHandler<PriceRuleTriggeredEvent>, PriceRuleTriggeredEventHandler>();
        services.AddSingleton<RuleCache>();
        services.AddSingleton<PriceHistoryCache>(_ => new PriceHistoryCache());

        services.AddSingleton(provider =>
        {
            var ruleEngineConfig = new RuleEngineConfig(provider);
            return ruleEngineConfig.CreateSession();
        });
        services.AddScoped<RuleEngine>();
        
        services.AddSingleton<INotificationChannel, NoneNotificationChannel>();
        services.AddSingleton<INotificationChannel, TelegramNotificationChannel>();
        services.AddSingleton<NotificationService>();
        return services;
    }
}
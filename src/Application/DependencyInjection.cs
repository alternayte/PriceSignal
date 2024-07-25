using Application.Common;
using Application.Common.Interfaces;
using Application.Notifications;
using Application.Price;
using Application.Rules;
using Microsoft.Extensions.DependencyInjection;

namespace Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddSingleton<RuleCache>();
        services.AddSingleton<PriceHistoryCache>(_ => new PriceHistoryCache());

        services.AddSingleton(provider =>
        {
            var ruleEngineConfig = new RuleEngineConfig(provider);
            return ruleEngineConfig.CreateSession();
        });
        services.AddSingleton<RuleEngine>();
        
        services.AddSingleton<INotificationChannel, NoneNotificationChannel>();
        services.AddSingleton<INotificationChannel, TelegramNotificationChannel>();
        services.AddSingleton<NotificationService>();
        return services;
    }
}
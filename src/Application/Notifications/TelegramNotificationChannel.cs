using Application.Common.Interfaces;
using Domain.Models.NotificationChannel;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Application.Notifications;

public class TelegramNotificationChannel : INotificationChannel
{
    private readonly IServiceProvider _serviceProvider;
    // private readonly IAppDbContext _context;
    private readonly IPubSub _pubSub;

    public TelegramNotificationChannel(IPubSub pubSub, IServiceProvider serviceProvider)
    {
        _pubSub = pubSub;
        _serviceProvider = serviceProvider;
    }

    public NotificationChannelType ChannelType => NotificationChannelType.telegram;

    public async Task SendAsync(string userId, string message)
    {
        using var scope = _serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<IAppDbContext>();
        var telegramChats = await context.UserNotificationChannels
            .Where(unc => unc.User.Id == userId && unc.ChannelType == ChannelType)
            .Select(unc => unc.TelegramChatId)
            .ToListAsync();

        foreach (var chatId in telegramChats)
        {
            if (chatId == null)
                continue;
            await _pubSub.PublishAsync("notifications.telegram", new Messageinput(chatId.Value, message));
        }
            
        
    }
}

record Messageinput(Int64 Chat_Id, string Message);

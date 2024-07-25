using Application.Common.Interfaces;
using Domain.Models.NotificationChannel;

namespace Application.Notifications;

public class TelegramNotificationChannel : INotificationChannel
{
    public NotificationChannelType ChannelType => NotificationChannelType.telegram;

    public Task SendAsync(string userId, string message)
    {
        throw new NotImplementedException();
        return Task.CompletedTask;
    }
}
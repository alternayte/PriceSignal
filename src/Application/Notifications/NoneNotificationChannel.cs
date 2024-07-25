using Application.Common.Interfaces;
using Domain.Models.NotificationChannel;

namespace Application.Notifications;

public class NoneNotificationChannel : INotificationChannel
{
    public NotificationChannelType ChannelType => NotificationChannelType.none;
    public Task SendAsync(string userId, string message)
    {
        Console.WriteLine(message);
        return Task.CompletedTask;
    }
}
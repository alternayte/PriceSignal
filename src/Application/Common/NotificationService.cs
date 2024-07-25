using Application.Common.Interfaces;
using Domain.Models.NotificationChannel;

namespace Application.Common;

public class NotificationService
{
    private readonly IEnumerable<INotificationChannel> _notificationChannels;

    public NotificationService(IEnumerable<INotificationChannel> notificationChannels)
    {
        _notificationChannels = notificationChannels;
    }

    public async Task SendAsync(string userId, string message, NotificationChannelType type)
    {
        var channel = _notificationChannels.FirstOrDefault(c => c.GetType().Name.StartsWith(type.ToString(), StringComparison.OrdinalIgnoreCase));
        if (channel != null)
        {
            await channel.SendAsync(userId, message);
        }
        else
        {
            throw new ArgumentException("Invalid notification channel type");
        }
    }
}
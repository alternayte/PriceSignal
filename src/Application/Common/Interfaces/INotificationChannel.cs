using Domain.Models.NotificationChannel;

namespace Application.Common.Interfaces;

public interface INotificationChannel
{
    Task SendAsync(string userId, string message);
}
using Domain.Models.NotificationChannel;

namespace Domain.Models.User;

public class UserNotificationChannel : BaseAuditableEntity
{
    public User User { get; set; }
    public NotificationChannelType ChannelType { get; set; }
    public Int64? TelegramChatId { get; set; }
    public string? TelegramUsername { get; set; }
}

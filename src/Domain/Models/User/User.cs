namespace Domain.Models.User;

public class User
{
    public string Id { get; set; }
    public string Email { get; set; }
    public string? StripeCustomerId { get; set; }
    public ICollection<Subscription.Subscription> Subscriptions { get; set; } = new List<Subscription.Subscription>();
    public ICollection<PriceRule.PriceRule> PriceRules { get; set; } = new List<PriceRule.PriceRule>();
    public ICollection<UserNotificationChannel> NotificationChannels { get; set; } = new List<UserNotificationChannel>();
}
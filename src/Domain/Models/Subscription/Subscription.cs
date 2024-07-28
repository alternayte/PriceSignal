using System.Text.Json;

namespace Domain.Models.Subscription;

public class Subscription
{
    public string Id { get; set; }
    public string? Status { get; set; }
    public JsonDocument? Metadata { get; set; }
    public string? PriceId { get; set; }
    public long? Quantity { get; set; }
    public bool? CancelAtPeriodEnd { get; set; }
    public DateTime? CurrentPeriodStart { get; set; }
    public DateTime? CurrentPeriodEnd { get; set; }
    public DateTime? CreatedAt { get; set; }
    public DateTime? EndedAt { get; set; }
    public DateTime? CanceledAt { get; set; }
    public DateTime? CancelAt { get; set; }
    public DateTime? TrialStart { get; set; }
    public DateTime? TrialEnd { get; set; }
    public User.User? User { get; set; }
}
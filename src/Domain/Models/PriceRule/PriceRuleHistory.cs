using System.Text.Json;
using Domain.Models.Instruments;
using Domain.Models.NotificationChannel;

namespace Domain.Models.PriceRule;

public class PriceRuleTriggerLog : BaseAuditableEntity
{
    public decimal? Price { get; set; }
    public decimal? PriceChange { get; set; }
    public decimal? PriceChangePercentage { get; set; }
    public required DateTime TriggeredAt { get; set; }
    public required PriceRule PriceRule { get; set; }
    public required JsonDocument PriceRuleSnapshot { get; set; }
}


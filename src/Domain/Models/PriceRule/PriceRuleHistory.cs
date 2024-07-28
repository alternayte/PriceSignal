using System.Text.Json;
using Domain.Models.Instruments;
using Domain.Models.NotificationChannel;

namespace Domain.Models.PriceRule;

public class PriceRuleTriggerLog : BaseAuditableEntity
{
    public PriceRuleTriggerLog() { }
    public decimal? Price { get; set; }
    public decimal? PriceChange { get; set; }
    public decimal? PriceChangePercentage { get; set; }
    public DateTime TriggeredAt { get;  init; }
    public PriceRule PriceRule { get; init; }
    public JsonDocument PriceRuleSnapshot { get; init; }
    
    public PriceRuleTriggerLog(PriceRule rule)
    {
        PriceRule = rule;
        Price = rule.LastTriggeredPrice;
        TriggeredAt = rule.LastTriggeredAt ?? DateTime.UtcNow;
        PriceRuleSnapshot = JsonSerializer.Deserialize<JsonDocument>(JsonSerializer.Serialize(rule)) ??
                            JsonDocument.Parse("{}");
    }
}


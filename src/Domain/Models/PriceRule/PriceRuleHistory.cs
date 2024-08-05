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
        var opts = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        };

        PriceRule = new PriceRule()
            {Id = rule.Id, Name = rule.Name, Description = rule.Description, InstrumentId = rule.InstrumentId};
        Price = rule.LastTriggeredPrice;
        TriggeredAt = rule.LastTriggeredAt ?? DateTime.UtcNow;
        var snapshot = new PriceRuleSnapshot(rule.Name, rule.Description, rule.NotificationChannel.ToString(), rule.Instrument.Symbol, rule.Conditions.Select(c => new ConditionSnapshot(c.ConditionType, c.Value, c.AdditionalValues)).ToList());
        PriceRuleSnapshot = JsonSerializer.Deserialize<JsonDocument>(JsonSerializer.Serialize(snapshot,opts)) ??
                            JsonDocument.Parse("{}");
    }
}


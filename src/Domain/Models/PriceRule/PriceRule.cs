using System.ComponentModel.DataAnnotations.Schema;
using Domain.Models.Instruments;
using Domain.Models.NotificationChannel;
using Domain.Models.PriceRule.Events;

namespace Domain.Models.PriceRule;

public class PriceRule : BaseAuditableEntity
{
    public required string Name { get; set; }
    public required string Description { get; set; }
    public bool IsEnabled { get; set; }
    public DateTime? LastTriggeredAt { get; set; }
    public decimal? LastTriggeredPrice { get; set; }
    public NotificationChannelType NotificationChannel { get; set; }
    public Instrument Instrument { get; set; }
    public required long InstrumentId { get; set; }
    public ICollection<PriceCondition> Conditions { get; set; } = new List<PriceCondition>();
    public User.User? User { get; set; }
    public string? UserId { get; init; }
    
    public ICollection<PriceRuleTriggerLog> ActivationLogs { get; set; } = new List<PriceRuleTriggerLog>();
    
    [NotMapped]
    public bool HasAttempted { get; set; } = false;
    
    public void Trigger(decimal price)
    {
        LastTriggeredPrice = price;
        LastTriggeredAt = DateTime.UtcNow;
        if (Events.Count != 0) return;
        AddEvent(new PriceRuleTriggeredEvent(this));
        ActivationLogs.Add(new PriceRuleTriggerLog(this));

    }
}


using Domain.Models.Instruments;
using Domain.Models.NotificationChannel;

namespace Domain.Models.PriceRule;

public class PriceRule : BaseAuditableEntity
{
    public required string Name { get; set; }
    public required string Description { get; set; }
    public bool IsEnabled { get; set; }
    public DateTime? LastTriggeredAt { get; set; }
    public NotificationChannelType NotificationChannel { get; set; }
    public Instrument Instrument { get; set; }
    public required long InstrumentId { get; set; }
    public ICollection<PriceCondition> Conditions { get; set; } = new List<PriceCondition>();
}


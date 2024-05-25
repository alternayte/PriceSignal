using Domain.Models.Instruments;

namespace Domain.Models.PriceRule;

public class PriceRule : BaseAuditableEntity
{
    public required string Name { get; set; }
    public required string Description { get; set; }
    public required Instrument Instrument { get; set; }
    public ICollection<PriceCondition> Conditions { get; set; } = new List<PriceCondition>();
}


using System.Text.Json;

namespace Domain.Models.PriceRule;

public class PriceCondition : BaseAuditableEntity
{
    public required string ConditionType { get; set; }
    public decimal Value { get; set; }
    public JsonDocument AdditionalValues { get; set; }
    public PriceRule Rule { get; set; }
}

public enum ConditionType
{
    PricePercentage,
    TechnicalIndicator
}
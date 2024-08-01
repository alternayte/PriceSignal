using System.Text.Json;
using System.Text.Json.Serialization;

namespace Domain.Models.PriceRule;

public class PriceCondition : BaseAuditableEntity
{
    public required string ConditionType { get; set; }
    public decimal Value { get; set; }
    public JsonDocument AdditionalValues { get; set; }
    [JsonIgnore]
    public PriceRule Rule { get; set; }
}

public enum ConditionType
{
    PricePercentage,
    Price,
    PriceAction,
    PriceCrossover,
    TechnicalIndicator,
}
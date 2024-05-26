using System.Text.Json;

namespace Domain.Models.PriceRule;

public class PriceCondition : BaseAuditableEntity
{
    public required string ConditionType { get; set; }
    public decimal Value { get; set; }
    public JsonDocument AdditionalValues { get; set; }
    public PriceRule Rule { get; set; }
}

public class AdditionalValue
{
    public string? Name { get; set; }
    public string? Period { get; set; }
    public string? Value { get; set; }
    public string? Type { get; set; }
}
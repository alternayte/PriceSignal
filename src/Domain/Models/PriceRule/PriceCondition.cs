namespace Domain.Models.PriceRule;

public class PriceCondition : BaseAuditableEntity
{
    public required string ConditionType { get; set; }
    public decimal Value { get; set; }
    public string? AdditionalValue { get; set; }
    public required PriceRule Rule { get; set; }
}
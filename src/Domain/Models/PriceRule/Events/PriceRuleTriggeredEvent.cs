namespace Domain.Models.PriceRule.Events;

public class PriceRuleTriggeredEvent(PriceRule rule) : BaseEvent
{
 public PriceRule Rule { get; set; } = rule;
}
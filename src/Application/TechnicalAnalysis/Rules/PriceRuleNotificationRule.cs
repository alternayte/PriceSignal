using System.Text.Json;
using Application.Common;
using Application.Common.Interfaces;
using Application.Price;
using Domain.Models;
using Domain.Models.PriceRule;
using Domain.Models.PriceRule.Events;
using Microsoft.Extensions.Logging;
using NRules.Fluent.Dsl;
using NRules.RuleModel;

namespace Application.TechnicalAnalysis.Rules;

public class PriceRuleNotificationRule : Rule
{
    private readonly ILogger<PriceRuleNotificationRule> _logger;

    public PriceRuleNotificationRule(ILogger<PriceRuleNotificationRule> logger)
    {
        _logger = logger;
    }
    
    public PriceRuleNotificationRule()
    {
    }

    public override void Define()
    {
        Domain.Models.PriceRule.PriceRule priceRule = null;

        When()
            .Match<Domain.Models.PriceRule.PriceRule>(() => priceRule)
            .Exists<Domain.Models.PriceRule.PriceRule>(r => r.HasAttempted);

        Then()
            .Do(ctx => NotifyRuleTrigger(priceRule));
    }
    
    private void NotifyRuleTrigger(Domain.Models.PriceRule.PriceRule rule)
    {
        _logger.LogInformation("Price rule triggered: {Rule}", rule.Name);
    }
}
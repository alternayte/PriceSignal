using System.Text.Json;
using Application.Common;
using Application.Common.Interfaces;
using Application.Price;
using Domain.Models.PriceRule;
using NRules.Fluent.Dsl;
using NRules.RuleModel;

namespace Application.TechnicalAnalysis.Rules;

public class TechnicalAnalysisRule : Rule
{
    private readonly TechnicalAnalysisFactory _taFactory = new();

    public override void Define()
    {
        IPrice price = null;
        PriceRule priceRule = null;

        When()
            .Match<IPrice>(() => price)
            .Match<PriceRule>(() => priceRule, r => r.Instrument.Symbol == price.Symbol && r.IsEnabled && r.DeletedAt == null);

        Then()
            .Do(ctx => EvaluateConditions(ctx, price, priceRule));
    }
    
    private void EvaluateConditions(IContext ctx, IPrice price, PriceRule rule)
    {
        var priceHistoryCache = ctx.Resolve<PriceHistoryCache>();
        var notificationService = ctx.Resolve<NotificationService>();

        var allConditionsMet = true;
        
        foreach (var condition in rule.Conditions)
        {
            bool conditionMet;

            switch (condition.ConditionType)
            {
                case nameof(ConditionType.TechnicalIndicator):
                    var indicatorInputs = JsonSerializer.Deserialize<Dictionary<string, string>>(condition.AdditionalValues.RootElement.GetRawText());
                    var indicatorName = indicatorInputs["Name"];
                    var threshold = condition.Value;

                    var indicator = _taFactory.GetIndicator(indicatorName);
                    var prices = priceHistoryCache.GetPriceHistory(price.Symbol);
                    var value = indicator.Calculate(prices, indicatorInputs);
                    
                    conditionMet = value > threshold;
                    break;
                default:
                    conditionMet = false;
                    break;
            }


            if (!conditionMet)
            {
                allConditionsMet = false;
                break;
            }
            if (allConditionsMet)
            {
                TriggerAlert(price, rule, notificationService);
            }
        }
    }

    private void TriggerAlert(IPrice price, PriceRule rule, NotificationService notificationService)
    {
        Task.FromResult(notificationService.SendAsync("1234", $"Rule triggered: {rule.Name} for {price.Symbol}.",
            rule.NotificationChannel));
    }
}
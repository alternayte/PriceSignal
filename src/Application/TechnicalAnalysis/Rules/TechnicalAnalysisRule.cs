using System.Text.Json;
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
            .Match<PriceRule>(() => priceRule, r => r.Instrument.Symbol == price.Symbol &&
                                                    r.Conditions.Any(c => c.ConditionType == "TechnicalIndicator"));

        Then()
            .Do(ctx => EvaluateTechnicalIndicators(ctx, price, priceRule));
    }
    
    private void EvaluateTechnicalIndicators(IContext ctx, IPrice price, PriceRule rule)
    {
        var priceHistoryCache = ctx.Resolve<PriceHistoryCache>();
        
        var conditions = rule.Conditions.Where(c => c.ConditionType == "TechnicalIndicator");
        foreach (var condition in conditions)
        {
            var indicatorInputs = JsonSerializer.Deserialize<Dictionary<string, string>>(condition.AdditionalValues.RootElement.GetRawText());
            var indicatorName = indicatorInputs["Name"];
            var threshold = condition.Value;

            var indicator = _taFactory.GetIndicator(indicatorName);
            var prices = priceHistoryCache.GetPriceHistory(price.Symbol);
            var value = indicator.Calculate(prices, indicatorInputs);

            if (value <= threshold) continue;
            TriggerAlert(price, rule);
            break;
        }
    }

    private void TriggerAlert(IPrice price, PriceRule rule)
    {
        // Logic to trigger alert
        Console.WriteLine($"Rule triggered: {rule.Name} for {price.Symbol}.");
    }
}
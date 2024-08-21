using Application.Price;
using Application.TechnicalAnalysis;
using Domain.Models.PriceRule;
using NRules.Fluent.Dsl;
using NRules.RuleModel;

namespace Application.Rules;

public class TechnicalAnalysisRule : Rule
{
    private readonly TechnicalAnalysisFactory _taFactory = new();
    public PriceHistoryCache _priceHistoryCache;
    
    public TechnicalAnalysisRule(PriceHistoryCache priceHistoryCache)
    {
        _priceHistoryCache = priceHistoryCache;
    }
    
    public TechnicalAnalysisRule()
    {
    }

    public override void Define()
    {
        IPrice price = null;
        PriceRule priceRule = null;

        When()
            .Match<IPrice>(() => price)
            .Exists<PriceRule>(pr => !pr.HasAttempted)
            .Match<PriceRule>(() => priceRule, r => r.Instrument.Symbol == price.Symbol && r.IsEnabled && r.DeletedAt == null && !r.HasAttempted);

        Then()
            .Do(ctx => EvaluateConditions(ctx, price, priceRule))
            .Do(ctx => ctx.Update(priceRule));
    }
    
    private void EvaluateConditions(IContext ctx, IPrice price, PriceRule rule)
    {
        try
        {
            _priceHistoryCache = ctx.Resolve<PriceHistoryCache>();    
        }
        catch (Exception e)
        {
            // ignored
        }


        var allConditionsMet = true;
        
        foreach (var condition in rule.Conditions)
        {
            bool conditionMet;
            var threshold = condition.Value;
            var metadata = condition.AdditionalValues.RootElement;
            var direction = metadata.GetProperty("direction").GetString();


            switch (condition.ConditionType)
            {
                case nameof(ConditionType.TechnicalIndicator):
                    var indicatorName = metadata.GetProperty("name").GetString();

                    var indicator = _taFactory.GetIndicator(indicatorName);
                    var prices = _priceHistoryCache.GetPriceHistory(price.Symbol);
                    var value = indicator.Calculate(prices, metadata);
                    
                    conditionMet = direction switch
                    {
                        "Above" => value >= threshold,
                        "Below" => value <= threshold,
                        _ => false
                    };
                    break;
                case nameof(ConditionType.Price):
                    conditionMet = direction switch
                    {
                        "Above" => price.Close >= threshold,
                        "Below" => price.Close <= threshold,
                        _ => false
                    };
                    break;
                case nameof(ConditionType.PricePercentage):
                    var previousPrice = _priceHistoryCache.GetPriceHistory(price.Symbol).LastOrDefault();
                    if (previousPrice == null)
                    {
                        conditionMet = false;
                        break;
                    }
                    var percentageChange = (price.Close - previousPrice.Close) / previousPrice.Close * 100;
                    
                    conditionMet = direction switch
                    {
                        "Above" => percentageChange >= threshold,
                        "Below" => percentageChange <= threshold,
                        _ => false
                    };
                    break;
                case nameof(ConditionType.PriceCrossover):
                    var previousPriceCrossover = _priceHistoryCache.GetPriceHistory(price.Symbol).SkipLast(1).LastOrDefault();
                    if (previousPriceCrossover == null)
                    {
                        conditionMet = false;
                        break;
                    }
                    conditionMet = direction switch
                    {
                        "Above" => previousPriceCrossover.Close < threshold && price.Close >= threshold,
                        "Below" => previousPriceCrossover.Close > threshold && price.Close <= threshold,
                        _ => false
                    };
                    break;
                default:
                    conditionMet = false;
                    break;
            }

            if (conditionMet) continue;
            allConditionsMet = false;
            break;
        }
        if (allConditionsMet)
        {
            rule.HasAttempted = true;
        }
    }
}
using System.Text.Json;
using Application.Common;
using Application.Common.Interfaces;
using Application.Price;
using Domain.Models;
using Domain.Models.PriceRule;
using Domain.Models.PriceRule.Events;
using NRules.Fluent.Dsl;
using NRules.RuleModel;

namespace Application.TechnicalAnalysis.Rules;

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
        Domain.Models.PriceRule.PriceRule priceRule = null;

        When()
            .Match<IPrice>(() => price)
            .Exists<Domain.Models.PriceRule.PriceRule>(pr => !pr.HasAttempted)
            .Match<Domain.Models.PriceRule.PriceRule>(() => priceRule, r => r.Instrument.Symbol == price.Symbol && r.IsEnabled && r.DeletedAt == null && !r.HasAttempted);

        Then()
            .Do(ctx => EvaluateConditions(ctx, price, priceRule))
            .Do(ctx => ctx.Update(priceRule));
    }
    
    private void EvaluateConditions(IContext ctx, IPrice price, Domain.Models.PriceRule.PriceRule rule)
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
            var metadata = JsonSerializer.Deserialize<Dictionary<string, string>>(condition.AdditionalValues.RootElement.GetRawText());
            var direction = metadata["direction"];


            switch (condition.ConditionType)
            {
                case nameof(ConditionType.TechnicalIndicator):
                    var indicatorName = metadata["name"];

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


            rule.HasAttempted = true;
            if (!conditionMet)
            {
                allConditionsMet = false;
                break;
            }
            if (allConditionsMet)
            {
                rule.Trigger(price.Close);
            }
        }
    }

    private void TriggerAlert(IPrice price, Domain.Models.PriceRule.PriceRule rule, NotificationService notificationService)
    {
        rule.Trigger(price.Close);
        Task.FromResult(notificationService.SendAsync("1234", $"Rule triggered: {rule.Name} for {price.Symbol}.",
            rule.NotificationChannel));
    }
}
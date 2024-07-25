using Application.Price;
using Domain.Models.PriceRule;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.Extensions.Logging;
using NRules;
using Skender.Stock.Indicators;

namespace Application.Rules;

public class RuleEngine(RuleCache ruleCache, PriceHistoryCache priceHistoryCache, ILogger<RuleEngine> logger, ISession session)
{
    public void EvaluateRules(IPrice price)
    {
        priceHistoryCache.AddPrice(price.Symbol, price);
        session.Insert(price);
        
        var rules = ruleCache.GetAllRules().Where(r => r.Instrument.Symbol == price.Symbol).ToList();

        foreach (var rule in rules)
        {
            session.Insert(rule);
        }

        session.Fire();

        session.Retract(price);
        foreach (var rule in rules)
        {
            session.Retract(rule);
        }
    }

    private bool EvaluateConditions(IPrice price, PriceRule rule)
    {
        var priceHistory = priceHistoryCache.GetPriceHistory(price.Symbol).ToList();
        if (priceHistory.Count == 0) return false;

        foreach (var condition in rule.Conditions)
        {
            switch (condition.ConditionType)
            {
                case "PricePercentage":
                {
                    var additionalData = JsonSerializer.Deserialize<Dictionary<string, string>?>(condition.AdditionalValues);
                    var direction = additionalData["Direction"];
                    var percentage = condition.Value;
                    var timeWindowHours = Convert.ToInt32(additionalData["TimeWindow"]);

                    var pastPrice = priceHistory
                        .Where(p => p.Date >= DateTime.Now.AddHours(-timeWindowHours)).MinBy(p => p.Date);
                    if (pastPrice == null) return false;

                    switch (direction)
                    {
                        case "up" when (price.Close / pastPrice.Close - 1) * 100 < percentage:
                        case "down" when (1 - price.Close / pastPrice.Close) * 100 < percentage:
                            return false;
                    }

                    break;
                }
                case "TechnicalIndicator":
                {
                    var indicatorInputs = JsonSerializer.Deserialize<Dictionary<string, string>?>(condition.AdditionalValues);
                    var indicatorName = indicatorInputs["Name"];
                    var threshold = condition.Value;

                    if (indicatorName == "RSI")
                    {
                        var period = int.Parse(indicatorInputs["Period"]);
                        if (priceHistory.Count < period +1 ) return false;
                        var rsi = priceHistory.GetRsi(period).Last().Rsi;
                        // var rsi = CalculateRSI(priceHistory, period);
                        if (rsi <= (double?) threshold)
                        {
                            return false;
                        }
                        else
                        {
                            logger.LogInformation($"RSI for {price.Symbol} is {rsi}");
                        }
                    }

                    break;
                }
            }
            // Add additional condition types as needed
        }
        //logger.LogInformation($"Rule {rule.Name} triggered for {price.Symbol} at {price.Date} with price {price.Close} and conditions {string.Join(", ", rule.Conditions.Select(c => c.ConditionType))}");
        return true;
    }

    private decimal CalculateRSI(List<Domain.Models.Instruments.Price> prices, int period)
    {
        // Implement RSI calculation using historical price data
        // This is a simplified example
        if (prices.Count < period+1) return 0;

        var gains = 0.0m;
        var losses = 0.0m;
        for (int i = 1; i <= period; i++)
        {
            var change = prices[prices.Count - i].Close - prices[prices.Count - i - 1].Close;
            if (change > 0)
            {
                gains += change;
            }
            else
            {
                losses -= change;
            }
        }

        var avgGain = gains / period;
        var avgLoss = losses / period;
        if (avgLoss == 0) return 100;
        var rs = avgGain / avgLoss;
        if (rs == 0) return 0;

        return 100 - (100 / (1 + rs));
    }
}
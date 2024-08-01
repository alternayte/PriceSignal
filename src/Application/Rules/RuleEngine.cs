using Application.Price;
using Domain.Models.PriceRule;
using System.Text.Json;
using System.Text.Json.Serialization;
using Application.Common.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NRules;
using Skender.Stock.Indicators;

namespace Application.Rules;

public class RuleEngine(RuleCache ruleCache, PriceHistoryCache priceHistoryCache, ILogger<RuleEngine> logger, ISession session, IServiceProvider serviceProvider)
{
    public void EvaluateRules(IPrice price)
    {
        priceHistoryCache.AddPrice(price.Symbol, price);

        session.Insert(price);
        
        
        var rules = ruleCache.GetAllRules().Where(r => r.Instrument.Symbol == price.Symbol).ToList();

        session.InsertAll(rules);
        // foreach (var rule in rules)
        // {
        //     session.Insert(rule);
        // }

        session.Fire();

        session.Retract(price);
        session.RetractAll(rules);
        // foreach (var rule in rules)
        // {
        //     session.Retract(rule);
        // }
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
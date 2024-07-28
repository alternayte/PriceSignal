using System.Text.Json;
using Application.Price;
using Application.Rules;
using Application.TechnicalAnalysis.Rules;
using Application.UnitTests.Common;
using Domain.Models.PriceRule;
using Microsoft.Extensions.Logging;
using NRules.Testing;
using Xunit.Abstractions;

namespace Application.UnitTests.PriceRules.Handlers;

public class RuleEvaluationTests : BaseRulesTestFixture
{
    private readonly ITestOutputHelper _output;
    private PriceHistoryCache _priceHistoryCache = new();

    public RuleEvaluationTests(ITestOutputHelper output)
    {
        _output = output;
        var priceHistoryCache = new PriceHistoryCache();
        var closingPrices = new List<decimal>
        {
            10500m, 
            10500m, // 14 periods ago
            10450m,
            10550m,
            10600m,
            10700m,
            10650m,
            10750m,
            10800m,
            10850m,
            10900m,
            10950m,
            11000m,
            11050m,
            11100m  // Most recent period
        };
        
        for (int i = 0; i < closingPrices.Count; i++)
        {
            var close = closingPrices[i];
            var open = close - 100; 
            var high = close + 100; 
            var low = close - 200;  
            priceHistoryCache.AddPrice("BTCUSDT", new PriceQuote(new Domain.Models.Instruments.Price()
            {
                Bucket = new DateTimeOffset(DateTime.Now.AddHours(-i)),
                Symbol = "BTCUSDT",
                Open = open,
                High = high,
                Low = low,
                Close = close
            }));
        }
        _priceHistoryCache = priceHistoryCache;
        var logger = new Logger<PriceRuleNotificationRule>(new LoggerFactory());
        Setup.Rule(new TechnicalAnalysisRule(priceHistoryCache));
        Setup.Rule(new PriceRuleNotificationRule(logger));
        // Setup.Rule<PriceRuleNotificationRule>();
    }

    [Fact]
    public void Test1()
    {
        // Arrange
        IPrice price = new PriceQuote(new Domain.Models.Instruments.Price()
        {
            Bucket = new DateTimeOffset(DateTime.Now),
            Symbol = "BTCUSDT",
            Open = 10000,
            High = 11000,
            Low = 9000,
            Close = 10500,
        });

        var rule = new Domain.Models.PriceRule.PriceRule
        {
            Name = "Test Rule",
            Description = "Test Rule Description",
            IsEnabled = true,
            InstrumentId = 1,
            Instrument = new Domain.Models.Instruments.Instrument
            {
                Symbol = "BTCUSDT",
                Name = null,
                BaseAsset = null,
                QuoteAsset = null,
            },
            Conditions = new List<PriceCondition>()
            {
                new()
                {
                    ConditionType = ConditionType.TechnicalIndicator.ToString(),
                    Value = 30,
                    AdditionalValues = JsonDocument.Parse("{\"direction\": \"Below\", \"name\": \"RSI\", \"period\": \"14\"}"),
                }
            }
        };
        
        
        Session.Insert(price);
        Session.Insert(rule);

        // Act
        Session.Fire();

        // Assert
        Verify(x =>
        {
            x.Rule<TechnicalAnalysisRule>().Fired(Times.Once);
            x.Rule<PriceRuleNotificationRule>().Fired(Times.Once);
        });
    }
}
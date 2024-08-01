using System.Text.Json;
using Application.Common.Interfaces;
using Application.Price;
using Skender.Stock.Indicators;

namespace Application.TechnicalAnalysis.Indicators;

public class RSI : ITechnicalIndicator
{
    public decimal Calculate(IEnumerable<IPrice> prices, JsonElement inputs)
    {
        var period = inputs.GetProperty("period").GetInt32();
        var results = prices.GetRsi(period);
        var rsi = results.Last().Rsi;
        if (rsi == null) return 0;
        return (decimal) rsi;
    }

    public string Name => "RSI";
}
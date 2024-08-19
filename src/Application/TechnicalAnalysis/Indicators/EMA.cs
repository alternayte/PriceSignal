using System.Text.Json;
using Application.Common.Interfaces;
using Application.Price;
using Skender.Stock.Indicators;

namespace Application.TechnicalAnalysis.Indicators;

public class EMA: ITechnicalIndicator
{
    public decimal Calculate(IEnumerable<IPrice> prices, JsonElement inputs)
    {
        var period = inputs.GetProperty("period").GetInt32();
        var results = prices.GetEma(period);
        var value = results.Last().Ema;
        if (value == null) return 0;
        return (decimal) value;
    }

    public string Name => "EMA";
}
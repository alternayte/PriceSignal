using Application.Common.Interfaces;
using Application.Price;
using Skender.Stock.Indicators;

namespace Application.TechnicalAnalysis.Indicators;

public class RSI : ITechnicalIndicator
{
    public decimal Calculate(IEnumerable<IPrice> prices, Dictionary<string, string> inputs)
    {
        var period = int.Parse(inputs["period"]);
        var results = prices.GetRsi(period);
        var rsi = results.Last().Rsi;
        if (rsi == null) return 0;
        return (decimal) rsi;
    }

    public string Name => "RSI";
}
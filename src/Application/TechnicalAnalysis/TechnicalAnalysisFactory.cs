using Application.Common.Interfaces;

namespace Application.TechnicalAnalysis;

public class TechnicalAnalysisFactory
{
    private readonly Dictionary<string, ITechnicalIndicator> _indicators = new()
    {
        {"RSI", new Indicators.RSI()}
    };

    public ITechnicalIndicator GetIndicator(string name)
    {
        if (_indicators.TryGetValue(name, out var indicator))
        {
            return indicator;
        }

        throw new ArgumentException($"Technical indicator '{name}' is not supported.");
    }

}
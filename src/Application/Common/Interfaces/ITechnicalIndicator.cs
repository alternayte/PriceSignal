using Application.Price;

namespace Application.Common.Interfaces;

public interface ITechnicalIndicator
{
    decimal Calculate(IEnumerable<IPrice> prices, Dictionary<string,string> inputs);
    string Name { get; }
}
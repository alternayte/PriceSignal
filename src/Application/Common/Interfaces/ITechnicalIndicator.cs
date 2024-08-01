using System.Text.Json;
using Application.Price;

namespace Application.Common.Interfaces;

public interface ITechnicalIndicator
{
    decimal Calculate(IEnumerable<IPrice> prices, JsonElement inputs);
    string Name { get; }
}
using System.Collections.Concurrent;
using Application.Common;

namespace Application.Price;

public class PriceHistoryCache
{
    private readonly ConcurrentDictionary<string, FixedSizeQueue<IPrice>> _priceHistory = new();
    private readonly int _maxSize;

    public PriceHistoryCache(int maxSize = 500)
    {
        _maxSize = maxSize;
    }

    public IEnumerable<IPrice> GetPriceHistory(string symbol)
    {
        if (_priceHistory.TryGetValue(symbol, out var history))
        {
            return history.ToArray();
        }

        return new List<IPrice>();
    }

    public void AddPrice(string symbol, IPrice price)
    {
        var history = _priceHistory.GetOrAdd(symbol, new FixedSizeQueue<IPrice>(_maxSize));
        history.Enqueue(price);
    }

    public void LoadPriceHistory(string symbol, List<IPrice> history)
    {
        var fixedSizeQueue = new FixedSizeQueue<IPrice>(_maxSize);
        foreach (var price in history)
        {
            fixedSizeQueue.Enqueue(price);
        }
        _priceHistory[symbol] = fixedSizeQueue;
    }
}
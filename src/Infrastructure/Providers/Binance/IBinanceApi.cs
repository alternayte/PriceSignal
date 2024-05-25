using Domain.Models.Instruments;
using Infrastructure.Providers.Binance.Models;
using Refit;

namespace Infrastructure.Providers.Binance;

public interface IBinanceApi
{
    [Get("/api/v3/exchangeInfo")]
    internal Task<IApiResponse<ExchangeInfo>> GetExchangeInfoAsync(CancellationToken cancellationToken = default);
    
    public async Task<IEnumerable<Instrument>> GetTradingPairs(CancellationToken cancellationToken = default)
    {
        var response = await GetExchangeInfoAsync(cancellationToken);
        if (response is null) throw new Exception("Failed to get exchange info");
        var instruments = response.Content?.symbols.Where(s=>s is {quoteAsset: "USDT", isSpotTradingAllowed: true, status:"TRADING"}).Select(s => new Instrument
        {
            Name = s.baseAsset,
            Symbol = s.symbol,
            BaseAsset = s.baseAsset,
            QuoteAsset = s.quoteAsset,
        }).ToList();
        return instruments ?? [];
    }
}
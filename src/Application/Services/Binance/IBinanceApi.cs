using System.Text.Json.Nodes;
using Application.Services.Binance.Models;
using Domain.Models.Instruments;
using Refit;

namespace Application.Services.Binance;

public interface IBinanceApi
{
    [Get("/api/v3/exchangeInfo")]
    internal Task<IApiResponse<ExchangeInfo>> GetExchangeInfoAsync(CancellationToken cancellationToken = default);
    
    public async Task<IEnumerable<Instrument>> GetTradingPairs(CancellationToken cancellationToken = default)
    {
        var response = await GetExchangeInfoAsync(cancellationToken);
        if (response is null || !response.IsSuccessStatusCode) throw new Exception("Failed to get exchange info");
        var instruments = response.Content?.symbols.Where(s=>s is {quoteAsset: "USDT", isSpotTradingAllowed: true, status:"TRADING"}).Select(s => new Instrument
        {
            Name = s.baseAsset,
            Symbol = s.symbol,
            BaseAsset = s.baseAsset,
            QuoteAsset = s.quoteAsset,
        }).ToList();
        return instruments ?? [];
    }


    [Get("/api/v3/aggTrades?limit=1000")]
    internal Task<IApiResponse<IEnumerable<AggTrade>>> GetAggTradesAsync(string symbol, long? startTime, long? endTime,
        CancellationToken cancellationToken = default);
    
    public async Task<IEnumerable<InstrumentPrice>> GetHistoricalPricesAsync(AggTradeParams @params, CancellationToken cancellationToken = default)
    {
        var response = await GetAggTradesAsync(@params.Symbol,@params.StartTime,@params.EndTime, cancellationToken);
        if (response is null) throw new Exception("Failed to get exchange info");
        var result = response.Content;
        
        var prices = result.Select(t => new InstrumentPrice()
        {
            Price = decimal.Parse(t.p),
            Symbol = @params.Symbol,
            Volume = decimal.Parse(t.q),
            Quantity = decimal.Parse(t.q),
            Timestamp = DateTimeOffset.FromUnixTimeMilliseconds(t.T)
        });
        
        return prices;
    }
    
    
    [Get("/api/v3/klines?limit=1000")]
    internal Task<IApiResponse<IEnumerable<object>>> GetKlinesAsync(string symbol, string interval, long? startTime, long? endTime,
        CancellationToken cancellationToken = default);

    public async Task<IEnumerable<InstrumentPrice>> GetHistoricalCandlesAsync(KlineParams @params,
        CancellationToken cancellationToken = default)
    {
        var response = await GetKlinesAsync(@params.Symbol,@params.Interval.ToApiString(),@params.StartTime,@params.EndTime, cancellationToken);
        if (response is null || !response.IsSuccessStatusCode) throw new Exception("Failed to get candle data");
        var result = response.Content;
        var prices = new List<InstrumentPrice>();
        foreach (var item in result)
        {
            var data = JsonNode.Parse(item.ToString()).AsArray();
            
            var price = new InstrumentPrice
            {
                Symbol = @params.Symbol,
                Timestamp = DateTimeOffset.FromUnixTimeMilliseconds(data[0].GetValue<long>()),
                Price = decimal.Parse(data[4].GetValue<string>()),
                Volume = decimal.Parse(data[5].GetValue<string>()),
                Quantity = data[8].GetValue<long>(),
            };
            prices.Add(price);
        }
        return prices;
    }
}
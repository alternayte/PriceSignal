using Infrastructure.Providers.Binance.Models;
using Refit;

namespace Infrastructure.Providers.Binance;

public interface IBinanceApi
{
    [Get("/api/v3/exchangeInfo")]
    Task<IApiResponse<ExchangeInfo>> GetExchangeInfoAsync(CancellationToken cancellationToken = default);
}
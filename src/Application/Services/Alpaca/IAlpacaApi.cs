using Application.Services.Alpaca.Models;
using Refit;

namespace Application.Services.Alpaca;

public interface IAlpacaApi
{
    [Get("/v1beta1/news?sort=desc")]
    internal Task<IApiResponse<NewsResponse>> GetAlpacaNewsAsync(CancellationToken cancellationToken = default);
    
    public async Task<IEnumerable<News>> GetNewsAsync(CancellationToken cancellationToken = default)
    {
        var response = await GetAlpacaNewsAsync(cancellationToken);
        if (response is null || !response.IsSuccessStatusCode) throw new Exception("Failed to get exchange info");
        return response.Content?.news ?? [];
    }
}
using Application.Services.Alpaca;
using Application.Services.Alpaca.Models;

namespace PriceSignal.GraphQL.Queries;

[QueryType]
public class NewsQueries
{
    public async Task<IEnumerable<News>> GetNews([Service] IAlpacaApi alpacaApi, CancellationToken cancellationToken)
    {
        return await alpacaApi.GetNewsAsync(cancellationToken);
    }
}
using Domain.Models.Exchanges;
using HotChocolate.Resolvers;
using Infrastructure.Data;
using PriceSignal.GraphQL.Types;

namespace PriceSignal.GraphQL.Queries;

[QueryType]
public class ExchangeQueries
{

    [UseOffsetPaging(IncludeTotalCount = true)]
    [UseProjection]
    [UseFiltering]
    [UseSorting]
    public IQueryable<Exchange> GetExchanges(AppDbContext dbContext)
    {
        return dbContext.Exchanges.AsQueryable();
    }
}
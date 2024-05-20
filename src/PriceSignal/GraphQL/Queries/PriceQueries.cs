using Domain.Models.Instruments;
using Infrastructure.Data;

namespace PriceSignal.GraphQL.Queries;

[QueryType]
public class PriceQueries
{
    [UseOffsetPaging(IncludeTotalCount = true)]
    [UseProjection]
    [UseFiltering]
    [UseSorting]
    public IQueryable<Price> GetPrices(AppDbContext dbContext)
    {
        return dbContext.Prices.AsQueryable();
    }
}
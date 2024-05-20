using Domain.Models.Instruments;
using Infrastructure.Data;

namespace PriceSignal.GraphQL.Queries;

[QueryType]
public class InstrumentPriceQueries
{
    [UseOffsetPaging(IncludeTotalCount = true)]
    [UseProjection]
    [UseFiltering]
    [UseSorting]
    public IQueryable<InstrumentPrice> GetPrices(AppDbContext dbContext)
    {
        return dbContext.InstrumentPrices.AsQueryable();
    }
}
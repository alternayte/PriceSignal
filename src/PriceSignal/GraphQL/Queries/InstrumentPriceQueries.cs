using Domain.Models.Instruments;
using Infrastructure.Data;

namespace PriceSignal.GraphQL.Queries;

[QueryType]
public class InstrumentPriceQueries
{
    [UsePaging(IncludeTotalCount = true)]
    [UseProjection]
    [UseFiltering]
    [UseSorting]
    public IQueryable<InstrumentPrice> GetInstrumentPrices(AppDbContext dbContext)
    {
        return dbContext.InstrumentPrices.AsQueryable();
    }
}
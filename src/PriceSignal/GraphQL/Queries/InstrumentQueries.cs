using System.ComponentModel;
using System.Linq.Expressions;
using Domain.Models.Instruments;
using Infrastructure.Data;
using PriceSignal.GraphQL.Types;

namespace PriceSignal.GraphQL.Queries;

[QueryType]
public class InstrumentQueries
{
    [UsePaging(IncludeTotalCount = true)]
    [UseProjection]
    [UseFiltering]
    [UseSorting]
    public IQueryable<InstrumentPrice> GetInstrumentPrices(AppDbContext dbContext)
    {
        return dbContext.InstrumentPrices.AsQueryable();
    }
    
    [UsePaging(IncludeTotalCount = true)]
    [UseProjection]
    [UseFiltering]
    public IQueryable<Instrument> GetInstruments(AppDbContext dbContext)
    {
        return dbContext.Instruments.AsQueryable();
        
    }
}
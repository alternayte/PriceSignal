using Domain.Models.Instruments;
using Infrastructure.Data;
using PriceSignal.GraphQL.Types;

namespace PriceSignal.GraphQL.Queries;

[QueryType]
public class PriceQueries
{
    [UseOffsetPaging(IncludeTotalCount = true)]
    [UseProjection]
    [UseFiltering]
    [UseSorting]
    public IQueryable<Price> GetPrices(AppDbContext dbContext,PriceInterval interval)
    {
        return interval switch
        {
            PriceInterval.OneMin => MapToPrice(dbContext.OneMinCandle.AsQueryable()),
            PriceInterval.FiveMin => MapToPrice(dbContext.FiveMinCandle.AsQueryable()),
            _ => MapToPrice(dbContext.OneMinCandle.AsQueryable())
        };
    }
    
    private IQueryable<Price> MapToPrice<T>(IQueryable<T> query) where T : Price
    {
        return query.Select(p => new Price
        {
            Symbol = p.Symbol,
            Open = p.Open,
            High = p.High,
            Low = p.Low,
            Close = p.Close,
            Volume = p.Volume,
            Bucket = p.Bucket,
            Exchange = p.Exchange
        });
    }
}
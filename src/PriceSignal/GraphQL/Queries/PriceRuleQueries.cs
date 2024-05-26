using Domain.Models.PriceRule;
using Infrastructure.Data;

namespace PriceSignal.GraphQL.Queries;

[QueryType]
public class PriceRuleQueries
{
    [UsePaging(IncludeTotalCount = true)]
    [UseProjection]
    [UseFiltering]
    public IQueryable<PriceRule> GetPriceRules(AppDbContext dbContext)
    {
        return dbContext.PriceRules.AsQueryable();
    }
}
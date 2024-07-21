using Domain.Models.PriceRule;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

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
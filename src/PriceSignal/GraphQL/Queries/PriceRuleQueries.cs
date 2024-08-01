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
    
    [UseSingleOrDefault]
    [UseProjection]
    public IQueryable<PriceRule> GetPriceRule(AppDbContext dbContext, Guid id)
    {
        return dbContext.PriceRules.Where(pr => pr.EntityId == id).AsQueryable();
    }
    
    
    // [UsePaging(IncludeTotalCount = true)]
    // [UseProjection]
    // [UseFiltering]
    // public IQueryable<PriceRuleTriggerLog> GetPriceRuleActivationLogs(AppDbContext dbContext)
    // {
    //     return dbContext.PriceRuleTriggerLogs.AsQueryable();
    // }

}
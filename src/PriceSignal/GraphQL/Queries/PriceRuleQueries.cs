using Application.Common.Interfaces;
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
    public IQueryable<PriceRule> GetPriceRules(AppDbContext dbContext,[Service] IUser user)
    {
        return dbContext.PriceRules.Where(pr=>pr.UserId == user.UserIdentifier).AsQueryable();
    }
    
    [UseSingleOrDefault]
    [UseProjection]
    public IQueryable<PriceRule> GetPriceRule(AppDbContext dbContext, Guid id,[Service] IUser user)
    {
        return dbContext.PriceRules
            .Where(pr => pr.EntityId == id && pr.UserId == user.UserIdentifier)
            .AsQueryable();
    }
}
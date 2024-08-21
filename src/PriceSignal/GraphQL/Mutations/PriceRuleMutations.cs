using System.Text.Json;
using Application.Common.Interfaces;
using Application.Rules;
using Application.Rules.Common;
using Domain.Models.Instruments;
using Domain.Models.PriceRule;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using PriceSignal.BackgroundServices;
using PriceSignal.GraphQL.Types;


namespace PriceSignal.GraphQL.Mutations;

[MutationType]
public class PriceRuleMutations
{
    public async Task<PriceRule> CreatePriceRule(PriceRuleInput input, AppDbContext dbContext, [Service] IServiceProvider serviceProvider, [Service] RuleCache ruleCache, [Service] IUser user)
    {
        if (user.UserIdentifier == null)
        {
            throw new InvalidOperationException("User not found");
        }
        var instrument = await dbContext.Instruments.FirstOrDefaultAsync(i => i.EntityId == input.InstrumentId);
        if (instrument == null)
        {
            throw new InvalidOperationException("Instrument not found");
        }

        var options = new JsonDocumentOptions
        {
            AllowTrailingCommas = true,
            CommentHandling = JsonCommentHandling.Skip,
            MaxDepth = 64
        };
        var conditions = input.Conditions.Select(c => new PriceCondition
        {
            ConditionType = c.ConditionType,
            Value = c.Value,
            AdditionalValues = JsonDocument.Parse(c.AdditionalValues, options)
        }).ToList();
        
        var priceRule = new PriceRule
        {
            Name = input.Name,
            Description = input.Description,
            InstrumentId = instrument.Id,
            Conditions = conditions,
            UserId = user.UserIdentifier
        };

        try
        {
            dbContext.PriceRules.Add(priceRule);
            await dbContext.SaveChangesAsync();
            ruleCache.AddOrUpdateRule(priceRule);
            var binanceProcessingService = serviceProvider.GetService<BinancePriceFetcherService>();
            binanceProcessingService?.UpdateSubscriptionsAsync();            
        } catch (Exception e)
        {
            throw new InvalidOperationException("Error creating price rule", e);
        }

        return priceRule;
    }
    
    public async Task<PriceRule> UpdatePriceRule(PriceRuleInput input, AppDbContext dbContext, [Service] RuleCache ruleCache,[Service] IUser user)
    {
        var priceRule = await dbContext.PriceRules.Include(pr => pr.Conditions).FirstOrDefaultAsync(pr => pr.EntityId == input.Id && pr.UserId == user.UserIdentifier);
        if (priceRule == null)
        {
            throw new InvalidOperationException("Price rule not found");
        }

        var instrument = await dbContext.Instruments.FirstOrDefaultAsync(i => i.EntityId == input.InstrumentId);
        if (instrument == null)
        {
            throw new InvalidOperationException("Instrument not found");
        }

        var options = new JsonDocumentOptions
        {
            AllowTrailingCommas = true,
            CommentHandling = JsonCommentHandling.Skip,
            MaxDepth = 64
        };
        var conditions = input.Conditions.Select(c => new PriceCondition
        {
            ConditionType = c.ConditionType,
            Value = c.Value,
            AdditionalValues = JsonDocument.Parse(c.AdditionalValues, options)
        }).ToList();
        
        priceRule.Name = input.Name;
        priceRule.Description = input.Description;
        priceRule.InstrumentId = instrument.Id;
        priceRule.Conditions = conditions;

        try
        {
            dbContext.PriceRules.Update(priceRule);
            await dbContext.SaveChangesAsync();
            ruleCache.AddOrUpdateRule(priceRule);
        } catch (Exception e)
        {
            throw new InvalidOperationException("Error updating price rule", e);
        }

        return priceRule;
    }
    
    public async Task<PriceRule> DeletePriceRule(Guid id, AppDbContext dbContext, [Service] RuleCache ruleCache,[Service] IUser user)
    {
        var priceRule = await dbContext.PriceRules.FirstOrDefaultAsync(pr => pr.EntityId == id && pr.UserId == user.UserIdentifier);
        if (priceRule == null)
        {
            throw new InvalidOperationException("Price rule not found");
        }

        try
        {
            dbContext.PriceRules.Remove(priceRule);
            await dbContext.SaveChangesAsync();
            ruleCache.RemoveRule(priceRule.Id);
        } catch (Exception e)
        {
            throw new InvalidOperationException("Error deleting price rule", e);
        }

        return priceRule;
    }
    
    public async Task<PriceRule> TogglePriceRule(Guid id, AppDbContext dbContext,[Service] IServiceProvider serviceProvider, [Service] RuleCache ruleCache,[Service] IUser user)
    {
        var priceRule = await dbContext.PriceRules.FirstOrDefaultAsync(pr => pr.EntityId == id && pr.UserId == user.UserIdentifier);
        if (priceRule == null)
        {
            throw new InvalidOperationException("Price rule not found");
        }

        priceRule.IsEnabled = !priceRule.IsEnabled;

        try
        {
            dbContext.PriceRules.Update(priceRule);
            await dbContext.SaveChangesAsync();
            ruleCache.AddOrUpdateRule(priceRule);
            var binanceProcessingService = serviceProvider.GetService<BinancePriceFetcherService>();
            binanceProcessingService?.UpdateSubscriptionsAsync();     
        } catch (Exception e)
        {
            throw new InvalidOperationException("Error enabling price rule", e);
        }

        return priceRule;
    }
}
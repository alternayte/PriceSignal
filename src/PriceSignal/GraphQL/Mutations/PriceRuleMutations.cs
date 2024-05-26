using System.Text.Json;
using Application.Rules;
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
    public async Task<PriceRule> CreatePriceRule(PriceRuleInput input, AppDbContext dbContext, [Service] IServiceProvider serviceProvider, [Service] RuleCache ruleCache)
    {
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
            Conditions = conditions
        };

        try
        {
            dbContext.PriceRules.Add(priceRule);
            await dbContext.SaveChangesAsync();
            ruleCache.AddOrUpdateRule(priceRule);
            var binanceProcessingService = serviceProvider.GetRequiredService<BinancePriceFetcherService>();
            if (binanceProcessingService != null) binanceProcessingService.UpdateSubscriptionsAsync();            
        } catch (Exception e)
        {
            throw new InvalidOperationException("Error creating price rule", e);
        }

        return priceRule;
    }
}
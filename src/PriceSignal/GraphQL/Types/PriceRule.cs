using System.Text.Json;
using Application.Common.Interfaces;
using Domain.Models.Instruments;
using Domain.Models.NotificationChannel;
using Domain.Models.PriceRule;
using HotChocolate.Types.Pagination;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using PriceSignal.GraphQL.Queries;

namespace PriceSignal.GraphQL.Types;


public class PriceRuleType : ObjectType<PriceRule>
{
    protected override void Configure(IObjectTypeDescriptor<PriceRule> descriptor)
    {
        descriptor.BindFieldsExplicitly();
        descriptor.Field(x => x.EntityId).Type<NonNullType<IdType>>().Name("id");
        descriptor.Field(x => x.Name).Type<NonNullType<StringType>>();
        descriptor.Field(x => x.Description).Type<StringType>();
        descriptor.Field(x => x.Instrument).Type<NonNullType<InstrumentType>>();
        descriptor.Field(x => x.IsEnabled).Type<NonNullType<BooleanType>>();
        descriptor.Field(x => x.NotificationChannel).Type<NonNullType<NotificationChannelTypeType>>();
        descriptor.Field(x => x.Conditions).Type<ListType<NonNullType<PriceConditionType>>>()
            .UsePaging(options: new PagingOptions {IncludeTotalCount = true})
            .UseProjection()
            .Resolve(context =>
            {
                var rule = context.Parent<PriceRule>();
                return context.Services.GetRequiredService<IPriceConditionsOnPriceRuleDataLoader>().LoadAsync(rule.EntityId);
            });
        descriptor.Field(x=>x.ActivationLogs).Type<ListType<PriceRuleTriggerLogType>>()
            .UsePaging(options: new PagingOptions {IncludeTotalCount = true})
            .UseProjection()
            .Resolve(context =>
            {
                var rule = context.Parent<PriceRule>();
                return context.Services.GetRequiredService<IPriceRuleTriggerLogsDataLoader>().LoadAsync(rule.EntityId);
            });
        descriptor.Field(x=>x.CreatedAt).Type<NonNullType<DateTimeType>>();
        
    }
    
    [DataLoader]
    internal static async Task<ILookup<Guid,PriceCondition>> GetPriceConditionsOnPriceRuleAsync(IReadOnlyList<Guid> priceRuleIds, 
        AppDbContext dbContext, CancellationToken cancellationToken)
    {
        var rules = dbContext.PriceRules.Include(x => x.Conditions).AsQueryable()
            .Where(x => priceRuleIds.Contains(x.EntityId));

        return rules.SelectMany(x => x.Conditions.Select(c => new PriceCondition()
        {
            Rule = x,
            ConditionType = c.ConditionType,
            Value = c.Value,
            AdditionalValues = c.AdditionalValues
        })).ToLookup(x => x.Rule.EntityId);
    }
    
    [DataLoader]
    internal static async Task<ILookup<Guid,PriceRuleTriggerLog>> GetPriceRuleTriggerLogsAsync(IReadOnlyList<Guid> priceRuleIds, 
        AppDbContext dbContext, CancellationToken cancellationToken)
    {
        var logs = dbContext.PriceRules.Include(x=>x.ActivationLogs).AsQueryable()
            .Where(x => priceRuleIds.Contains(x.EntityId));
        
        return logs.SelectMany(x => x.ActivationLogs.Select(l => new PriceRuleTriggerLog()
        {
            EntityId = l.EntityId,
            PriceRule = x,
            TriggeredAt = l.TriggeredAt,
            Price = l.Price,
            PriceChange = l.PriceChange,
            PriceChangePercentage = l.PriceChangePercentage,
            PriceRuleSnapshot = l.PriceRuleSnapshot
        })).ToLookup(x => x.PriceRule.EntityId);
    }
}

public class PriceRuleInput
{
    public  Guid Id { get; set; }
    public required string Name { get; set; }
    public required string Description { get; set; }
    public required Guid InstrumentId { get; set; }
    public bool IsEnabled { get; set; }
    public NotificationChannelType NotificationChannel { get; set; }
    public ICollection<PriceConditionInput> Conditions { get; set; } = new List<PriceConditionInput>();
}


public class PriceConditionInput
{
    public required Guid Id { get; set; }
    public required string ConditionType { get; set; }
    public required decimal Value { get; set; }
    public string? AdditionalValues { get; set; }
}

public class PriceRuleInputType : InputObjectType<PriceRuleInput>
{
    protected override void Configure(IInputObjectTypeDescriptor<PriceRuleInput> descriptor)
    {
        descriptor.BindFieldsExplicitly();
        descriptor.Field(x => x.Id).Type<NonNullType<IdType>>();
        descriptor.Field(x => x.Name).Type<NonNullType<StringType>>();
        descriptor.Field(x => x.Description).Type<NonNullType<StringType>>();
        descriptor.Field(x => x.InstrumentId).Type<NonNullType<IdType>>();
        descriptor.Field(x => x.Conditions).Type<ListType<PriceConditionInputType>>();
        descriptor.Field(x => x.IsEnabled).Type<BooleanType>();
        descriptor.Field(x=> x.NotificationChannel).Type<NotificationChannelTypeType>();
    }
}

public class PriceConditionInputType : InputObjectType<PriceConditionInput>
{
    protected override void Configure(IInputObjectTypeDescriptor<PriceConditionInput> descriptor)
    {
        descriptor.BindFieldsExplicitly();
        descriptor.Field(x => x.ConditionType).Type<NonNullType<ConditionTypeType>>();
        descriptor.Field(x => x.Value).Type<NonNullType<DecimalType>>();
        descriptor.Field(x => x.AdditionalValues).Type<JsonType>();
    }
}
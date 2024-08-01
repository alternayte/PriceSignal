using Domain.Models.PriceRule;

namespace PriceSignal.GraphQL.Types;

public class PriceRuleTriggerLogType : ObjectType<PriceRuleTriggerLog>
{
    protected override void Configure(IObjectTypeDescriptor<PriceRuleTriggerLog> descriptor)
    {
        descriptor.BindFieldsExplicitly();
        descriptor.Field(x=>x.EntityId).Type<NonNullType<IdType>>().Name("id");
        descriptor.Field(x=>x.TriggeredAt).Type<NonNullType<DateTimeType>>();
        descriptor.Field(x=>x.Price).Type<DecimalType>();
        descriptor.Field(x=>x.PriceChange).Type<DecimalType>();
        descriptor.Field(x=>x.PriceChangePercentage).Type<DecimalType>();
        descriptor.Field(x=>x.PriceRuleSnapshot).Type<StringType>()
            .Resolve(context =>
            {
                var log = context.Parent<PriceRuleTriggerLog>();
                return log.PriceRuleSnapshot.RootElement.GetRawText();
            });
    }
}
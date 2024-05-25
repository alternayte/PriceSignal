using Domain.Models.PriceRule;

namespace PriceSignal.GraphQL.Types;

public class PriceRuleType : ObjectType<PriceRule>
{
    protected override void Configure(IObjectTypeDescriptor<PriceRule> descriptor)
    {
        descriptor.BindFieldsExplicitly();
        descriptor.Field(x => x.Description).Type<StringType>();
    }
}
using System.Text.Json;
using Domain.Models.PriceRule;

namespace PriceSignal.GraphQL.Types;

public class PriceConditionType : ObjectType<PriceCondition>
{
    protected override void Configure(IObjectTypeDescriptor<PriceCondition> descriptor)
    {
        descriptor.BindFieldsExplicitly();
        //descriptor.Field(x=>x.Id).Type<NonNullType<IdType>>();
        //descriptor.Field(x=> x.Rule).Type<NonNullType<PriceRuleType>>();
        descriptor.Field(x=>x.Value).Type<NonNullType<DecimalType>>();
        descriptor.Field(x => x.AdditionalValues).Type<StringType>()
            .Resolve(context =>
            {
                var condition = context.Parent<PriceCondition>();
                //return condition.AdditionalValues.RootElement.GetRawText();
                //return condition.AdditionalValues.Deserialize<string>();
                return JsonSerializer.Serialize(condition.AdditionalValues);
            });
        descriptor.Field(x=>x.ConditionType).Type<NonNullType<ConditionTypeType>>();

    }
}

// public class AdditionalValueType : InputObjectType<AdditionalValue>
// {
//     protected override void Configure(IInputObjectTypeDescriptor<AdditionalValue> descriptor)
//     {
//         descriptor.BindFieldsExplicitly();
//         descriptor.Field(x=>x.Name).Type<StringType>();
//         descriptor.Field(x=>x.Period).Type<StringType>();
//         descriptor.Field(x=>x.Value).Type<StringType>();
//         descriptor.Field(x=>x.Type).Type<StringType>();
//     }
// }


public class ConditionTypeType : EnumType<ConditionType>
{
    protected override void Configure(IEnumTypeDescriptor<ConditionType> descriptor)
    {
        descriptor.BindValuesExplicitly();
        descriptor.Value(ConditionType.PricePercentage).Name("PRICE_PERCENTAGE");
        descriptor.Value(ConditionType.Price).Name("PRICE");
        descriptor.Value(ConditionType.PriceAction).Name("PRICE_ACTION");
        descriptor.Value(ConditionType.PriceCrossover).Name("PRICE_CROSSOVER");
        descriptor.Value(ConditionType.TechnicalIndicator).Name("TECHNICAL_INDICATOR");
    }
}
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
        descriptor.Field(x => x.AdditionalValues).Type<JsonType>();
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

public enum ConditionType
{
    PricePercentChange,
    PriceChange,
    TechnicalIndicator,
}

public class ConditionTypeType : EnumType<ConditionType>
{
    protected override void Configure(IEnumTypeDescriptor<ConditionType> descriptor)
    {
        descriptor.BindValuesExplicitly();
        descriptor.Value(ConditionType.PriceChange).Name("PRICE_CHANGE");
        descriptor.Value(ConditionType.PricePercentChange).Name("PRICE_PERCENT_CHANGE");
        descriptor.Value(ConditionType.TechnicalIndicator).Name("TECHNICAL_INDICATOR");
    }
}
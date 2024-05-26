using System.Text.Json;
using Domain.Models.PriceRule;

namespace PriceSignal.GraphQL.Types;

public class PriceRuleType : ObjectType<PriceRule>
{
    protected override void Configure(IObjectTypeDescriptor<PriceRule> descriptor)
    {
        descriptor.BindFieldsExplicitly();
        descriptor.Field(x => x.Id).Type<NonNullType<IdType>>();
        descriptor.Field(x => x.Name).Type<NonNullType<StringType>>();
        descriptor.Field(x => x.Description).Type<StringType>();
        descriptor.Field(x => x.Instrument).Type<NonNullType<InstrumentType>>();
        descriptor.Field(x => x.Conditions).Type<ListType<NonNullType<PriceConditionType>>>();
    }
}

public class PriceRuleInput
{
    public required string Name { get; set; }
    public required string Description { get; set; }
    public required Guid InstrumentId { get; set; }
    public ICollection<PriceConditionInput> Conditions { get; set; } = new List<PriceConditionInput>();
}

public class PriceConditionInput
{
    public required string ConditionType { get; set; }
    public required decimal Value { get; set; }
    public string? AdditionalValues { get; set; }
}

public class PriceRuleInputType : InputObjectType<PriceRuleInput>
{
    protected override void Configure(IInputObjectTypeDescriptor<PriceRuleInput> descriptor)
    {
        descriptor.BindFieldsExplicitly();
        descriptor.Field(x => x.Name).Type<NonNullType<StringType>>();
        descriptor.Field(x => x.Description).Type<NonNullType<StringType>>();
        descriptor.Field(x => x.InstrumentId).Type<NonNullType<IdType>>();
        descriptor.Field(x => x.Conditions).Type<ListType<PriceConditionInputType>>();
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
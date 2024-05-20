using Domain.Models.Instruments;

namespace PriceSignal.GraphQL.Types;

public class PriceType : ObjectType<Price>
{
    protected override void Configure(IObjectTypeDescriptor<Price> descriptor)
    {
        descriptor.BindFieldsExplicitly();
        descriptor.Field(x => x.Symbol).Type<NonNullType<StringType>>();
        descriptor.Field(x => x.Open).Type<NonNullType<DecimalType>>();
        descriptor.Field(x => x.High).Type<NonNullType<DecimalType>>();
        descriptor.Field(x => x.Low).Type<NonNullType<DecimalType>>();
        descriptor.Field(x => x.Close).Type<NonNullType<DecimalType>>();
        descriptor.Field(x => x.Volume).Type<DecimalType>();
        descriptor.Field(x => x.Bucket).Type<NonNullType<DateTimeType>>();
        descriptor.Field(x => x.Exchange).Type<NonNullType<ExchangeType>>();
    }
}

public enum PriceInterval
{
    OneMin,
    FiveMin
}

public class PriceIntervalType : EnumType<PriceInterval>
{
    protected override void Configure(IEnumTypeDescriptor<PriceInterval> descriptor)
    {
        descriptor.BindValuesExplicitly();
        descriptor.Value(PriceInterval.OneMin).Name("ONE_MIN");
        descriptor.Value(PriceInterval.FiveMin).Name("FIVE_MIN");
    }
}
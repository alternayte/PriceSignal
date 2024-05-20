using Domain.Models.Instruments;
using HotChocolate.Data.Filters;
using HotChocolate.Data.Sorting;

namespace PriceSignal.GraphQL.Types;

public class InstrumentPriceType : ObjectType<InstrumentPrice>
{
    protected override void Configure(IObjectTypeDescriptor<InstrumentPrice> descriptor)
    {
        descriptor.BindFieldsExplicitly();
        descriptor.Field(x => x.Symbol).Type<NonNullType<StringType>>();
        descriptor.Field(x => x.Price).Type<NonNullType<DecimalType>>();
        descriptor.Field(x => x.Volume).Type<DecimalType>();
        descriptor.Field(x => x.Quantity).Type<DecimalType>();
        descriptor.Field(x => x.Timestamp).Type<NonNullType<DateTimeType>>();
        descriptor.Field(x => x.Exchange).Type<NonNullType<ExchangeType>>();
    }
}

public class InstrumentPriceFilterType : FilterInputType<InstrumentPrice>
{
    protected override void Configure(IFilterInputTypeDescriptor<InstrumentPrice> descriptor)
    {
        descriptor.BindFieldsExplicitly();
        descriptor.Field(x => x.Symbol).Type<StringOperationFilterInputType>();
        descriptor.Field(x => x.Price).Type<DecimalOperationFilterInputType>();
        descriptor.Field(x => x.Volume).Type<DecimalOperationFilterInputType>();
        descriptor.Field(x => x.Quantity).Type<DecimalOperationFilterInputType>();
        descriptor.Field(x => x.Timestamp).Type<DateTimeOperationFilterInputType>();
        descriptor.Field(x => x.Exchange).Type<ExchangeFilterType>();
    }
}

public class InstrumentPriceSortType : SortInputType<InstrumentPrice>
{
    protected override void Configure(ISortInputTypeDescriptor<InstrumentPrice> descriptor)
    {
        descriptor.BindFieldsExplicitly();
        descriptor.Field(x => x.Symbol);
        descriptor.Field(x => x.Price);
        descriptor.Field(x => x.Volume);
        descriptor.Field(x => x.Quantity);
        descriptor.Field(x => x.Timestamp);
        descriptor.Field(x => x.Exchange);
    }
}
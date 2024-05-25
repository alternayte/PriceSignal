using Domain.Models.Instruments;
using HotChocolate.Data.Filters;
using HotChocolate.Data.Sorting;

namespace PriceSignal.GraphQL.Types;

public class InstrumentType : ObjectType<Instrument>
{
    protected override void Configure(IObjectTypeDescriptor<Instrument> descriptor)
    {
        descriptor.BindFieldsExplicitly();
        descriptor.Field(x => x.EntityId).Type<NonNullType<IdType>>().Name("id");
        descriptor.Field(x => x.Symbol).Type<NonNullType<StringType>>();
        descriptor.Field(x => x.Name).Type<NonNullType<StringType>>();
        descriptor.Field(x => x.Description).Type<StringType>();
        descriptor.Field(x => x.BaseAsset).Type<NonNullType<StringType>>();
        descriptor.Field(x => x.QuoteAsset).Type<NonNullType<StringType>>();
    }
}

public class InstrumentFilterType : FilterInputType<Instrument>
{
    protected override void Configure(IFilterInputTypeDescriptor<Instrument> descriptor)
    {
        descriptor.BindFieldsExplicitly();
        descriptor.Field(x => x.EntityId).Type<IdOperationFilterInputType>().Name("id");
        descriptor.Field(x => x.Symbol).Type<StringOperationFilterInputType>();
        descriptor.Field(x => x.Name).Type<StringOperationFilterInputType>();
        descriptor.Field(x => x.BaseAsset).Type<StringOperationFilterInputType>();
        descriptor.Field(x => x.QuoteAsset).Type<StringOperationFilterInputType>();
        descriptor.Field(x => x.Exchange.Name).Type<EnumOperationFilterInputType<ExchangeCode>>().Name("exchange");
    }
}

public class InstrumentSortType : SortInputType<Instrument>
{
    protected override void Configure(ISortInputTypeDescriptor<Instrument> descriptor)
    {
        base.Configure(descriptor);
    }
}
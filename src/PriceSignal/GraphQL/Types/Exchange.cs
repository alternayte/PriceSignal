using Domain.Models.Exchanges;
using HotChocolate.Data.Filters;
using HotChocolate.Data.Sorting;

namespace PriceSignal.GraphQL.Types;

public class ExchangeType : ObjectType<Exchange>
{
    protected override void Configure(IObjectTypeDescriptor<Exchange> descriptor)
    {
        descriptor.BindFieldsExplicitly();
        descriptor.Field(x => x.Name).Type<NonNullType<StringType>>();
        descriptor.Field(x => x.Description).Type<StringType>();
    }
}

public class ExchangeFilterType : FilterInputType<Exchange>
{
    protected override void Configure(IFilterInputTypeDescriptor<Exchange> descriptor)
    {
        descriptor.BindFieldsExplicitly();
        descriptor.Field(x => x.Name).Type<StringOperationFilterInputType>();
        descriptor.Field(x => x.Description).Type<StringOperationFilterInputType>();
    }
}

public class ExchangeSortType : SortInputType<Exchange>
{
    protected override void Configure(ISortInputTypeDescriptor<Exchange> descriptor)
    {
        descriptor.BindFieldsExplicitly();
        descriptor.Field(x => x.Name);
        descriptor.Field(x => x.Description);
    }
}
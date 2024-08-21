using Application.Services.Alpaca.Models;

namespace PriceSignal.GraphQL.Types;

public class NewsType : ObjectType<News>
{
    protected override void Configure(IObjectTypeDescriptor<News> descriptor)
    {
        descriptor.BindFieldsImplicitly();
        descriptor.Field(x => x.id).Type<NonNullType<IdType>>().Name("id");
    }
}
import { ContentLayout } from "@/components/layouts/content";
import { ExchangesList } from "@/features/exchanges/components/exchanges-list";


export const ExchangesRoute = () => {
    return (
        <ContentLayout title='Exchanges'>
            <div className="flex justify-end">
                {/*<CreateDiscussion />*/}
            </div>
            <div className="mt-4">
                <ExchangesList/>
            </div>
        </ContentLayout>
    );
}
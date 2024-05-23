import { ContentLayout } from '@/components/layouts/content';
import { PriceSummary } from '@/features/prices/components/price-summary';

export const PricesRoute = () => {
    return (
        <ContentLayout title='Summary'>
            <div className='flex flex-1 flex-col gap-4 p-4 md:gap-8 md:p-6'>
            <PriceSummary />
            </div>
        </ContentLayout>
    );
};
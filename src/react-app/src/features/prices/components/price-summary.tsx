import { Card, CardContent, CardHeader, CardTitle } from '@/components/ui/card';
import { BarChartIcon, DollarSignIcon, LineChart } from 'lucide-react';
import { FocusedPricesList } from './focused-prices-list';

export const PriceSummary = () => {
  return (
    <>
      <div className="grid gap-4 md:grid-cols-2 lg:grid-cols-3">
        <Card>
          <CardHeader className="flex flex-row items-center justify-between pb-2">
            <CardTitle className="text-sm font-medium">Total Portfolio Value</CardTitle>
            <DollarSignIcon className="w-4 h-4 text-gray-500 dark:text-gray-400" />
          </CardHeader>
          <CardContent>
            <div className="text-2xl font-bold">$42,356.89</div>
            <p className="text-xs text-gray-500 dark:text-gray-400">+4.2% from last month</p>
          </CardContent>
        </Card>
        <FocusedPricesList />
      </div>
      <div className="grid gap-4 md:grid-cols-2 lg:grid-cols-2">
        <Card>
          <CardHeader className="flex flex-row items-center justify-between pb-2">
            <CardTitle className="text-sm font-medium">Bitcoin Price Chart</CardTitle>
            <BarChartIcon className="w-4 h-4 text-gray-500 dark:text-gray-400" />
          </CardHeader>
          <CardContent>
            <LineChart className="aspect-[9/4]" />
          </CardContent>
        </Card>
        <Card>
          <CardHeader className="flex flex-row items-center justify-between pb-2">
            <CardTitle className="text-sm font-medium">Ethereum Price Chart</CardTitle>
            <BarChartIcon className="w-4 h-4 text-gray-500 dark:text-gray-400" />
          </CardHeader>
          <CardContent>
            <LineChart className="aspect-[9/4]" />
          </CardContent>
        </Card>
      </div>
    </>
  );
};

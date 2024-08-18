import { Card, CardContent, CardHeader, CardTitle } from '@/components/ui/card';
import {SearchIcon} from 'lucide-react';
import { FocusedPricesList } from './focused-prices-list';
import { FocusedPricesCharts } from './focused-prices-charts';
import { Input } from '@/components/ui/input';
import { ActiveRulesList } from '@/features/rules/components/active-rules-list';
import { MarketNewsList } from '@/features/news/components/market-news-list';

export const PriceSummary = () => {
  return (
      <>
          <div>
              <h2 className="text-xl font-bold mb-4">Focused Instruments</h2>
              <div className="grid gap-4 md:grid-cols-2 lg:grid-cols-3">
                  <Card
                      className="border-dashed border-2 hover:shadow-lg hover:border-slate-400 hover:scale-105 transition duration-150 ease-in-out">
                      <CardHeader className="flex flex-row items-center justify-between pb-2">
                          <CardTitle className="text-sm font-medium">Add an instruement to track</CardTitle>
                      </CardHeader>
                      <CardContent>
                          <Input placeholder='Search stocks, forex, crypto...' startIcon={SearchIcon} disabled/>
                      </CardContent>
                  </Card>
                  <FocusedPricesList/>
              </div>
          </div>
          <div className="grid gap-4 lg:grid-cols-2">
              <ActiveRulesList/>
              <MarketNewsList/>
          </div>
          <div className="grid gap-4 md:grid-cols-2 lg:grid-cols-2">
              <FocusedPricesCharts/>
              {/*<PriceChartSimple title="BTC"/>*/}
              {/*<PriceChartSimple title="ETH"/>*/}
          </div>
      </>
  );
};

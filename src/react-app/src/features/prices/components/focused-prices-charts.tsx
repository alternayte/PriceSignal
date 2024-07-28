import { graphql } from '@/gql';
import {useQuery} from "@apollo/client";
import { PriceChartSimple } from './price-chart-simple';
import { PriceItemFragment } from '@/gql/graphql';

export const PriceItem = graphql(/* GraphQL */ `
  fragment PriceItem on Price {
    bucket
    close
    symbol
    volume
  }
`)


const focusedPriceLineQuery = graphql(`
query GetFocusedPriceLine {
  ETH:prices(last: 500,interval: ONE_HOUR,where: {symbol:{eq:"ETHUSDT"}}) {
    edges {
      node {
        ...PriceItem
      }
    }
  }
  BTC:prices(last: 500,interval: ONE_HOUR,where: {symbol:{eq:"BTCUSDT"}}) {
    edges {
      node {
        ...PriceItem
      }
    }
  }
}
`)

export const FocusedPricesCharts = () => {
    const { data, loading } = useQuery(focusedPriceLineQuery);
    
    if (loading) return <p>Loading...</p>;
    if (!data) return <p>No data</p>;

    const ETHData = data.ETH?.edges?.map(e=> ({
        bucket:(e.node as PriceItemFragment).bucket,
        price:(e.node as PriceItemFragment).close}));
    
    const BTCData = data.BTC?.edges?.map(e=> ({
        bucket:(e.node as PriceItemFragment).bucket,
        price:(e.node as PriceItemFragment).close}));

    return (
        <>
            <PriceChartSimple title="ETH" data={ETHData}/>
            <PriceChartSimple title="BTC" data={BTCData}/>
        </>
    );
};
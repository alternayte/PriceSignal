import { ChartLayout } from '@/components/layouts/chart';
import { ContentLayout } from '@/components/layouts/content';
import { MAX_SLICE_SIZE } from '@/constants';
import { PricesChart } from '@/features/prices/components/prices-chart';
import { graphql } from '@/gql';
import { Price, PriceInterval } from '@/gql/graphql';
import { useQuery } from '@apollo/client';
import { useEffect } from 'react';
import { useNavigate, useParams } from 'react-router-dom';

const getPricesForSymbolQuery = graphql(`
  query GetPricesForSymbol($symbol: String!,$last: Int!, $interval: PriceInterval!) {
    prices(
      last: $last
      where: { symbol: { eq: $symbol } }
      order: { bucket: ASC }
      interval: $interval
    ) {
      nodes {
        timestamp: bucket
        close
        high
        low
        open
        symbol
        volume
      }
    }
  }
`);

const subscribeToPricesForSymbol = graphql(`
  subscription SubscribeToPricesForSymbol($symbol: String!) {
    onPriceUpdated(symbol: $symbol) {
      timestamp: bucket
      close
      high
      low
      open
      symbol
      volume
    }
  }
`);

export const SymbolRoute = () => {
  const params = useParams();
  const navigate = useNavigate();
  const symbol = params.symbol as string;

  const { data, loading, subscribeToMore } = useQuery(getPricesForSymbolQuery, { variables: { symbol, last:MAX_SLICE_SIZE , interval: PriceInterval.OneMin} });

  useEffect(() => {
    const unsubscribe = subscribeToMore<Price>({
      document: subscribeToPricesForSymbol,
      variables: { symbol,last:MAX_SLICE_SIZE, interval: PriceInterval.OneMin },
      updateQuery: (prev, { subscriptionData }) => {
        if (!subscriptionData.data) {
          return {
            prices: {
              __typename: 'PricesConnection',
              nodes: [{ symbol, timestamp: '', close: 0, high: 0, low: 0, open: 0, volume: 0 }],
            },
          };
        }
        // @ts-ignore
        const newData = subscriptionData.data.onPriceUpdated;
        const newPrice = {
          timestamp: newData.timestamp,
          close: newData.close,
          high: newData.high,
          low: newData.low,
          open: newData.open,
          symbol,
          volume: newData.volume,
        };
        const newPrices = [...(prev?.prices?.nodes || []), newPrice];
        if (newPrices.length > MAX_SLICE_SIZE) {
          newPrices.shift()
        }
        return {
          prices: {
            __typename: 'PricesConnection',
            nodes: newPrices,
          },
        };
      },
    });
    return () => {
      unsubscribe();
    };
  }, [symbol, subscribeToMore]);

  if (loading) return <p>Loading...</p>;

  if (!data) return <p>No data</p>;

  const klines =
    data.prices?.nodes?.map((price) => ({
      timestamp: price.timestamp,
      open: price.open,
      high: price.high,
      low: price.low,
      close: price.close,
      volume: price.volume,
    })) || [];

  if (klines.length === 0) {
    navigate('/');
  }

  return (
    <ChartLayout title={`Chart | ${symbol}`}>
      <PricesChart key={symbol} data={klines} />
    </ChartLayout>
  );
};

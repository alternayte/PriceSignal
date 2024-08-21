import { graphql } from '@/gql';
import { useQuery } from '@apollo/client';
import { PriceCard } from './price-card';
import { useEffect, useState } from 'react';

export const PriceItem = graphql(`
  fragment PriceItem on Price {
    bucket
    close
    symbol
    volume
  }
`);

const focusedPricesQuery = graphql(`
  query GetFocusedPrices {
    ETH: prices(last: 1, interval: ONE_MIN, where: { symbol: { eq: "ETHUSDT" } }) {
      edges {
        node {
          ...PriceItem
        }
      }
    }
    BTC: prices(last: 1, interval: ONE_MIN, where: { symbol: { eq: "BTCUSDT" } }) {
      edges {
        node {
          ...PriceItem
        }
      }
    }
  }
`);
export const FocusedPricesList = () => {
  const { data, loading, refetch } = useQuery(focusedPricesQuery);
  const [progress, setProgress] = useState(0);

  useEffect(() => {
    const intervalId = setInterval(() => {
      refetch();
      setProgress(0);
    }, 30000);

    return () => clearInterval(intervalId); // Clean up on unmount
  }, [refetch]);

  useEffect(() => {
    const progressIntervalId = setInterval(() => {
      setProgress((prevProgress) => (prevProgress < 100 ? prevProgress + 1 : 0));
    }, 300);

    return () => clearInterval(progressIntervalId); // Clean up on unmount
  }, []);

  if (loading) return <p>Loading...</p>;
  if (!data) return <p>No data</p>;

  return (
    <>
      {data && data?.ETH?.edges?.map((price, i) => price.node && <PriceCard price={price.node} key={`price-${i}`} progress={progress} />)}
      {data && data?.BTC?.edges?.map((price, i) => price.node && <PriceCard price={price.node} key={`price-${i}`} progress={progress} />)}
    </>
  );
};

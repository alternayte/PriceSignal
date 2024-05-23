import { ContentLayout } from '@/components/layouts/content';
import { PricesChart } from '@/features/prices/components/prices-chart';
import { graphql } from '@/gql';
import { useQuery } from '@apollo/client';
import { useNavigate, useParams } from 'react-router-dom';

const getPricesForSymbolQuery = graphql(`
  query GetPricesForSymbol($symbol: String!) {
    prices(
      last: 100
      where: { symbol: { eq: $symbol } }
      # order: { bucket: DESC }
      interval: FIVE_MIN
    ) {
      nodes {
        bucket
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

export const SymbolRoute = () => {
  const params = useParams();
  const navigate = useNavigate();
  const symbol = params.symbol as string;

  const { data, loading } = useQuery(getPricesForSymbolQuery, { variables: { symbol } });

  if (loading) return <p>Loading...</p>;
 
  if (!data) return <p>No data</p>;

  const klines =
    data.prices?.nodes?.map((price) => ({
      timestamp: price.bucket,
      open: price.open,
      high: price.high,
      low: price.low,
      close: price.close,
      volume: price.volume,
    })) || [];
  
  if (klines.length === 0) {
    navigate('/')
  }
  
  return (
    <ContentLayout title={`Chart | ${symbol}`}>
        <PricesChart key={symbol} data={klines} />
    </ContentLayout>
  );
};

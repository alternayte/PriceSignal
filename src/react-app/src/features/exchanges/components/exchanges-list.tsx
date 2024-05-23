import { graphql } from '@/gql';
import {useQuery} from "@apollo/client";



const allExchangesQuery = graphql(`
query GetExchanges($take: Int!) {
      exchanges(take: $take) {
        totalCount
        items {
          name
          description
        }
      }
    }
`)
export const ExchangesList = () => {
    const { data, loading } = useQuery(allExchangesQuery, {variables: {take: 10}});
    
    if (loading) return <p>Loading...</p>;
    if (!data) return <p>No data</p>;
    
    return (
        <div>
            <h1>Exchanges</h1>
            {loading && <p>Loading...</p>}
            {data && data?.exchanges?.items?.map((exchange) => (
                <div key={exchange.name}>
                    <h2>{exchange.name}</h2>
                    <p>{exchange.description}</p>
                </div>
            ))}
        </div>
    );
};
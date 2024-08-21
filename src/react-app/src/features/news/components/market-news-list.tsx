import { Card, CardContent, CardHeader } from '@/components/ui/card';
import { graphql } from '@/gql';
import { useQuery } from '@apollo/client';
import { Newspaper } from 'lucide-react';
import { Link } from 'react-router-dom';

const marketNewsQuery = graphql(`
  query MarketNews {
    news {
      id
      headline
      summary
      url
    }
  }
`);
export const MarketNewsList = () => {
  const { data, loading } = useQuery(marketNewsQuery);
  if (loading) {
    return <div>Loading...</div>;
  }
  if (!data) {
    return <div>No data</div>;
  }

  return (
    <div className='flex flex-col'>
      <h2 className="text-xl font-bold mb-4 flex gap-x-2">
        Market News
        <Newspaper />
      </h2>
      <Card className='h-full'>
        <CardHeader className="flex flex-row items-center justify-between pb-2"></CardHeader>
        <CardContent>
          <ul className="space-y-4">
            {data.news.slice(0,5).map((news, index) => (
              <li key={index} className="hover:bg-gray-50 relative">
                <div className="flex justify-between items-start">
                  <div>
                    <h3 className="text-lg font-medium text-blue-600 line-clamp-1 hover:underline">{news.headline}</h3>
                    {news.summary ? <p className="text-muted-foreground line-clamp-1">{news.summary}</p> : <p className="h-[24]px" />}
                  </div>
                  <Link to={news.url} className="text-primary" target="_blank">
                    <span className="absolute inset-x-0 -top-px bottom-0"></span>
                  </Link>
                </div>
              </li>
            ))}
          </ul>
        </CardContent>
      </Card>
    </div>
  );
};

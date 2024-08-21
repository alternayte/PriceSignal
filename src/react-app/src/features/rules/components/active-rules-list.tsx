import { Button } from '@/components/ui/button';
import { Card, CardContent, CardHeader } from '@/components/ui/card';
import { graphql } from '@/gql';
import { Puzzle } from 'lucide-react';
import { Link } from 'react-router-dom';
import { useQuery } from '@apollo/client';
import { formatDistanceToNow } from 'date-fns';
import { SkeletonRule } from '@/components/common/loaders/skeleton-rule';

const activeRulesQuery = graphql(`
  query ActiveRules($first: Int) {
    priceRules(first: $first, where: { isEnabled: { eq: true } }) {
      nodes {
        id
        name
        description
        lastTriggeredAt
        lastTriggeredPrice
        instrument {
          id
          baseAsset
          quoteAsset
          symbol
        }
      }
    }
  }
`);
export const ActiveRulesList = () => {
  const { data, loading } = useQuery(activeRulesQuery, { variables: { first: 4 } });

  if (loading)
    return (
      <div className="flex flex-col">
        <div className="flex justify-between">
          <h2 className="text-xl font-bold mb-4 flex gap-x-2">
            Active Rules
            <Puzzle />
          </h2>
          <Button asChild variant="link">
            <Link to="/rules">View all</Link>
          </Button>
        </div>
        <Card className="h-full">
          <CardHeader className="flex flex-row items-center justify-between pb-2"></CardHeader>
          <CardContent>
            {Array.from({ length: 4 }).map((_, index) => (
              <SkeletonRule key={index} />
            ))}
          </CardContent>
        </Card>
      </div>
    );

  if (!data) return <p>No data</p>;
  return (
    <div className="flex flex-col">
      <div className="flex justify-between">
        <h2 className="text-xl font-bold mb-4 flex gap-x-2">
          Active Rules
          <Puzzle />
        </h2>
        <Button asChild variant="link">
          <Link to="/rules">View all</Link>
        </Button>
      </div>
      <Card className="h-full">
        <CardHeader className="flex flex-row items-center justify-between pb-2"></CardHeader>
        <CardContent>
          <ul className="space-y-4">
            {data.priceRules?.nodes?.map((rule, index) => (
              <li key={index} className="hover:bg-gray-50 relative">
                <div className="flex justify-between items-start">
                  <div className="flex space-x-4">
                    <div className="flex -space-x-1">
                      <img
                        alt={`${rule.instrument.baseAsset} logo`}
                        height="32"
                        className="rounded-full bg-gray-50 ring-2 ring-white h-6 w-6 object-contain"
                        src={`https://cryptofonts.com/img/SVG/${rule.instrument.baseAsset.toLowerCase()}.svg`}
                        width="32"
                      />
                      {rule.instrument.quoteAsset && (
                        <img
                          alt={`${rule.instrument.quoteAsset} logo`}
                          className="rounded-full bg-gray-50 ring-2 ring-white h-6 w-6 object-contain mt-1"
                          height="32"
                          src={`https://cryptofonts.com/img/SVG/${rule.instrument.quoteAsset.toLowerCase()}.svg`}
                          width="32"
                        />
                      )}
                    </div>
                    <div>
                      <div className="flex items-baseline space-x-2">
                        <h3 className="text-lg font-medium">{rule.name}</h3>
                        {rule.lastTriggeredAt && (
                          <p className="text-muted-foreground text-xs">
                            Last triggered: {formatDistanceToNow(rule.lastTriggeredAt, { addSuffix: true })}
                          </p>
                        )}
                      </div>
                      <p className="text-muted-foreground line-clamp-1">{rule.description}</p>
                    </div>
                  </div>
                  <Link to={`/rules/${rule.id}`} className="text-primary">
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

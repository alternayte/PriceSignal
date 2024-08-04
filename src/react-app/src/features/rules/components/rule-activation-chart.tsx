import {PriceInterval, PriceRule} from "@/gql/graphql"
import {useEffect, useRef} from "react";
import {Chart, dispose, init} from "klinecharts";
import {graphql} from "@/gql";
import {useQuery} from "@apollo/client";
import {MAX_SLICE_SIZE} from "@/constants";

const getPricesRuleQuery = graphql(`
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

const CHART_ID = 'activation-chart'

type RuleActivationChartProps = {
    rule: PriceRule
}

export const RuleActivationChart = ({rule}:RuleActivationChartProps) => {
    const chart = useRef<Chart | null>();
    const {data, error, loading} = useQuery(getPricesRuleQuery, {
        variables: {
            symbol: rule.instrument.symbol,
            last: MAX_SLICE_SIZE,
            interval: PriceInterval.OneHour
        }
    });

    useEffect(() => {
        if (data?.prices?.nodes?.length === 0) return;
        const klines =
            data?.prices?.nodes?.map((price) => ({
                timestamp: price.timestamp,
                open: price.open,
                high: price.high,
                low: price.low,
                close: price.close,
                volume: price.volume,
            })) || [];
        
        const findClosestIndex = (timestamp:any) => {
            const targetTime = timestamp
            for (let i = 0; i < klines.length - 1; i++) {
                if (targetTime >= klines[i].timestamp && targetTime <= klines[i + 1].timestamp) {
                    return i;
                }
            }
            return -1;
        };
        
        chart.current = init(CHART_ID)
        chart.current?.applyNewData(klines)
        chart.current?.createIndicator('RSI',true,{id:CHART_ID})
        chart.current?.overrideIndicator({name:'RSI',calcParams:[14]},CHART_ID)
        
        const annotationPoints = rule.activationLogs?.nodes?.map((log) => {
            return {
                value: log.price,
                dataIndex: findClosestIndex(log.triggeredAt),
                timestamp: log.triggeredAt
            }
        })
        
        annotationPoints?.forEach((point) => {
            if (point.dataIndex === -1) return;
            chart.current?.createOverlay({
                name: 'simpleAnnotation',
                extendData: new Date(point.timestamp).toLocaleString(),
                points: [{value: point.value, dataIndex: point.dataIndex}],
            })
        })
        
        return () => {
            dispose(CHART_ID)
        }
    }, [data])
    

    if (loading) return <p>Loading...</p>;
    if (error || !data) return <p>Please try again later</p>;
    
    return <div className="h-full mx-auto max-w-7xl px-4 py-6 sm:px-6 md:px-8 h-[600px]">
        <div id={CHART_ID} style={{height: '100%'}}/>
    </div>


}
import { useEffect, useRef } from 'react'
import { init, dispose, KLineData, Chart } from 'klinecharts'
import { MAX_SLICE_SIZE } from '@/constants'

const CHART_ID = 'chart'

// const options:  Options | undefined = {
//    
// }
type Props = {
    data: KLineData[]
}
export const PricesChart = ({data}:Props) => {
    const chart = useRef<Chart | null>();
    
    useEffect(() => {
        chart.current = init(CHART_ID)
        chart.current?.applyNewData(data)
        chart.current?.createIndicator('RSI',true,{id:CHART_ID})
        return () => {
            dispose(CHART_ID)
        }
    }, [])

    useEffect(() => {
        const lastData = data[data.length - 1]
        lastData.low = Math.min(lastData.open, lastData.close);
        lastData.high = Math.max(lastData.open, lastData.close);
        chart.current?.updateData(lastData);
    }, [data]);

    return <div id={CHART_ID} style={{ height: '100%' }}/>
}
import { useEffect } from 'react'
import { init, dispose, KLineData } from 'klinecharts'

type Props = {
    data: KLineData[]
}
export const PricesChart = ({data}:Props) => {
    useEffect(() => {
        const chart = init('chart')

        chart.applyNewData(data)

        return () => {
            dispose('chart')
        }
    }, [])

    return <div id="chart" style={{ height: '100%' }}/>
}
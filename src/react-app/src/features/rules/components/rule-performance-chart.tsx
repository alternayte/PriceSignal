import { Card, CardContent, CardDescription, CardHeader, CardTitle } from "@/components/ui/card"

type RulePerformanceChartProps = {
    id: string
}
export const RulePerformanceChart = ({}:RulePerformanceChartProps) => {
    return (
        <Card>
            <CardHeader>
                <CardTitle>Performance</CardTitle>
                <CardDescription>
                    Growth and performance of the rule
                </CardDescription>
            </CardHeader>
            <CardContent>
                <div className="grid gap-6">
                    <div className="grid gap-3">
                        {/*<RulePerformanceChart id={id!}/>*/}
                    </div>
                </div>
            </CardContent>
        </Card>
    )
}
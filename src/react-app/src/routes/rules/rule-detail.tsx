import { graphql } from "@/gql";
import {useQuery} from "@apollo/client";
import {redirect, useParams} from "react-router-dom";
import {ActivationLogsEdge, ConditionType,PriceRule} from '@/gql/graphql'
import { AdditionalMetadata } from "@/features/rules/types";
import { EditRule } from "@/features/rules/components/edit-rule";
import { ContentLayout } from "@/components/layouts/content";
import { RulePerformanceChart } from "@/features/rules/components/rule-performance-chart";
import { RuleActivityLog } from "@/features/rules/components/rule-activity-log";
import { RuleActivationChart } from "@/features/rules/components/rule-activation-chart";
import {Tabs, TabsContent, TabsList, TabsTrigger } from "@/components/ui/tabs";
import { Card, CardContent, CardDescription, CardHeader, CardTitle } from "@/components/ui/card";
import { Badge } from "@/components/ui/badge";
import { RuleToggle } from "@/features/rules/components/rule-toggle";
import { cn } from "@/lib/utils";

export const ActivationLog = graphql(`
  fragment ActivationLog on PriceRuleTriggerLog {
    id
    triggeredAt
    price
  }
`)

const ruleDetailQuery = graphql(`
query GetPriceRule($id: UUID!) {
  priceRule(id: $id) {
    description
    id
    isEnabled
    name
    instrument {
      id
      symbol
    }
    createdAt
    activationLogs (first: 10) {
        nodes {
            ...ActivationLog
        }
    }
    conditions(first: 10) {
      totalCount
      edges {
        cursor
        node {
          value
          additionalValues
          conditionType
        }
      }
    }
  }
}
`)



export const RuleDetail = () => {

    const {id} = useParams();

    const {data, loading } = useQuery(ruleDetailQuery,{variables:{id}})

    if (loading) return <p>Loading...</p>;
    if (!data) return <p>Rule Not found</p>;
    if (data.priceRule == null) return redirect('/rules')
    const ResolveConditionType = (value:string) => {
        var types = Object.entries(ConditionType)
        var f = types.find(([_,y])=>y == value);
        if (f) {
            return f[0]
        }
        return ""
    }
    
    return (
        <ContentLayout title={`${data.priceRule.name!} - ${data.priceRule.instrument.symbol}`}>
            <div className="grid auto-rows-max items-start gap-4 lg:col-span-2 lg:gap-8">
                <Card x-chunk="dashboard-07-chunk-0">
                    <CardHeader>
                        <div className='flex w-full justify-between items-center'>
                            <div className='flex align-baseline items-center gap-x-6'>
                                <CardTitle>Rule Info</CardTitle>
                                <RuleToggle rule={data.priceRule}/>
                                <Badge className={cn("h-min",{"bg-green-500":data.priceRule.isEnabled,"bg-gray-400":!data.priceRule.isEnabled})}>Active</Badge>
                            </div>
                            
                            <EditRule data={data.priceRule as PriceRule}/>                            
                        </div>
                        <CardDescription>
                            {data.priceRule.description}
                        </CardDescription>
                        
                    </CardHeader>
                    <CardContent>
                        <div className="grid gap-6">
                            <div className="grid gap-0">
                                {data.priceRule.conditions?.edges?.map((c, idx) => {
                                    var additional: AdditionalMetadata = JSON.parse(c.node.additionalValues as string)

                                    return (
                                        <div key={idx}>
                                            <div className='space-y-0 flex flex-row flex-wrap gap-3 items-baseline'>
                                                <span className='flex-0'>When</span>
                                                <span className='text-blue-700'>{ResolveConditionType(c.node.conditionType)}</span>
                                                <span className='flex-0'>has</span>
                                                <span className='text-blue-700'>{additional.name} ({additional.period})</span>
                                                <span className='flex-0'>going</span>
                                                <span className='text-blue-700'>{additional.direction?.toLowerCase()}</span>
                                                <span className='text-blue-700'>{c.node.value}</span>
                                            </div>
                                            {idx + 1 < (data.priceRule?.conditions?.totalCount ?? 0) && (
                                                <div> AND </div>
                                            )}
                                        </div>
                                    )
                                })}
                            </div>
                        </div>
                    </CardContent>
                </Card>
                {/*<RulePerformanceChart id={id!}/>*/}
                <Card>
                    <CardHeader>
                        <CardTitle>Activity</CardTitle>

                        <CardDescription>
                            Activity of the rule
                        </CardDescription>
                    </CardHeader>
                    <CardContent>
                        <div className="grid gap-6">
                            <div className="grid gap-3">
                                <Tabs defaultValue="log" className="w-full">
                                    <TabsList>
                                        <TabsTrigger value="log">Activity Log</TabsTrigger>
                                        <TabsTrigger value="chart">Activity Chart</TabsTrigger>
                                    </TabsList>
                                    <TabsContent value="log">
                                        <RuleActivityLog data={data.priceRule.activationLogs?.nodes as ActivationLogsEdge[]}/>
                                    </TabsContent>
                                    <TabsContent value="chart">
                                        <RuleActivationChart rule={data.priceRule}/>
                                    </TabsContent>
                                </Tabs>                            </div>
                        </div>
                    </CardContent>

                </Card>
                
                


            </div>
        </ContentLayout>

    );
};
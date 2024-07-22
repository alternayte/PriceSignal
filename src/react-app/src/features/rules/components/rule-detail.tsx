import { graphql } from "@/gql";
import {useQuery} from "@apollo/client";
import {redirect, useParams} from "react-router-dom";
import {ConditionType} from '@/gql/graphql'


const ruleDetailQuery = graphql(`
query GetPriceRule($id: UUID!) {
  priceRule(id: $id) {
    description
    id
    name
    instrument {
      symbol
    }
    createdAt
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

type AdditionalMetadata = {
    name: string
    period: number
    direction: 'Above' | 'Below'
}

export const RuleDetail = () => {
    // TODO: move to routes/rules/folder
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
        <div>
            <h3 className='text-xl'>Detail - {data.priceRule.name}</h3>
            <p>{data.priceRule.description}</p>
            
            {/*<EditRule/>*/}
            <div>
                {data.priceRule.conditions?.edges?.map((c,idx)=> {
                    var additional: AdditionalMetadata = JSON.parse(c.node.additionalValues as string)
                    
                    return (
                        <div>
                        <div className='space-y-4 flex flex-row flex-wrap gap-3 items-baseline'>
                            <span className='flex-0'>When</span>
                            <span className='text-blue-700'>{ResolveConditionType(c.node.conditionType)}</span>
                            <span className='flex-0'>has</span>
                            <span className='text-blue-700'>{additional.name} ({additional.period})</span>
                            <span className='flex-0'>going</span>
                            <span className='text-blue-700'>{additional.direction.toLowerCase()}</span>
                            <span className='text-blue-700'>{c.node.value}</span>
                        </div>
                            {idx+1 < (data.priceRule?.conditions?.totalCount ?? 0) && (
                                <div> AND </div>
                            ) }
                        </div>
                    )
                })}
            </div>
            {/*<RulePerformanceChart/>*/}
            {/*<RuleActivationHistoryTable/>*/}
        </div>
    );
};
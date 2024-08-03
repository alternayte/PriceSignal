import { MoreHorizontal } from "lucide-react"

import { Badge } from "@/components/ui/badge"
import { Button } from "@/components/ui/button"
import {
    Card,
    CardContent,
    CardDescription,
    CardFooter,
    CardHeader,
    CardTitle,
} from "@/components/ui/card"
import {
    DropdownMenu,
    DropdownMenuContent,
    DropdownMenuItem,
    DropdownMenuLabel,
    DropdownMenuTrigger,
} from "@/components/ui/dropdown-menu"
import {
    Table,
    TableBody,
    TableCell,
    TableHead,
    TableHeader,
    TableRow,
} from "@/components/ui/table"
import { graphql } from "@/gql"
import {useMutation, useQuery} from "@apollo/client";
import {format} from "date-fns";
import {useNavigate} from "react-router-dom";
import { RuleToggle } from "./rule-toggle"
import { PriceRule } from "@/gql/graphql"
import { cn } from "@/lib/utils"

const allRulesQuery = graphql(`
query GetPriceRules($first: Int) {
  priceRules(first: $first) {
    edges {
      node {
        description
        isEnabled
        id
        name
        instrument {
          symbol
        }
        createdAt
      }
    }
    totalCount
    pageInfo {
      hasPreviousPage
      hasNextPage
    }
  }
}
`)

const deleteRuleMutation = graphql(`
mutation DeletePriceRule($id: UUID!) {
  deletePriceRule(id: $id) {
    id
  }
}
`)

export const RulesList = () => {
    const { data, loading } = useQuery(allRulesQuery, {variables: {first: 10}});
    const [deleteRule] = useMutation(deleteRuleMutation,{});
    const navigate = useNavigate();
    
    const handleDeleteRule = async (id: string) => {
        try {
            await deleteRule({variables: {id},update: (cache) => {
                cache.evict({id: `PriceRule:${id}`});
            }});
        } catch (e) {
            console.error(e);
        }
    }
    
    const handleViewDetail = (id:string) => {
        navigate(`/rules/${id}`)
    }

    if (loading) return <p>Loading...</p>;
    if (!data) return <p>No data</p>;
    
    return (
        <Card>
            <CardHeader>
                <CardTitle>Rules</CardTitle>
                <CardDescription>
                    Manage your rules and their conditions
                </CardDescription>
            </CardHeader>
            <CardContent>
                <Table>
                    <TableHeader>
                        <TableRow>
                            <TableHead className="hidden w-[100px] sm:table-cell">
                                <span className="sr-only">Image</span>
                            </TableHead>
                            <TableHead>Name</TableHead>
                            <TableHead>Status</TableHead>
                            <TableHead className="hidden md:table-cell">Symbol</TableHead>
                            <TableHead className="hidden md:table-cell">Created at</TableHead>
                            <TableHead>
                                <span className="sr-only">Actions</span>
                            </TableHead>
                        </TableRow>
                    </TableHeader>
                    <TableBody>
                        {data && data?.priceRules?.edges?.map((rule) => (
                            <TableRow key={rule.node.id}>
                                <TableCell className="hidden sm:table-cell">
                                    {/*<Image*/}
                                    {/*    alt="Product image"*/}
                                    {/*    className="aspect-square rounded-md object-cover"*/}
                                    {/*    height="64"*/}
                                    {/*    src="/placeholder.svg"*/}
                                    {/*    width="64"*/}
                                    {/*/>*/}
                                </TableCell>
                                <TableCell className="font-medium">
                                    {rule.node.name}
                                </TableCell>
                                <TableCell className='flex items-center gap-x-2'>
                                    <RuleToggle rule={rule.node as PriceRule}/>
                                    <Badge className={cn("h-min",{"bg-green-500":rule.node.isEnabled,"bg-gray-400":!rule.node.isEnabled})}>Active</Badge>
                                </TableCell>
                                <TableCell className="hidden md:table-cell">{rule.node.instrument.symbol}</TableCell>
                                <TableCell className="hidden md:table-cell">
                                    {format(new Date(rule.node.createdAt), "MMM dd, yyyy")}
                                </TableCell>
                                <TableCell>
                                    <DropdownMenu>
                                        <DropdownMenuTrigger asChild>
                                            <Button aria-haspopup="true" size="icon" variant="ghost">
                                                <MoreHorizontal className="h-4 w-4" />
                                                <span className="sr-only">Toggle menu</span>
                                            </Button>
                                        </DropdownMenuTrigger>
                                        <DropdownMenuContent align="end">
                                            <DropdownMenuLabel>Actions</DropdownMenuLabel>
                                            <DropdownMenuItem onClick={()=> handleViewDetail(rule.node.id)}>View</DropdownMenuItem>
                                            <DropdownMenuItem onClick={()=> handleDeleteRule(rule.node.id)}>Delete</DropdownMenuItem>
                                        </DropdownMenuContent>
                                    </DropdownMenu>
                                </TableCell>
                            </TableRow>
                        ))}
                    </TableBody>
                </Table>
            </CardContent>
            <CardFooter>
                <div className="text-xs text-muted-foreground">
                    Showing <strong>1-{data.priceRules?.edges?.length}</strong> of <strong>{data.priceRules?.totalCount}</strong> products
                </div>
            </CardFooter>
        </Card>
    )
}
    
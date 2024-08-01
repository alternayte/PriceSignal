import {ActivationLogsEdge} from '@/gql/graphql'
import {format} from "date-fns";
import {
    Table,
    TableBody,
    TableCell,
    TableHead,
    TableHeader,
    TableRow,
} from "@/components/ui/table"

type RuleActivityLogProps = {
    data: ActivationLogsEdge[]
}
export const RuleActivityLog = ({data}:RuleActivityLogProps) => {
    console.log(data)
    return (
        <Table>
            <TableHeader>
                <TableRow>
                    <TableHead>Detail</TableHead>
                    <TableHead>Triggered at</TableHead>
                </TableRow>
            </TableHeader>
            <TableBody>
                {data?.map((log) => (
                    <TableRow key={log.node.id}>
                        <TableCell className="font-medium">
                            {new Intl.NumberFormat().format(log.node.price)}
                        </TableCell>
                        <TableCell className='flex items-center gap-x-2'>
                            {format(new Date(log.node.triggeredAt), "PPPpp")}
                        </TableCell>
                    </TableRow>
                ))}
            </TableBody>
        </Table>
    )
}
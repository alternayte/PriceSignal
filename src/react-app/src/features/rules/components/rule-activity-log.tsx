import {PriceRuleTriggerLog} from '@/gql/graphql'
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
    data: PriceRuleTriggerLog[]
}
export const RuleActivityLog = ({data}:RuleActivityLogProps) => {
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
                    <TableRow key={log.id}>
                        <TableCell className="font-medium">
                            {new Intl.NumberFormat().format(log.price)}
                        </TableCell>
                        <TableCell className='flex items-center gap-x-2'>
                            {format(new Date(log.triggeredAt), "PPPpp")}
                        </TableCell>
                    </TableRow>
                ))}
            </TableBody>
        </Table>
    )
}
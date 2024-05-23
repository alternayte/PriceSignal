import {FragmentType, useFragment } from "@/gql";

import { PriceItem } from "./focused-prices-list";
import { Card, CardContent, CardTitle, CardHeader } from "@/components/ui/card";
import { formatDistanceToNow } from "date-fns";
import { CircularProgress } from "@/components/common/circular-progress";


type Props = {
    price: FragmentType<typeof PriceItem>;
    progress: number;
}
export const PriceCard = ({ price, progress }:Props) => {
    const fprice = useFragment(PriceItem, price)
    return (
        <Card>
            <CardHeader className="flex flex-row items-center justify-between pb-2">
                <CardTitle className="text-sm font-medium">({fprice.symbol})</CardTitle>
                {/*<CoinsIcon className="w-4 h-4 text-gray-500 dark:text-gray-400" />*/}
                <CircularProgress size={20} strokeWidth={3} progress={progress}  />
            </CardHeader>
            <CardContent>
                <div className="text-2xl font-bold">${fprice.close}</div>
                <p className="text-xs text-gray-500 dark:text-gray-400">Last updated: {formatDistanceToNow(fprice.bucket)}</p>
            </CardContent>
        </Card>
    );
}
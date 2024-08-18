import { Button } from '@/components/ui/button';
import { Card, CardContent, CardDescription, CardHeader, CardTitle } from '@/components/ui/card';
import { useAuth } from '@/features/auth/auth-context';
import { Link } from 'react-router-dom';
import {Send, Unplug} from "lucide-react";
import { graphql } from "@/gql";
import {NotificationChannelType} from '@/gql/graphql';
import {useMutation, useQuery} from "@apollo/client";


const userProfileQuery = graphql(`
query UserSettings {
  user {
    email
    notificationChannels(first: 10) {
      nodes {
        id
        channelType
        telegramUsername
      }
    }
  }
}
`)

const disconnectTelegramMutation = graphql(`
mutation DisconnectTelegram {
  deleteTelegramConnection
}
`)
export const NotificationSettings = () => {
    const auth = useAuth();
    const { data, loading } = useQuery(userProfileQuery);
    const [disconnectTelegram] = useMutation(disconnectTelegramMutation,{});

    if (loading) return <p>Loading...</p>;
    
    const telegramNode = data?.user?.notificationChannels?.nodes?.find((node: any) => node.channelType === NotificationChannelType.Telegram);
    const TelegramToggle = () => {
        if (telegramNode === undefined) {
            return (<Link to={`https://t.me/price_signal_nxt_bot?start=${auth?.user?.uid}`} target='_blank'>
                <Button>
                    <Send className="mr-2 h-4 w-4" />Connect to Telegram
                </Button>
            </Link>);
        } else {
            return (<>
            <p>@{telegramNode.telegramUsername}</p>
            <Button variant='destructive' onClick={()=> disconnectTelegram()}>
                <Unplug className="mr-2 h-4 w-4"/>Disconnect Telegram
            </Button>
            </>);
        }
    }


    return (
        <Card>
        <CardHeader>
                <CardTitle>Notifications</CardTitle>
                <CardDescription>
                    Customize your notification preferences across different channels.
                </CardDescription>
            </CardHeader>
            <CardContent>
                <div>
                    <h2 className="text-xl md:text-2xl font-semibold">Telegram</h2>
                    <div className="mt-4 grid md:grid-cols-2 gap-4">
                        <div className="flex items-center justify-between">
                            <div>
                                <p className="font-medium">Receive Telegram notifications</p>
                                <p className="text-muted-foreground text-sm">
                                    Get notified about important updates and activity on Telegram.
                                </p>
                            </div>
                            {/*<Switch id="telegram-notifications" className="ml-auto"/>*/}
                        </div>
                        <div className="flex items-center justify-end space-x-4">
                            <TelegramToggle/>
                        </div>
                    </div>
                </div>
            </CardContent>
        </Card>
    );
};




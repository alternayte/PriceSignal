import { ContentLayout } from '@/components/layouts/content';
import { Input } from '@/components/ui/input';
import {TabsContent, TabsTrigger, TabsList } from '@/components/ui/tabs';
import { Tabs } from '@/components/ui/tabs';
import { useAuth } from '@/features/auth/auth-context';
import { NotificationSettings } from '@/features/settings/components/notification-settings';
import { Label } from '@radix-ui/react-label';

export const Settings = () => {
    const auth = useAuth();

    return (
        <ContentLayout title='Settings'>
                        <Tabs defaultValue="notifications" className="w-full">
                            <TabsList>
                                <TabsTrigger value="general">General</TabsTrigger>
                                <TabsTrigger value="notifications">Notifications</TabsTrigger>
                            </TabsList>
                            <TabsContent value="general">
                                <div className='max-w-md'>
                                    <Label className=''>Email</Label>
                                    <Input disabled type="email" value={auth?.user?.email ?? undefined} className='truncate' />    
                                </div>
                            </TabsContent>
                            <TabsContent value="notifications">
                                <NotificationSettings/>
                            </TabsContent>
                        </Tabs>                            
        </ContentLayout>
    );
};
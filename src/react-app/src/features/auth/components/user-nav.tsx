
import { DropdownMenu, DropdownMenuContent, DropdownMenuItem, DropdownMenuSeparator, DropdownMenuTrigger } from '@/components/ui/dropdown-menu';
import { Avatar, AvatarFallback, AvatarImage } from '@/components/ui/avatar';
import { useAuth } from '@/features/auth/auth-context';
import { Link } from 'react-router-dom';

import { useEffect, useState } from 'react';
import {Button, buttonVariants } from '@/components/ui/button';
import { cn } from '@/lib/utils';

type UserNavProps = {
    onClick?: () => void
}
export const UserNav = ({ onClick }: UserNavProps) => {
    const [loading, setLoading] = useState(true);
    const auth = useAuth();

    const user = auth?.user;
    const userLoading = auth?.userLoading;

    useEffect(() => {}, [userLoading]);

    if (userLoading) {
        return null;
    }
    if (!user?.uid) {
        return (
            <nav>
                <Button className={cn(buttonVariants({ variant: 'secondary', size: 'sm' }), 'px-4')} onClick={()=> auth?.signinWithProvider('google')}>
                    Login
                </Button>
            </nav>
        );
    }

    // get initials ofuser displayName
    const initials = user?.displayName
        ?.split(' ')
        .map((n) => n[0])
        .join('');

    return (
        <DropdownMenu>
            <DropdownMenuTrigger>
                <Avatar className="h-8 w-8">
                    <AvatarImage alt={user?.displayName!} src={user?.photoURL!} />
                    <AvatarFallback>{initials}</AvatarFallback>
                </Avatar>
            </DropdownMenuTrigger>
            <DropdownMenuContent align="end">
                <div className="flex items-center justify-start gap-2 p-2">
                    <div className="flex flex-col space-y-1 leading-none">
                        {user?.displayName && <p className="font-medium">{user?.displayName}</p>}
                        {user?.email && <p className="w-[200px] truncate text-sm text-muted-foreground">{user?.email}</p>}
                    </div>
                </div>
                <DropdownMenuSeparator />
                <DropdownMenuItem asChild onSelect={() => onClick && onClick()}>
                    <Link to="/dashboard">Dashboard</Link>
                </DropdownMenuItem>
                <DropdownMenuItem asChild onSelect={() => onClick && onClick()}>
                    <Link to="/dashboard/profile">Profile</Link>
                </DropdownMenuItem>
                {/* <DropdownMenuItem asChild>
          <Link href="/dashboard/settings">Settings</Link>
        </DropdownMenuItem> */}
                <DropdownMenuSeparator />
                <DropdownMenuItem
                    className="cursor-pointer"
                    onSelect={(event: Event) => {
                        onClick && onClick();
                        event.preventDefault();
                        auth?.signout();
                        // signOut({
                        //   callbackUrl: `${window.location.origin}/login`,
                        // })
                    }}
                >
                    Sign out
                </DropdownMenuItem>
            </DropdownMenuContent>
        </DropdownMenu>
    );
};

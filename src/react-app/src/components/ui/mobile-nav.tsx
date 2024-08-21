import React from 'react';
import { Sheet, SheetContent, SheetTrigger } from './sheet';
import { Button } from './button';
import { Menu } from 'lucide-react';
import {Link, useLocation} from 'react-router-dom';
import { cn } from '@/lib/utils';

type MobileNavProps = {
    items: { label: string, to: string, icon:  React.ForwardRefExoticComponent<Omit<any, "ref"> & React.RefAttributes<SVGSVGElement>> }[]
}
export const MobileNav = ({items}:MobileNavProps) => {
    const location = useLocation();

    const isActive = (path: string) => location.pathname === path;
    return (
        <Sheet>
            <SheetTrigger asChild>
                <Button
                    variant="outline"
                    size="icon"
                    className="shrink-0 lg:hidden"
                >
                    <Menu className="h-5 w-5" />
                    <span className="sr-only">Toggle navigation menu</span>
                </Button>
            </SheetTrigger>
            <SheetContent side="left">
                <nav className="grid gap-6 text-lg font-medium">
                    {items.map((item) => (
                        
                        <Link key={item.label}
                              to={item.to}
                              className={cn('flex items-center gap-3 rounded-lg px-3 py-2 transition-all', {
                                  'bg-gray-100 text-gray-900 dark:bg-gray-800 dark:text-gray-50': isActive(item.to),
                                  'text-gray-500 dark:text-gray-400 hover:text-gray-900 dark:hover:text-gray-50': !isActive(item.to),
                              })}
                        >
                            <item.icon className="h-4 w-4"/>
                            {item.label}
                        </Link>
                    ))}
                </nav>
            </SheetContent>
        </Sheet>
    );
};
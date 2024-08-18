import { cn } from '@/lib/utils';
import { LucideProps } from 'lucide-react';
import React from 'react';
import {Link, useLocation} from "react-router-dom";

type MainNavProps = {
    items: { label: string, to: string, icon:  React.ForwardRefExoticComponent<Omit<LucideProps, "ref"> & React.RefAttributes<SVGSVGElement>> }[]
}
export const MainNav = ({items}:MainNavProps) => {
    const location = useLocation();

    const isActive = (path: string) => location.pathname === path;
    return (
        <div className="hidden border-r bg-gray-100/40 lg:block dark:bg-gray-800/40 min-w-[250px]">
            <div className="flex h-full max-h-screen flex-col gap-2">
                <div className="flex-1 overflow-auto py-2">
                    <nav className="grid items-start px-4 text-sm font-medium">
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
                </div>
            </div>
        </div>
    );
};
import { SymbolsSearch } from '@/features/symbols/components/symbols-search';
import { cn } from '@/lib/utils';
import {CoinsIcon, HomeIcon, PieChartIcon, PuzzleIcon, ReplaceIcon, SettingsIcon, WalletIcon} from 'lucide-react';
import React from 'react';
import { Link, useLocation } from 'react-router-dom';
import { UserNav } from '@/features/auth/components/user-nav';

const navItems = [
  {
    label: 'Dashboard',
    icon: HomeIcon,
    to: '/',
  },
  {
    label: 'Rules',
    icon: PuzzleIcon,
    to: '/rules',
  },
  // {
  //   label: 'Wallet',
  //   icon: WalletIcon,
  //   to: '/wallet',
  // },
  // {
  //   label: 'Exchange',
  //   icon: ReplaceIcon,
  //   to: '/exchanges',
  // },
  {
    label: 'Analytics',
    icon: PieChartIcon,
    to: '/symbols/BTCUSDT',
  },
  {
    label: 'Settings',
    icon: SettingsIcon,
    to: '/settings',
  },
];
export const DashboardLayout = ({ children }: { children: React.ReactNode }) => {
  const location = useLocation();

  const isActive = (path: string) => location.pathname === path;
  return (
    <div className="flex min-h-screen w-full flex-col">
      <header className="flex h-16 items-center justify-between border-b bg-gray-100 px-6 dark:bg-gray-950">
        <div className="flex items-center gap-4">
          <Link className="flex items-center gap-2 text-lg font-semibold" to="/">
            <CoinsIcon className="h-6 w-6" />
            <span>Signal Dashboard</span>
          </Link>
        </div>
        <div className="flex items-center gap-4">
          <SymbolsSearch />
        </div>
        <UserNav/>
      </header>
      <div className="flex flex-1">
        <div className="hidden border-r bg-gray-100/40 lg:block dark:bg-gray-800/40 min-w-[250px]">
          <div className="flex h-full max-h-screen flex-col gap-2">
            <div className="flex-1 overflow-auto py-2">
              <nav className="grid items-start px-4 text-sm font-medium">
                {navItems.map((item) => (
                  <Link key={item.label}
                    to={item.to}
                    className={cn('flex items-center gap-3 rounded-lg px-3 py-2 transition-all', {
                      'bg-gray-100 text-gray-900 dark:bg-gray-800 dark:text-gray-50': isActive(item.to),
                      'text-gray-500 dark:text-gray-400 hover:text-gray-900 dark:hover:text-gray-50': !isActive(item.to),
                    })}
                  >
                    <item.icon className="h-4 w-4" />
                    {item.label}
                  </Link>
                ))}
              </nav>
            </div>
          </div>
        </div>
        <div className="flex flex-1 flex-col">
          <main className="flex flex-1 flex-col gap-4 p-4 md:gap-8 md:p-6">{children}</main>
        </div>
      </div>
    </div>
  );
};

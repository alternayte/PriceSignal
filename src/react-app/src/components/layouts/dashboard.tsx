import { SymbolsSearch } from '@/features/symbols/components/symbols-search';
import {CoinsIcon, HomeIcon, PieChartIcon, PuzzleIcon, SettingsIcon,LayoutDashboard} from 'lucide-react';
import React from 'react';
import { Link } from 'react-router-dom';
import { UserNav } from '@/features/auth/components/user-nav';
import { MainNav } from '../ui/main-nav';
import { MobileNav } from '../ui/mobile-nav';

const navItems = [
  {
    label: 'Home',
    icon: HomeIcon,
    to: '/',
  },
  {
    label: 'Dashboard',
    icon: LayoutDashboard,
    to: '/dashboard',
  },
  {
    label: 'Rules',
    icon: PuzzleIcon,
    to: '/dashboard/rules',
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

  return (
    <div className="flex min-h-screen w-full flex-col">
      <header className="flex h-16 items-center justify-between border-b bg-gray-100 px-6 dark:bg-gray-950">
        <div className="flex items-center gap-4">
          <MobileNav items={navItems}/>
          <Link className="flex items-center gap-2 text-lg font-semibold" to="/">
            <CoinsIcon className="h-6 w-6" />
            <span className='hidden md:flex'>Signal Dashboard</span>
          </Link>
        </div>
        <div className="flex items-center gap-4">
          <SymbolsSearch />
        </div>
        <UserNav/>
      </header>
      <div className="flex flex-1">
        <MainNav items={navItems}/>
        <div className="flex flex-1 flex-col">
          <main className="flex flex-1 flex-col gap-4 p-4 md:gap-8 md:p-6">{children}</main>
        </div>
      </div>
    </div>
  );
};

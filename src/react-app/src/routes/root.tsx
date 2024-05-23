import { DashboardLayout } from '@/components/layouts/dashboard';
import { Suspense } from 'react';
import { Outlet } from 'react-router-dom';

export const Root = () => {
  return (
    <DashboardLayout>
      <Suspense
        fallback={
          <div className="flex size-full items-center justify-center">
            <div className="loader">Loading...</div>
          </div>
        }
      >
        <Outlet />
      </Suspense>
    </DashboardLayout>
  );
};

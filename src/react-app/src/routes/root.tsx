import { DashboardLayout } from '@/components/layouts/dashboard';
import { AuthProvider } from '@/features/auth/components/auth-provider';
import { Suspense } from 'react';
import { Outlet } from 'react-router-dom';

export const Root = () => {
  return (
      <AuthProvider>
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
    </AuthProvider>
  );
};

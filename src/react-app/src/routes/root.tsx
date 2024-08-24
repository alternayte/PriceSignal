import { DashboardLayout } from '@/components/layouts/dashboard';
import { PublicLayout } from '@/components/layouts/public';
import { useAuth } from '@/features/auth/auth-context';
import { Suspense } from 'react';
import { Navigate, Outlet } from 'react-router-dom';

export const Root = () => {
  const auth = useAuth();
  if (!auth?.isLoggedIn && !auth?.userLoading) return <Navigate to="/" />;
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

export const PublicRoot = () => {
  return (
    <PublicLayout>
      <Suspense
        fallback={
          <div className="flex size-full items-center justify-center">
            <div className="loader">Loading...</div>
          </div>
        }
      >
        <Outlet />
      </Suspense>
    </PublicLayout>
  );
};

import { createBrowserRouter } from 'react-router-dom';
import { PublicRoot, Root } from './root';

export const createRouter = () =>
  createBrowserRouter([
    {
      path: '/',
      element: <PublicRoot />,
      children: [
        {
          path: '/',
          lazy: async () => {
            const { HomeRoute } = await import('@/routes/home/home');
            return { Component: HomeRoute };
          },
        },
        {
          path: '/symbols/:symbol',
          lazy: async () => {
            const { SymbolRoute } = await import('@/routes/symbols/symbol');
            return { Component: SymbolRoute };
          },
          loader: () => {
            return <div>Loading...</div>;
          },
        },
      ],
    },
    {
      path: '/dashboard',
      element: <Root />,
      children: [
        {
          path: '/dashboard',
          lazy: async () => {
            const { PricesRoute } = await import('@/routes/prices/prices');
            return { Component: PricesRoute };
          },
          loader: () => {
            return <div>Loading...</div>;
          },
        },
        {
          path: '/dashboard/exchanges',
          lazy: async () => {
            const { ExchangesRoute } = await import('@/routes/exchanges/exchanges');
            return { Component: ExchangesRoute };
          },
          loader: () => {
            return <div>Loading...</div>;
          },
        },
        {
          path: '/dashboard/rules',
          lazy: async () => {
            const { RulesRoute } = await import('@/routes/rules/rules');
            return { Component: RulesRoute };
          },
          loader: () => {
            return <div>Loading...</div>;
          },
        },
        {
          path: '/dashboard/rules/:id',
          // @ts-ignore
          lazy: async () => {
            const { RuleDetail } = await import('@/routes/rules/rule-detail');
            return { Component: RuleDetail };
          },
          loader: () => {
            return <div>Loading Rule...</div>;
          },
        },
        {
          path: '/dashboard/symbols/:symbol',
          lazy: async () => {
            const { SymbolRoute } = await import('@/routes/symbols/symbol');
            return { Component: SymbolRoute };
          },
          loader: () => {
            return <div>Loading...</div>;
          },
        },
        {
          path: '/dashboard/settings',
          lazy: async () => {
            const { Settings } = await import('@/routes/settings/settings');
            return { Component: Settings };
          },
          loader: () => {
            return <div>Loading...</div>;
          },
        },
      ],
    },
    {
      path: '/contact',
      element: <div>Contact</div>,
    },

    {
      path: '/login',
      element: <div>Login</div>,
    },
    {
      path: '/register',
      element: <div>Register</div>,
    },
    {
      path: '/forgot-password',
      element: <div>Forgot Password</div>,
    },
    {
      path: '/reset-password',
      element: <div>Reset Password</div>,
    },
    {
      path: '/404',
      element: <div>404</div>,
    },
    {
      path: '*',
      element: <div>404</div>,
    },
  ]);

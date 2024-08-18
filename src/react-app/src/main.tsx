import React, { Suspense } from 'react';
import ReactDOM from 'react-dom/client';
import App from './App.tsx';
import './index.css';
import { ApolloClient, InMemoryCache, ApolloProvider, HttpLink, split } from '@apollo/client';
import { GraphQLWsLink } from '@apollo/client/link/subscriptions';
import { createClient } from 'graphql-ws';
import { getMainDefinition } from '@apollo/client/utilities';

const httpLink = new HttpLink({
    uri: '/graphql',
    credentials: 'include',
});

const wsLink = new GraphQLWsLink(
    createClient({
        url: import.meta.env.VITE_WS_URL as string,
    }),
);

const splitLink = split(
    ({ query }) => {
        const definition = getMainDefinition(query);
        return (
            definition.kind === 'OperationDefinition' &&
            definition.operation === 'subscription'
        );
    },
    wsLink,
    httpLink,
);

const client = new ApolloClient({
    link: splitLink,
    cache: new InMemoryCache({
        // typePolicies: {
        //   Price: {
        //     keyFields: ['symbol', 'bucket'],
        //   },
        // },
    }),

});

const AppProvider = ({ children }: { children: React.ReactNode }) => {
    return (
            <Suspense fallback={<div>Loading...</div>}>
                <ApolloProvider client={client}>{children}</ApolloProvider>
            </Suspense>
    );
};

ReactDOM.createRoot(document.getElementById('root')!).render(
    <React.StrictMode>
        <AppProvider>
            <App />
        </AppProvider>
    </React.StrictMode>,
);

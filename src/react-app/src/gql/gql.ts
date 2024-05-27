/* eslint-disable */
import * as types from './graphql';
import { TypedDocumentNode as DocumentNode } from '@graphql-typed-document-node/core';

/**
 * Map of all GraphQL operations in the project.
 *
 * This map has several performance disadvantages:
 * 1. It is not tree-shakeable, so it will include all operations in the project.
 * 2. It is not minifiable, so the string of a GraphQL query will be multiple times inside the bundle.
 * 3. It does not support dead code elimination, so it will add unused operations.
 *
 * Therefore it is highly recommended to use the babel or swc plugin for production.
 */
const documents = {
    "\nquery GetExchanges($take: Int!) {\n      exchanges(take: $take) {\n        totalCount\n        items {\n          name\n          description\n        }\n      }\n    }\n": types.GetExchangesDocument,
    "\n  fragment PriceItem on Price {\n    bucket\n    close\n    symbol\n    volume\n  }\n": types.PriceItemFragmentDoc,
    "\nquery GetFocusedPrices {\n  ETH:prices(last: 1,interval: ONE_MIN,where: {symbol:{eq:\"ETHUSDT\"}}) {\n    edges {\n      node {\n        ...PriceItem\n      }\n    }\n  }\n  BTC:prices(last: 1,interval: ONE_MIN,where: {symbol:{eq:\"BTCUSDT\"}}) {\n    edges {\n      node {\n        ...PriceItem\n      }\n    }\n  }\n}\n": types.GetFocusedPricesDocument,
    "\n  query GetPricesForSymbol($symbol: String!,$last: Int!) {\n    prices(\n      last: $last\n      where: { symbol: { eq: $symbol } }\n      # order: { bucket: DESC }\n      interval: ONE_MIN\n    ) {\n      nodes {\n        timestamp: bucket\n        close\n        high\n        low\n        open\n        symbol\n        volume\n      }\n    }\n  }\n": types.GetPricesForSymbolDocument,
    "\n  subscription SubscribeToPricesForSymbol($symbol: String!) {\n    onPriceUpdated(symbol: $symbol) {\n      timestamp: bucket\n      close\n      high\n      low\n      open\n      symbol\n      volume\n    }\n  }\n": types.SubscribeToPricesForSymbolDocument,
};

/**
 * The graphql function is used to parse GraphQL queries into a document that can be used by GraphQL clients.
 *
 *
 * @example
 * ```ts
 * const query = graphql(`query GetUser($id: ID!) { user(id: $id) { name } }`);
 * ```
 *
 * The query argument is unknown!
 * Please regenerate the types.
 */
export function graphql(source: string): unknown;

/**
 * The graphql function is used to parse GraphQL queries into a document that can be used by GraphQL clients.
 */
export function graphql(source: "\nquery GetExchanges($take: Int!) {\n      exchanges(take: $take) {\n        totalCount\n        items {\n          name\n          description\n        }\n      }\n    }\n"): (typeof documents)["\nquery GetExchanges($take: Int!) {\n      exchanges(take: $take) {\n        totalCount\n        items {\n          name\n          description\n        }\n      }\n    }\n"];
/**
 * The graphql function is used to parse GraphQL queries into a document that can be used by GraphQL clients.
 */
export function graphql(source: "\n  fragment PriceItem on Price {\n    bucket\n    close\n    symbol\n    volume\n  }\n"): (typeof documents)["\n  fragment PriceItem on Price {\n    bucket\n    close\n    symbol\n    volume\n  }\n"];
/**
 * The graphql function is used to parse GraphQL queries into a document that can be used by GraphQL clients.
 */
export function graphql(source: "\nquery GetFocusedPrices {\n  ETH:prices(last: 1,interval: ONE_MIN,where: {symbol:{eq:\"ETHUSDT\"}}) {\n    edges {\n      node {\n        ...PriceItem\n      }\n    }\n  }\n  BTC:prices(last: 1,interval: ONE_MIN,where: {symbol:{eq:\"BTCUSDT\"}}) {\n    edges {\n      node {\n        ...PriceItem\n      }\n    }\n  }\n}\n"): (typeof documents)["\nquery GetFocusedPrices {\n  ETH:prices(last: 1,interval: ONE_MIN,where: {symbol:{eq:\"ETHUSDT\"}}) {\n    edges {\n      node {\n        ...PriceItem\n      }\n    }\n  }\n  BTC:prices(last: 1,interval: ONE_MIN,where: {symbol:{eq:\"BTCUSDT\"}}) {\n    edges {\n      node {\n        ...PriceItem\n      }\n    }\n  }\n}\n"];
/**
 * The graphql function is used to parse GraphQL queries into a document that can be used by GraphQL clients.
 */
export function graphql(source: "\n  query GetPricesForSymbol($symbol: String!,$last: Int!) {\n    prices(\n      last: $last\n      where: { symbol: { eq: $symbol } }\n      # order: { bucket: DESC }\n      interval: ONE_MIN\n    ) {\n      nodes {\n        timestamp: bucket\n        close\n        high\n        low\n        open\n        symbol\n        volume\n      }\n    }\n  }\n"): (typeof documents)["\n  query GetPricesForSymbol($symbol: String!,$last: Int!) {\n    prices(\n      last: $last\n      where: { symbol: { eq: $symbol } }\n      # order: { bucket: DESC }\n      interval: ONE_MIN\n    ) {\n      nodes {\n        timestamp: bucket\n        close\n        high\n        low\n        open\n        symbol\n        volume\n      }\n    }\n  }\n"];
/**
 * The graphql function is used to parse GraphQL queries into a document that can be used by GraphQL clients.
 */
export function graphql(source: "\n  subscription SubscribeToPricesForSymbol($symbol: String!) {\n    onPriceUpdated(symbol: $symbol) {\n      timestamp: bucket\n      close\n      high\n      low\n      open\n      symbol\n      volume\n    }\n  }\n"): (typeof documents)["\n  subscription SubscribeToPricesForSymbol($symbol: String!) {\n    onPriceUpdated(symbol: $symbol) {\n      timestamp: bucket\n      close\n      high\n      low\n      open\n      symbol\n      volume\n    }\n  }\n"];

export function graphql(source: string) {
  return (documents as any)[source] ?? {};
}

export type DocumentType<TDocumentNode extends DocumentNode<any, any>> = TDocumentNode extends DocumentNode<  infer TType,  any>  ? TType  : never;
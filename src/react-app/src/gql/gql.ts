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
    "\nquery GetFocusedPriceLine {\n  ETH:prices(last: 500,interval: ONE_HOUR,where: {symbol:{eq:\"ETHUSDT\"}}) {\n    edges {\n      node {\n        ...PriceItem\n      }\n    }\n  }\n  BTC:prices(last: 500,interval: ONE_HOUR,where: {symbol:{eq:\"BTCUSDT\"}}) {\n    edges {\n      node {\n        ...PriceItem\n      }\n    }\n  }\n}\n": types.GetFocusedPriceLineDocument,
    "\nquery GetFocusedPrices {\n  ETH:prices(last: 1,interval: ONE_MIN,where: {symbol:{eq:\"ETHUSDT\"}}) {\n    edges {\n      node {\n        ...PriceItem\n      }\n    }\n  }\n  BTC:prices(last: 1,interval: ONE_MIN,where: {symbol:{eq:\"BTCUSDT\"}}) {\n    edges {\n      node {\n        ...PriceItem\n      }\n    }\n  }\n}\n": types.GetFocusedPricesDocument,
    "\n    mutation CreatePriceRule($newRule: PriceRuleInput!) {\n        createPriceRule(input:$newRule) {\n            id\n            name\n            description\n        }\n    }\n": types.CreatePriceRuleDocument,
    "\n    query GetInstruments($first: Int!) {\n        instruments(first:$first) {\n            totalCount\n            edges {\n                node {\n                symbol\n                id\n                }\n            }\n        }\n    }\n": types.GetInstrumentsDocument,
    "\n    mutation editPriceRule($existingRule: PriceRuleInput!) {\n        updatePriceRule(input:$existingRule) {\n            id\n            name\n            description\n        }\n    }\n": types.EditPriceRuleDocument,
    "\n  query GetPricesForSymbol($symbol: String!,$last: Int!, $interval: PriceInterval!) {\n    prices(\n      last: $last\n      where: { symbol: { eq: $symbol } }\n      order: { bucket: ASC }\n      interval: $interval\n    ) {\n      nodes {\n        timestamp: bucket\n        close\n        high\n        low\n        open\n        symbol\n        volume\n      }\n    }\n  }\n": types.GetPricesForSymbolDocument,
    "\n    mutation ToggleRuleStatus($id: UUID!) {\n        togglePriceRule(id:$id) {\n            id\n            isEnabled\n        }\n    }\n": types.ToggleRuleStatusDocument,
    "\nquery GetPriceRules($first: Int) {\n  priceRules(first: $first) {\n    edges {\n      node {\n        description\n        isEnabled\n        id\n        name\n        instrument {\n          symbol\n        }\n        createdAt\n      }\n    }\n    totalCount\n    pageInfo {\n      hasPreviousPage\n      hasNextPage\n    }\n  }\n}\n": types.GetPriceRulesDocument,
    "\nmutation DeletePriceRule($id: UUID!) {\n  deletePriceRule(id: $id) {\n    id\n  }\n}\n": types.DeletePriceRuleDocument,
    "\nquery UserSettings {\n  user {\n    email\n    notificationChannels(first: 10) {\n      nodes {\n        id\n        channelType\n        telegramUsername\n      }\n    }\n  }\n}\n": types.UserSettingsDocument,
    "\nmutation DisconnectTelegram {\n  deleteTelegramConnection\n}\n": types.DisconnectTelegramDocument,
    "\n  fragment ActivationLog on PriceRuleTriggerLog {\n    id\n    triggeredAt\n    price\n  }\n": types.ActivationLogFragmentDoc,
    "\nquery GetPriceRule($id: UUID!) {\n  priceRule(id: $id) {\n    description\n    id\n    isEnabled\n    name\n    instrument {\n      id\n      symbol\n    }\n    createdAt\n    activationLogs (first: 10) {\n        nodes {\n            ...ActivationLog\n        }\n    }\n    conditions(first: 10) {\n      totalCount\n      edges {\n        cursor\n        node {\n          value\n          additionalValues\n          conditionType\n        }\n      }\n    }\n  }\n}\n": types.GetPriceRuleDocument,
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
export function graphql(source: "\nquery GetFocusedPriceLine {\n  ETH:prices(last: 500,interval: ONE_HOUR,where: {symbol:{eq:\"ETHUSDT\"}}) {\n    edges {\n      node {\n        ...PriceItem\n      }\n    }\n  }\n  BTC:prices(last: 500,interval: ONE_HOUR,where: {symbol:{eq:\"BTCUSDT\"}}) {\n    edges {\n      node {\n        ...PriceItem\n      }\n    }\n  }\n}\n"): (typeof documents)["\nquery GetFocusedPriceLine {\n  ETH:prices(last: 500,interval: ONE_HOUR,where: {symbol:{eq:\"ETHUSDT\"}}) {\n    edges {\n      node {\n        ...PriceItem\n      }\n    }\n  }\n  BTC:prices(last: 500,interval: ONE_HOUR,where: {symbol:{eq:\"BTCUSDT\"}}) {\n    edges {\n      node {\n        ...PriceItem\n      }\n    }\n  }\n}\n"];
/**
 * The graphql function is used to parse GraphQL queries into a document that can be used by GraphQL clients.
 */
export function graphql(source: "\nquery GetFocusedPrices {\n  ETH:prices(last: 1,interval: ONE_MIN,where: {symbol:{eq:\"ETHUSDT\"}}) {\n    edges {\n      node {\n        ...PriceItem\n      }\n    }\n  }\n  BTC:prices(last: 1,interval: ONE_MIN,where: {symbol:{eq:\"BTCUSDT\"}}) {\n    edges {\n      node {\n        ...PriceItem\n      }\n    }\n  }\n}\n"): (typeof documents)["\nquery GetFocusedPrices {\n  ETH:prices(last: 1,interval: ONE_MIN,where: {symbol:{eq:\"ETHUSDT\"}}) {\n    edges {\n      node {\n        ...PriceItem\n      }\n    }\n  }\n  BTC:prices(last: 1,interval: ONE_MIN,where: {symbol:{eq:\"BTCUSDT\"}}) {\n    edges {\n      node {\n        ...PriceItem\n      }\n    }\n  }\n}\n"];
/**
 * The graphql function is used to parse GraphQL queries into a document that can be used by GraphQL clients.
 */
export function graphql(source: "\n    mutation CreatePriceRule($newRule: PriceRuleInput!) {\n        createPriceRule(input:$newRule) {\n            id\n            name\n            description\n        }\n    }\n"): (typeof documents)["\n    mutation CreatePriceRule($newRule: PriceRuleInput!) {\n        createPriceRule(input:$newRule) {\n            id\n            name\n            description\n        }\n    }\n"];
/**
 * The graphql function is used to parse GraphQL queries into a document that can be used by GraphQL clients.
 */
export function graphql(source: "\n    query GetInstruments($first: Int!) {\n        instruments(first:$first) {\n            totalCount\n            edges {\n                node {\n                symbol\n                id\n                }\n            }\n        }\n    }\n"): (typeof documents)["\n    query GetInstruments($first: Int!) {\n        instruments(first:$first) {\n            totalCount\n            edges {\n                node {\n                symbol\n                id\n                }\n            }\n        }\n    }\n"];
/**
 * The graphql function is used to parse GraphQL queries into a document that can be used by GraphQL clients.
 */
export function graphql(source: "\n    mutation editPriceRule($existingRule: PriceRuleInput!) {\n        updatePriceRule(input:$existingRule) {\n            id\n            name\n            description\n        }\n    }\n"): (typeof documents)["\n    mutation editPriceRule($existingRule: PriceRuleInput!) {\n        updatePriceRule(input:$existingRule) {\n            id\n            name\n            description\n        }\n    }\n"];
/**
 * The graphql function is used to parse GraphQL queries into a document that can be used by GraphQL clients.
 */
export function graphql(source: "\n  query GetPricesForSymbol($symbol: String!,$last: Int!, $interval: PriceInterval!) {\n    prices(\n      last: $last\n      where: { symbol: { eq: $symbol } }\n      order: { bucket: ASC }\n      interval: $interval\n    ) {\n      nodes {\n        timestamp: bucket\n        close\n        high\n        low\n        open\n        symbol\n        volume\n      }\n    }\n  }\n"): (typeof documents)["\n  query GetPricesForSymbol($symbol: String!,$last: Int!, $interval: PriceInterval!) {\n    prices(\n      last: $last\n      where: { symbol: { eq: $symbol } }\n      order: { bucket: ASC }\n      interval: $interval\n    ) {\n      nodes {\n        timestamp: bucket\n        close\n        high\n        low\n        open\n        symbol\n        volume\n      }\n    }\n  }\n"];
/**
 * The graphql function is used to parse GraphQL queries into a document that can be used by GraphQL clients.
 */
export function graphql(source: "\n    mutation ToggleRuleStatus($id: UUID!) {\n        togglePriceRule(id:$id) {\n            id\n            isEnabled\n        }\n    }\n"): (typeof documents)["\n    mutation ToggleRuleStatus($id: UUID!) {\n        togglePriceRule(id:$id) {\n            id\n            isEnabled\n        }\n    }\n"];
/**
 * The graphql function is used to parse GraphQL queries into a document that can be used by GraphQL clients.
 */
export function graphql(source: "\nquery GetPriceRules($first: Int) {\n  priceRules(first: $first) {\n    edges {\n      node {\n        description\n        isEnabled\n        id\n        name\n        instrument {\n          symbol\n        }\n        createdAt\n      }\n    }\n    totalCount\n    pageInfo {\n      hasPreviousPage\n      hasNextPage\n    }\n  }\n}\n"): (typeof documents)["\nquery GetPriceRules($first: Int) {\n  priceRules(first: $first) {\n    edges {\n      node {\n        description\n        isEnabled\n        id\n        name\n        instrument {\n          symbol\n        }\n        createdAt\n      }\n    }\n    totalCount\n    pageInfo {\n      hasPreviousPage\n      hasNextPage\n    }\n  }\n}\n"];
/**
 * The graphql function is used to parse GraphQL queries into a document that can be used by GraphQL clients.
 */
export function graphql(source: "\nmutation DeletePriceRule($id: UUID!) {\n  deletePriceRule(id: $id) {\n    id\n  }\n}\n"): (typeof documents)["\nmutation DeletePriceRule($id: UUID!) {\n  deletePriceRule(id: $id) {\n    id\n  }\n}\n"];
/**
 * The graphql function is used to parse GraphQL queries into a document that can be used by GraphQL clients.
 */
export function graphql(source: "\nquery UserSettings {\n  user {\n    email\n    notificationChannels(first: 10) {\n      nodes {\n        id\n        channelType\n        telegramUsername\n      }\n    }\n  }\n}\n"): (typeof documents)["\nquery UserSettings {\n  user {\n    email\n    notificationChannels(first: 10) {\n      nodes {\n        id\n        channelType\n        telegramUsername\n      }\n    }\n  }\n}\n"];
/**
 * The graphql function is used to parse GraphQL queries into a document that can be used by GraphQL clients.
 */
export function graphql(source: "\nmutation DisconnectTelegram {\n  deleteTelegramConnection\n}\n"): (typeof documents)["\nmutation DisconnectTelegram {\n  deleteTelegramConnection\n}\n"];
/**
 * The graphql function is used to parse GraphQL queries into a document that can be used by GraphQL clients.
 */
export function graphql(source: "\n  fragment ActivationLog on PriceRuleTriggerLog {\n    id\n    triggeredAt\n    price\n  }\n"): (typeof documents)["\n  fragment ActivationLog on PriceRuleTriggerLog {\n    id\n    triggeredAt\n    price\n  }\n"];
/**
 * The graphql function is used to parse GraphQL queries into a document that can be used by GraphQL clients.
 */
export function graphql(source: "\nquery GetPriceRule($id: UUID!) {\n  priceRule(id: $id) {\n    description\n    id\n    isEnabled\n    name\n    instrument {\n      id\n      symbol\n    }\n    createdAt\n    activationLogs (first: 10) {\n        nodes {\n            ...ActivationLog\n        }\n    }\n    conditions(first: 10) {\n      totalCount\n      edges {\n        cursor\n        node {\n          value\n          additionalValues\n          conditionType\n        }\n      }\n    }\n  }\n}\n"): (typeof documents)["\nquery GetPriceRule($id: UUID!) {\n  priceRule(id: $id) {\n    description\n    id\n    isEnabled\n    name\n    instrument {\n      id\n      symbol\n    }\n    createdAt\n    activationLogs (first: 10) {\n        nodes {\n            ...ActivationLog\n        }\n    }\n    conditions(first: 10) {\n      totalCount\n      edges {\n        cursor\n        node {\n          value\n          additionalValues\n          conditionType\n        }\n      }\n    }\n  }\n}\n"];
/**
 * The graphql function is used to parse GraphQL queries into a document that can be used by GraphQL clients.
 */
export function graphql(source: "\n  subscription SubscribeToPricesForSymbol($symbol: String!) {\n    onPriceUpdated(symbol: $symbol) {\n      timestamp: bucket\n      close\n      high\n      low\n      open\n      symbol\n      volume\n    }\n  }\n"): (typeof documents)["\n  subscription SubscribeToPricesForSymbol($symbol: String!) {\n    onPriceUpdated(symbol: $symbol) {\n      timestamp: bucket\n      close\n      high\n      low\n      open\n      symbol\n      volume\n    }\n  }\n"];

export function graphql(source: string) {
  return (documents as any)[source] ?? {};
}

export type DocumentType<TDocumentNode extends DocumentNode<any, any>> = TDocumentNode extends DocumentNode<  infer TType,  any>  ? TType  : never;
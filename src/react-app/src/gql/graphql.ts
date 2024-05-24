/* eslint-disable */
import { TypedDocumentNode as DocumentNode } from '@graphql-typed-document-node/core';
export type Maybe<T> = T | null;
export type InputMaybe<T> = Maybe<T>;
export type Exact<T extends { [key: string]: unknown }> = { [K in keyof T]: T[K] };
export type MakeOptional<T, K extends keyof T> = Omit<T, K> & { [SubKey in K]?: Maybe<T[SubKey]> };
export type MakeMaybe<T, K extends keyof T> = Omit<T, K> & { [SubKey in K]: Maybe<T[SubKey]> };
export type MakeEmpty<T extends { [key: string]: unknown }, K extends keyof T> = { [_ in K]?: never };
export type Incremental<T> = T | { [P in keyof T]?: P extends ' $fragmentName' | '__typename' ? T[P] : never };
/** All built-in and custom scalars, mapped to their actual values */
export type Scalars = {
  ID: { input: string; output: string; }
  String: { input: string; output: string; }
  Boolean: { input: boolean; output: boolean; }
  Int: { input: number; output: number; }
  Float: { input: number; output: number; }
  /** The `DateTime` scalar represents an ISO-8601 compliant date time type. */
  DateTime: { input: any; output: any; }
  /** The built-in `Decimal` scalar type. */
  Decimal: { input: any; output: any; }
};

export enum ApplyPolicy {
  AfterResolver = 'AFTER_RESOLVER',
  BeforeResolver = 'BEFORE_RESOLVER',
  Validation = 'VALIDATION'
}

/** Information about the offset pagination. */
export type CollectionSegmentInfo = {
  __typename?: 'CollectionSegmentInfo';
  /** Indicates whether more items exist following the set defined by the clients arguments. */
  hasNextPage: Scalars['Boolean']['output'];
  /** Indicates whether more items exist prior the set defined by the clients arguments. */
  hasPreviousPage: Scalars['Boolean']['output'];
};

export type DateTimeOperationFilterInput = {
  eq?: InputMaybe<Scalars['DateTime']['input']>;
  gt?: InputMaybe<Scalars['DateTime']['input']>;
  gte?: InputMaybe<Scalars['DateTime']['input']>;
  in?: InputMaybe<Array<InputMaybe<Scalars['DateTime']['input']>>>;
  lt?: InputMaybe<Scalars['DateTime']['input']>;
  lte?: InputMaybe<Scalars['DateTime']['input']>;
  neq?: InputMaybe<Scalars['DateTime']['input']>;
  ngt?: InputMaybe<Scalars['DateTime']['input']>;
  ngte?: InputMaybe<Scalars['DateTime']['input']>;
  nin?: InputMaybe<Array<InputMaybe<Scalars['DateTime']['input']>>>;
  nlt?: InputMaybe<Scalars['DateTime']['input']>;
  nlte?: InputMaybe<Scalars['DateTime']['input']>;
};

export type DecimalOperationFilterInput = {
  eq?: InputMaybe<Scalars['Decimal']['input']>;
  gt?: InputMaybe<Scalars['Decimal']['input']>;
  gte?: InputMaybe<Scalars['Decimal']['input']>;
  in?: InputMaybe<Array<InputMaybe<Scalars['Decimal']['input']>>>;
  lt?: InputMaybe<Scalars['Decimal']['input']>;
  lte?: InputMaybe<Scalars['Decimal']['input']>;
  neq?: InputMaybe<Scalars['Decimal']['input']>;
  ngt?: InputMaybe<Scalars['Decimal']['input']>;
  ngte?: InputMaybe<Scalars['Decimal']['input']>;
  nin?: InputMaybe<Array<InputMaybe<Scalars['Decimal']['input']>>>;
  nlt?: InputMaybe<Scalars['Decimal']['input']>;
  nlte?: InputMaybe<Scalars['Decimal']['input']>;
};

export type Exchange = {
  __typename?: 'Exchange';
  description?: Maybe<Scalars['String']['output']>;
  name: Scalars['String']['output'];
};

export type ExchangeFilterInput = {
  and?: InputMaybe<Array<ExchangeFilterInput>>;
  description?: InputMaybe<StringOperationFilterInput>;
  name?: InputMaybe<StringOperationFilterInput>;
  or?: InputMaybe<Array<ExchangeFilterInput>>;
};

export type ExchangeSortInput = {
  description?: InputMaybe<SortEnumType>;
  name?: InputMaybe<SortEnumType>;
};

/** A segment of a collection. */
export type ExchangesCollectionSegment = {
  __typename?: 'ExchangesCollectionSegment';
  /** A flattened list of the items. */
  items?: Maybe<Array<Exchange>>;
  /** Information to aid in pagination. */
  pageInfo: CollectionSegmentInfo;
  totalCount: Scalars['Int']['output'];
};

export type InstrumentPrice = {
  __typename?: 'InstrumentPrice';
  exchange: Exchange;
  price: Scalars['Decimal']['output'];
  quantity?: Maybe<Scalars['Decimal']['output']>;
  symbol: Scalars['String']['output'];
  timestamp: Scalars['DateTime']['output'];
  volume?: Maybe<Scalars['Decimal']['output']>;
};

export type InstrumentPriceFilterInput = {
  and?: InputMaybe<Array<InstrumentPriceFilterInput>>;
  exchange?: InputMaybe<ExchangeFilterInput>;
  or?: InputMaybe<Array<InstrumentPriceFilterInput>>;
  price?: InputMaybe<DecimalOperationFilterInput>;
  quantity?: InputMaybe<DecimalOperationFilterInput>;
  symbol?: InputMaybe<StringOperationFilterInput>;
  timestamp?: InputMaybe<DateTimeOperationFilterInput>;
  volume?: InputMaybe<DecimalOperationFilterInput>;
};

export type InstrumentPriceSortInput = {
  exchange?: InputMaybe<ExchangeSortInput>;
  price?: InputMaybe<SortEnumType>;
  quantity?: InputMaybe<SortEnumType>;
  symbol?: InputMaybe<SortEnumType>;
  timestamp?: InputMaybe<SortEnumType>;
  volume?: InputMaybe<SortEnumType>;
};

/** A connection to a list of items. */
export type InstrumentPricesConnection = {
  __typename?: 'InstrumentPricesConnection';
  /** A list of edges. */
  edges?: Maybe<Array<InstrumentPricesEdge>>;
  /** A flattened list of the nodes. */
  nodes?: Maybe<Array<InstrumentPrice>>;
  /** Information to aid in pagination. */
  pageInfo: PageInfo;
  /** Identifies the total count of items in the connection. */
  totalCount: Scalars['Int']['output'];
};

/** An edge in a connection. */
export type InstrumentPricesEdge = {
  __typename?: 'InstrumentPricesEdge';
  /** A cursor for use in pagination. */
  cursor: Scalars['String']['output'];
  /** The item at the end of the edge. */
  node: InstrumentPrice;
};

/** Information about pagination in a connection. */
export type PageInfo = {
  __typename?: 'PageInfo';
  /** When paginating forwards, the cursor to continue. */
  endCursor?: Maybe<Scalars['String']['output']>;
  /** Indicates whether more edges exist following the set defined by the clients arguments. */
  hasNextPage: Scalars['Boolean']['output'];
  /** Indicates whether more edges exist prior the set defined by the clients arguments. */
  hasPreviousPage: Scalars['Boolean']['output'];
  /** When paginating backwards, the cursor to continue. */
  startCursor?: Maybe<Scalars['String']['output']>;
};

export type Price = {
  __typename?: 'Price';
  bucket: Scalars['DateTime']['output'];
  close: Scalars['Decimal']['output'];
  exchange: Exchange;
  high: Scalars['Decimal']['output'];
  low: Scalars['Decimal']['output'];
  open: Scalars['Decimal']['output'];
  symbol: Scalars['String']['output'];
  volume?: Maybe<Scalars['Decimal']['output']>;
};

export type PriceFilterInput = {
  and?: InputMaybe<Array<PriceFilterInput>>;
  bucket?: InputMaybe<DateTimeOperationFilterInput>;
  close?: InputMaybe<DecimalOperationFilterInput>;
  exchange?: InputMaybe<ExchangeFilterInput>;
  high?: InputMaybe<DecimalOperationFilterInput>;
  low?: InputMaybe<DecimalOperationFilterInput>;
  open?: InputMaybe<DecimalOperationFilterInput>;
  or?: InputMaybe<Array<PriceFilterInput>>;
  symbol?: InputMaybe<StringOperationFilterInput>;
  volume?: InputMaybe<DecimalOperationFilterInput>;
};

export enum PriceInterval {
  FiveMin = 'FIVE_MIN',
  OneMin = 'ONE_MIN'
}

export type PriceSortInput = {
  bucket?: InputMaybe<SortEnumType>;
  close?: InputMaybe<SortEnumType>;
  exchange?: InputMaybe<ExchangeSortInput>;
  high?: InputMaybe<SortEnumType>;
  low?: InputMaybe<SortEnumType>;
  open?: InputMaybe<SortEnumType>;
  symbol?: InputMaybe<SortEnumType>;
  volume?: InputMaybe<SortEnumType>;
};

/** A connection to a list of items. */
export type PricesConnection = {
  __typename?: 'PricesConnection';
  /** A list of edges. */
  edges?: Maybe<Array<PricesEdge>>;
  /** A flattened list of the nodes. */
  nodes?: Maybe<Array<Price>>;
  /** Information to aid in pagination. */
  pageInfo: PageInfo;
  /** Identifies the total count of items in the connection. */
  totalCount: Scalars['Int']['output'];
};

/** An edge in a connection. */
export type PricesEdge = {
  __typename?: 'PricesEdge';
  /** A cursor for use in pagination. */
  cursor: Scalars['String']['output'];
  /** The item at the end of the edge. */
  node: Price;
};

export type Query = {
  __typename?: 'Query';
  exchanges?: Maybe<ExchangesCollectionSegment>;
  instrumentPrices?: Maybe<InstrumentPricesConnection>;
  prices?: Maybe<PricesConnection>;
};


export type QueryExchangesArgs = {
  order?: InputMaybe<Array<ExchangeSortInput>>;
  skip?: InputMaybe<Scalars['Int']['input']>;
  take?: InputMaybe<Scalars['Int']['input']>;
  where?: InputMaybe<ExchangeFilterInput>;
};


export type QueryInstrumentPricesArgs = {
  after?: InputMaybe<Scalars['String']['input']>;
  before?: InputMaybe<Scalars['String']['input']>;
  first?: InputMaybe<Scalars['Int']['input']>;
  last?: InputMaybe<Scalars['Int']['input']>;
  order?: InputMaybe<Array<InstrumentPriceSortInput>>;
  where?: InputMaybe<InstrumentPriceFilterInput>;
};


export type QueryPricesArgs = {
  after?: InputMaybe<Scalars['String']['input']>;
  before?: InputMaybe<Scalars['String']['input']>;
  first?: InputMaybe<Scalars['Int']['input']>;
  interval: PriceInterval;
  last?: InputMaybe<Scalars['Int']['input']>;
  order?: InputMaybe<Array<PriceSortInput>>;
  where?: InputMaybe<PriceFilterInput>;
};

export enum SortEnumType {
  Asc = 'ASC',
  Desc = 'DESC'
}

export type StringOperationFilterInput = {
  and?: InputMaybe<Array<StringOperationFilterInput>>;
  contains?: InputMaybe<Scalars['String']['input']>;
  endsWith?: InputMaybe<Scalars['String']['input']>;
  eq?: InputMaybe<Scalars['String']['input']>;
  in?: InputMaybe<Array<InputMaybe<Scalars['String']['input']>>>;
  ncontains?: InputMaybe<Scalars['String']['input']>;
  nendsWith?: InputMaybe<Scalars['String']['input']>;
  neq?: InputMaybe<Scalars['String']['input']>;
  nin?: InputMaybe<Array<InputMaybe<Scalars['String']['input']>>>;
  nstartsWith?: InputMaybe<Scalars['String']['input']>;
  or?: InputMaybe<Array<StringOperationFilterInput>>;
  startsWith?: InputMaybe<Scalars['String']['input']>;
};

export type Subscription = {
  __typename?: 'Subscription';
  onPriceUpdated?: Maybe<Price>;
};


export type SubscriptionOnPriceUpdatedArgs = {
  symbol: Scalars['String']['input'];
};

export type GetExchangesQueryVariables = Exact<{
  take: Scalars['Int']['input'];
}>;


export type GetExchangesQuery = { __typename?: 'Query', exchanges?: { __typename?: 'ExchangesCollectionSegment', totalCount: number, items?: Array<{ __typename?: 'Exchange', name: string, description?: string | null }> | null } | null };

export type PriceItemFragment = { __typename?: 'Price', bucket: any, close: any, symbol: string, volume?: any | null } & { ' $fragmentName'?: 'PriceItemFragment' };

export type GetFocusedPricesQueryVariables = Exact<{ [key: string]: never; }>;


export type GetFocusedPricesQuery = { __typename?: 'Query', ETH?: { __typename?: 'PricesConnection', edges?: Array<{ __typename?: 'PricesEdge', node: (
        { __typename?: 'Price' }
        & { ' $fragmentRefs'?: { 'PriceItemFragment': PriceItemFragment } }
      ) }> | null } | null, BTC?: { __typename?: 'PricesConnection', edges?: Array<{ __typename?: 'PricesEdge', node: (
        { __typename?: 'Price' }
        & { ' $fragmentRefs'?: { 'PriceItemFragment': PriceItemFragment } }
      ) }> | null } | null };

export type GetPricesForSymbolQueryVariables = Exact<{
  symbol: Scalars['String']['input'];
}>;


export type GetPricesForSymbolQuery = { __typename?: 'Query', prices?: { __typename?: 'PricesConnection', nodes?: Array<{ __typename?: 'Price', close: any, high: any, low: any, open: any, symbol: string, volume?: any | null, timestamp: any }> | null } | null };

export type SubscribeToPricesForSymbolSubscriptionVariables = Exact<{
  symbol: Scalars['String']['input'];
}>;


export type SubscribeToPricesForSymbolSubscription = { __typename?: 'Subscription', onPriceUpdated?: { __typename?: 'Price', close: any, high: any, low: any, open: any, symbol: string, volume?: any | null, timestamp: any } | null };

export const PriceItemFragmentDoc = {"kind":"Document","definitions":[{"kind":"FragmentDefinition","name":{"kind":"Name","value":"PriceItem"},"typeCondition":{"kind":"NamedType","name":{"kind":"Name","value":"Price"}},"selectionSet":{"kind":"SelectionSet","selections":[{"kind":"Field","name":{"kind":"Name","value":"bucket"}},{"kind":"Field","name":{"kind":"Name","value":"close"}},{"kind":"Field","name":{"kind":"Name","value":"symbol"}},{"kind":"Field","name":{"kind":"Name","value":"volume"}}]}}]} as unknown as DocumentNode<PriceItemFragment, unknown>;
export const GetExchangesDocument = {"kind":"Document","definitions":[{"kind":"OperationDefinition","operation":"query","name":{"kind":"Name","value":"GetExchanges"},"variableDefinitions":[{"kind":"VariableDefinition","variable":{"kind":"Variable","name":{"kind":"Name","value":"take"}},"type":{"kind":"NonNullType","type":{"kind":"NamedType","name":{"kind":"Name","value":"Int"}}}}],"selectionSet":{"kind":"SelectionSet","selections":[{"kind":"Field","name":{"kind":"Name","value":"exchanges"},"arguments":[{"kind":"Argument","name":{"kind":"Name","value":"take"},"value":{"kind":"Variable","name":{"kind":"Name","value":"take"}}}],"selectionSet":{"kind":"SelectionSet","selections":[{"kind":"Field","name":{"kind":"Name","value":"totalCount"}},{"kind":"Field","name":{"kind":"Name","value":"items"},"selectionSet":{"kind":"SelectionSet","selections":[{"kind":"Field","name":{"kind":"Name","value":"name"}},{"kind":"Field","name":{"kind":"Name","value":"description"}}]}}]}}]}}]} as unknown as DocumentNode<GetExchangesQuery, GetExchangesQueryVariables>;
export const GetFocusedPricesDocument = {"kind":"Document","definitions":[{"kind":"OperationDefinition","operation":"query","name":{"kind":"Name","value":"GetFocusedPrices"},"selectionSet":{"kind":"SelectionSet","selections":[{"kind":"Field","alias":{"kind":"Name","value":"ETH"},"name":{"kind":"Name","value":"prices"},"arguments":[{"kind":"Argument","name":{"kind":"Name","value":"last"},"value":{"kind":"IntValue","value":"1"}},{"kind":"Argument","name":{"kind":"Name","value":"interval"},"value":{"kind":"EnumValue","value":"ONE_MIN"}},{"kind":"Argument","name":{"kind":"Name","value":"where"},"value":{"kind":"ObjectValue","fields":[{"kind":"ObjectField","name":{"kind":"Name","value":"symbol"},"value":{"kind":"ObjectValue","fields":[{"kind":"ObjectField","name":{"kind":"Name","value":"eq"},"value":{"kind":"StringValue","value":"ETHUSDT","block":false}}]}}]}}],"selectionSet":{"kind":"SelectionSet","selections":[{"kind":"Field","name":{"kind":"Name","value":"edges"},"selectionSet":{"kind":"SelectionSet","selections":[{"kind":"Field","name":{"kind":"Name","value":"node"},"selectionSet":{"kind":"SelectionSet","selections":[{"kind":"FragmentSpread","name":{"kind":"Name","value":"PriceItem"}}]}}]}}]}},{"kind":"Field","alias":{"kind":"Name","value":"BTC"},"name":{"kind":"Name","value":"prices"},"arguments":[{"kind":"Argument","name":{"kind":"Name","value":"last"},"value":{"kind":"IntValue","value":"1"}},{"kind":"Argument","name":{"kind":"Name","value":"interval"},"value":{"kind":"EnumValue","value":"ONE_MIN"}},{"kind":"Argument","name":{"kind":"Name","value":"where"},"value":{"kind":"ObjectValue","fields":[{"kind":"ObjectField","name":{"kind":"Name","value":"symbol"},"value":{"kind":"ObjectValue","fields":[{"kind":"ObjectField","name":{"kind":"Name","value":"eq"},"value":{"kind":"StringValue","value":"BTCUSDT","block":false}}]}}]}}],"selectionSet":{"kind":"SelectionSet","selections":[{"kind":"Field","name":{"kind":"Name","value":"edges"},"selectionSet":{"kind":"SelectionSet","selections":[{"kind":"Field","name":{"kind":"Name","value":"node"},"selectionSet":{"kind":"SelectionSet","selections":[{"kind":"FragmentSpread","name":{"kind":"Name","value":"PriceItem"}}]}}]}}]}}]}},{"kind":"FragmentDefinition","name":{"kind":"Name","value":"PriceItem"},"typeCondition":{"kind":"NamedType","name":{"kind":"Name","value":"Price"}},"selectionSet":{"kind":"SelectionSet","selections":[{"kind":"Field","name":{"kind":"Name","value":"bucket"}},{"kind":"Field","name":{"kind":"Name","value":"close"}},{"kind":"Field","name":{"kind":"Name","value":"symbol"}},{"kind":"Field","name":{"kind":"Name","value":"volume"}}]}}]} as unknown as DocumentNode<GetFocusedPricesQuery, GetFocusedPricesQueryVariables>;
export const GetPricesForSymbolDocument = {"kind":"Document","definitions":[{"kind":"OperationDefinition","operation":"query","name":{"kind":"Name","value":"GetPricesForSymbol"},"variableDefinitions":[{"kind":"VariableDefinition","variable":{"kind":"Variable","name":{"kind":"Name","value":"symbol"}},"type":{"kind":"NonNullType","type":{"kind":"NamedType","name":{"kind":"Name","value":"String"}}}}],"selectionSet":{"kind":"SelectionSet","selections":[{"kind":"Field","name":{"kind":"Name","value":"prices"},"arguments":[{"kind":"Argument","name":{"kind":"Name","value":"last"},"value":{"kind":"IntValue","value":"100"}},{"kind":"Argument","name":{"kind":"Name","value":"where"},"value":{"kind":"ObjectValue","fields":[{"kind":"ObjectField","name":{"kind":"Name","value":"symbol"},"value":{"kind":"ObjectValue","fields":[{"kind":"ObjectField","name":{"kind":"Name","value":"eq"},"value":{"kind":"Variable","name":{"kind":"Name","value":"symbol"}}}]}}]}},{"kind":"Argument","name":{"kind":"Name","value":"interval"},"value":{"kind":"EnumValue","value":"ONE_MIN"}}],"selectionSet":{"kind":"SelectionSet","selections":[{"kind":"Field","name":{"kind":"Name","value":"nodes"},"selectionSet":{"kind":"SelectionSet","selections":[{"kind":"Field","alias":{"kind":"Name","value":"timestamp"},"name":{"kind":"Name","value":"bucket"}},{"kind":"Field","name":{"kind":"Name","value":"close"}},{"kind":"Field","name":{"kind":"Name","value":"high"}},{"kind":"Field","name":{"kind":"Name","value":"low"}},{"kind":"Field","name":{"kind":"Name","value":"open"}},{"kind":"Field","name":{"kind":"Name","value":"symbol"}},{"kind":"Field","name":{"kind":"Name","value":"volume"}}]}}]}}]}}]} as unknown as DocumentNode<GetPricesForSymbolQuery, GetPricesForSymbolQueryVariables>;
export const SubscribeToPricesForSymbolDocument = {"kind":"Document","definitions":[{"kind":"OperationDefinition","operation":"subscription","name":{"kind":"Name","value":"SubscribeToPricesForSymbol"},"variableDefinitions":[{"kind":"VariableDefinition","variable":{"kind":"Variable","name":{"kind":"Name","value":"symbol"}},"type":{"kind":"NonNullType","type":{"kind":"NamedType","name":{"kind":"Name","value":"String"}}}}],"selectionSet":{"kind":"SelectionSet","selections":[{"kind":"Field","name":{"kind":"Name","value":"onPriceUpdated"},"arguments":[{"kind":"Argument","name":{"kind":"Name","value":"symbol"},"value":{"kind":"Variable","name":{"kind":"Name","value":"symbol"}}}],"selectionSet":{"kind":"SelectionSet","selections":[{"kind":"Field","alias":{"kind":"Name","value":"timestamp"},"name":{"kind":"Name","value":"bucket"}},{"kind":"Field","name":{"kind":"Name","value":"close"}},{"kind":"Field","name":{"kind":"Name","value":"high"}},{"kind":"Field","name":{"kind":"Name","value":"low"}},{"kind":"Field","name":{"kind":"Name","value":"open"}},{"kind":"Field","name":{"kind":"Name","value":"symbol"}},{"kind":"Field","name":{"kind":"Name","value":"volume"}}]}}]}}]} as unknown as DocumentNode<SubscribeToPricesForSymbolSubscription, SubscribeToPricesForSymbolSubscriptionVariables>;
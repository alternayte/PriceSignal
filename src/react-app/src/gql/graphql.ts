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
  JSON: { input: any; output: any; }
  /** The `Long` scalar type represents non-fractional signed whole 64-bit numeric values. Long can represent values between -(2^63) and 2^63 - 1. */
  Long: { input: any; output: any; }
  UUID: { input: any; output: any; }
};

/** A connection to a list of items. */
export type ActivationLogsConnection = {
  __typename?: 'ActivationLogsConnection';
  /** A list of edges. */
  edges?: Maybe<Array<ActivationLogsEdge>>;
  /** A flattened list of the nodes. */
  nodes?: Maybe<Array<PriceRuleTriggerLog>>;
  /** Information to aid in pagination. */
  pageInfo: PageInfo;
  /** Identifies the total count of items in the connection. */
  totalCount: Scalars['Int']['output'];
};

/** An edge in a connection. */
export type ActivationLogsEdge = {
  __typename?: 'ActivationLogsEdge';
  /** A cursor for use in pagination. */
  cursor: Scalars['String']['output'];
  /** The item at the end of the edge. */
  node: PriceRuleTriggerLog;
};

export type BaseEventFilterInput = {
  and?: InputMaybe<Array<BaseEventFilterInput>>;
  or?: InputMaybe<Array<BaseEventFilterInput>>;
};

export type BooleanOperationFilterInput = {
  eq?: InputMaybe<Scalars['Boolean']['input']>;
  neq?: InputMaybe<Scalars['Boolean']['input']>;
};

/** Information about the offset pagination. */
export type CollectionSegmentInfo = {
  __typename?: 'CollectionSegmentInfo';
  /** Indicates whether more items exist following the set defined by the clients arguments. */
  hasNextPage: Scalars['Boolean']['output'];
  /** Indicates whether more items exist prior the set defined by the clients arguments. */
  hasPreviousPage: Scalars['Boolean']['output'];
};

export enum ConditionType {
  Price = 'PRICE',
  PriceAction = 'PRICE_ACTION',
  PriceCrossover = 'PRICE_CROSSOVER',
  PricePercentage = 'PRICE_PERCENTAGE',
  TechnicalIndicator = 'TECHNICAL_INDICATOR'
}

/** A connection to a list of items. */
export type ConditionsConnection = {
  __typename?: 'ConditionsConnection';
  /** A list of edges. */
  edges?: Maybe<Array<ConditionsEdge>>;
  /** A flattened list of the nodes. */
  nodes?: Maybe<Array<PriceCondition>>;
  /** Information to aid in pagination. */
  pageInfo: PageInfo;
  /** Identifies the total count of items in the connection. */
  totalCount: Scalars['Int']['output'];
};

/** An edge in a connection. */
export type ConditionsEdge = {
  __typename?: 'ConditionsEdge';
  /** A cursor for use in pagination. */
  cursor: Scalars['String']['output'];
  /** The item at the end of the edge. */
  node: PriceCondition;
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
  id: Scalars['ID']['output'];
  name: Scalars['String']['output'];
};

export enum ExchangeCode {
  Binance = 'BINANCE',
  Coinbase = 'COINBASE',
  DexScreener = 'DEX_SCREENER',
  Kraken = 'KRAKEN'
}

export type ExchangeCodeOperationFilterInput = {
  eq?: InputMaybe<ExchangeCode>;
  in?: InputMaybe<Array<ExchangeCode>>;
  neq?: InputMaybe<ExchangeCode>;
  nin?: InputMaybe<Array<ExchangeCode>>;
};

export type ExchangeFilterInput = {
  and?: InputMaybe<Array<ExchangeFilterInput>>;
  description?: InputMaybe<StringOperationFilterInput>;
  id?: InputMaybe<IdOperationFilterInput>;
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

export type IdOperationFilterInput = {
  eq?: InputMaybe<Scalars['ID']['input']>;
  in?: InputMaybe<Array<InputMaybe<Scalars['ID']['input']>>>;
  neq?: InputMaybe<Scalars['ID']['input']>;
  nin?: InputMaybe<Array<InputMaybe<Scalars['ID']['input']>>>;
};

export type Images = {
  __typename?: 'Images';
  size: Scalars['String']['output'];
  url: Scalars['String']['output'];
};

export type Instrument = {
  __typename?: 'Instrument';
  baseAsset: Scalars['String']['output'];
  description?: Maybe<Scalars['String']['output']>;
  id: Scalars['ID']['output'];
  name: Scalars['String']['output'];
  quoteAsset: Scalars['String']['output'];
  symbol: Scalars['String']['output'];
};

export type InstrumentFilterInput = {
  and?: InputMaybe<Array<InstrumentFilterInput>>;
  baseAsset?: InputMaybe<StringOperationFilterInput>;
  exchange?: InputMaybe<ExchangeCodeOperationFilterInput>;
  id?: InputMaybe<IdOperationFilterInput>;
  name?: InputMaybe<StringOperationFilterInput>;
  or?: InputMaybe<Array<InstrumentFilterInput>>;
  quoteAsset?: InputMaybe<StringOperationFilterInput>;
  symbol?: InputMaybe<StringOperationFilterInput>;
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

export type InstrumentSortInput = {
  baseAsset?: InputMaybe<SortEnumType>;
  createdAt?: InputMaybe<SortEnumType>;
  deletedAt?: InputMaybe<SortEnumType>;
  description?: InputMaybe<SortEnumType>;
  entityId?: InputMaybe<SortEnumType>;
  exchange?: InputMaybe<ExchangeSortInput>;
  id?: InputMaybe<SortEnumType>;
  modifiedAt?: InputMaybe<SortEnumType>;
  name?: InputMaybe<SortEnumType>;
  quoteAsset?: InputMaybe<SortEnumType>;
  symbol?: InputMaybe<SortEnumType>;
};

/** A connection to a list of items. */
export type InstrumentsConnection = {
  __typename?: 'InstrumentsConnection';
  /** A list of edges. */
  edges?: Maybe<Array<InstrumentsEdge>>;
  /** A flattened list of the nodes. */
  nodes?: Maybe<Array<Instrument>>;
  /** Information to aid in pagination. */
  pageInfo: PageInfo;
  /** Identifies the total count of items in the connection. */
  totalCount: Scalars['Int']['output'];
};

/** An edge in a connection. */
export type InstrumentsEdge = {
  __typename?: 'InstrumentsEdge';
  /** A cursor for use in pagination. */
  cursor: Scalars['String']['output'];
  /** The item at the end of the edge. */
  node: Instrument;
};

export type JsonDocumentFilterInput = {
  and?: InputMaybe<Array<JsonDocumentFilterInput>>;
  or?: InputMaybe<Array<JsonDocumentFilterInput>>;
  rootElement?: InputMaybe<JsonElementFilterInput>;
};

export type JsonElementFilterInput = {
  and?: InputMaybe<Array<JsonElementFilterInput>>;
  or?: InputMaybe<Array<JsonElementFilterInput>>;
  valueKind?: InputMaybe<JsonValueKindOperationFilterInput>;
};

export enum JsonValueKind {
  Array = 'ARRAY',
  False = 'FALSE',
  Null = 'NULL',
  Number = 'NUMBER',
  Object = 'OBJECT',
  String = 'STRING',
  True = 'TRUE',
  Undefined = 'UNDEFINED'
}

export type JsonValueKindOperationFilterInput = {
  eq?: InputMaybe<JsonValueKind>;
  in?: InputMaybe<Array<JsonValueKind>>;
  neq?: InputMaybe<JsonValueKind>;
  nin?: InputMaybe<Array<JsonValueKind>>;
};

export type ListFilterInputTypeOfBaseEventFilterInput = {
  all?: InputMaybe<BaseEventFilterInput>;
  any?: InputMaybe<Scalars['Boolean']['input']>;
  none?: InputMaybe<BaseEventFilterInput>;
  some?: InputMaybe<BaseEventFilterInput>;
};

export type ListFilterInputTypeOfPriceConditionFilterInput = {
  all?: InputMaybe<PriceConditionFilterInput>;
  any?: InputMaybe<Scalars['Boolean']['input']>;
  none?: InputMaybe<PriceConditionFilterInput>;
  some?: InputMaybe<PriceConditionFilterInput>;
};

export type ListFilterInputTypeOfPriceRuleFilterInput = {
  all?: InputMaybe<PriceRuleFilterInput>;
  any?: InputMaybe<Scalars['Boolean']['input']>;
  none?: InputMaybe<PriceRuleFilterInput>;
  some?: InputMaybe<PriceRuleFilterInput>;
};

export type ListFilterInputTypeOfPriceRuleTriggerLogFilterInput = {
  all?: InputMaybe<PriceRuleTriggerLogFilterInput>;
  any?: InputMaybe<Scalars['Boolean']['input']>;
  none?: InputMaybe<PriceRuleTriggerLogFilterInput>;
  some?: InputMaybe<PriceRuleTriggerLogFilterInput>;
};

export type ListFilterInputTypeOfSubscriptionFilterInput = {
  all?: InputMaybe<SubscriptionFilterInput>;
  any?: InputMaybe<Scalars['Boolean']['input']>;
  none?: InputMaybe<SubscriptionFilterInput>;
  some?: InputMaybe<SubscriptionFilterInput>;
};

export type ListFilterInputTypeOfUserNotificationChannelFilterInput = {
  all?: InputMaybe<UserNotificationChannelFilterInput>;
  any?: InputMaybe<Scalars['Boolean']['input']>;
  none?: InputMaybe<UserNotificationChannelFilterInput>;
  some?: InputMaybe<UserNotificationChannelFilterInput>;
};

export type LongOperationFilterInput = {
  eq?: InputMaybe<Scalars['Long']['input']>;
  gt?: InputMaybe<Scalars['Long']['input']>;
  gte?: InputMaybe<Scalars['Long']['input']>;
  in?: InputMaybe<Array<InputMaybe<Scalars['Long']['input']>>>;
  lt?: InputMaybe<Scalars['Long']['input']>;
  lte?: InputMaybe<Scalars['Long']['input']>;
  neq?: InputMaybe<Scalars['Long']['input']>;
  ngt?: InputMaybe<Scalars['Long']['input']>;
  ngte?: InputMaybe<Scalars['Long']['input']>;
  nin?: InputMaybe<Array<InputMaybe<Scalars['Long']['input']>>>;
  nlt?: InputMaybe<Scalars['Long']['input']>;
  nlte?: InputMaybe<Scalars['Long']['input']>;
};

export type Mutation = {
  __typename?: 'Mutation';
  createPriceRule: PriceRule;
  deletePriceRule: PriceRule;
  deleteTelegramConnection: Scalars['UUID']['output'];
  togglePriceRule: PriceRule;
  updatePriceRule: PriceRule;
};


export type MutationCreatePriceRuleArgs = {
  input: PriceRuleInput;
};


export type MutationDeletePriceRuleArgs = {
  id: Scalars['UUID']['input'];
};


export type MutationTogglePriceRuleArgs = {
  id: Scalars['UUID']['input'];
};


export type MutationUpdatePriceRuleArgs = {
  input: PriceRuleInput;
};

export type News = {
  __typename?: 'News';
  author: Scalars['String']['output'];
  content: Scalars['String']['output'];
  created_at: Scalars['String']['output'];
  headline: Scalars['String']['output'];
  id: Scalars['ID']['output'];
  images: Array<Images>;
  source: Scalars['String']['output'];
  summary: Scalars['String']['output'];
  symbols: Array<Scalars['String']['output']>;
  updated_at: Scalars['String']['output'];
  url: Scalars['String']['output'];
};

export enum NotificationChannelType {
  Email = 'EMAIL',
  None = 'NONE',
  PushNotification = 'PUSH_NOTIFICATION',
  Sms = 'SMS',
  Telegram = 'TELEGRAM',
  Webhook = 'WEBHOOK'
}

export type NotificationChannelTypeOperationFilterInput = {
  eq?: InputMaybe<NotificationChannelType>;
  in?: InputMaybe<Array<NotificationChannelType>>;
  neq?: InputMaybe<NotificationChannelType>;
  nin?: InputMaybe<Array<NotificationChannelType>>;
};

/** A connection to a list of items. */
export type NotificationChannelsConnection = {
  __typename?: 'NotificationChannelsConnection';
  /** A list of edges. */
  edges?: Maybe<Array<NotificationChannelsEdge>>;
  /** A flattened list of the nodes. */
  nodes?: Maybe<Array<UserNotificationChannel>>;
  /** Information to aid in pagination. */
  pageInfo: PageInfo;
  /** Identifies the total count of items in the connection. */
  totalCount: Scalars['Int']['output'];
};

/** An edge in a connection. */
export type NotificationChannelsEdge = {
  __typename?: 'NotificationChannelsEdge';
  /** A cursor for use in pagination. */
  cursor: Scalars['String']['output'];
  /** The item at the end of the edge. */
  node: UserNotificationChannel;
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

export type PriceCondition = {
  __typename?: 'PriceCondition';
  additionalValues?: Maybe<Scalars['String']['output']>;
  conditionType: ConditionType;
  value: Scalars['Decimal']['output'];
};

export type PriceConditionFilterInput = {
  additionalValues?: InputMaybe<JsonDocumentFilterInput>;
  and?: InputMaybe<Array<PriceConditionFilterInput>>;
  conditionType?: InputMaybe<StringOperationFilterInput>;
  createdAt?: InputMaybe<DateTimeOperationFilterInput>;
  deletedAt?: InputMaybe<DateTimeOperationFilterInput>;
  entityId?: InputMaybe<UuidOperationFilterInput>;
  events?: InputMaybe<ListFilterInputTypeOfBaseEventFilterInput>;
  id?: InputMaybe<LongOperationFilterInput>;
  modifiedAt?: InputMaybe<DateTimeOperationFilterInput>;
  or?: InputMaybe<Array<PriceConditionFilterInput>>;
  rule?: InputMaybe<PriceRuleFilterInput>;
  value?: InputMaybe<DecimalOperationFilterInput>;
};

export type PriceConditionInput = {
  additionalValues?: InputMaybe<Scalars['JSON']['input']>;
  conditionType: ConditionType;
  value: Scalars['Decimal']['input'];
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
  FifteenMin = 'FIFTEEN_MIN',
  FiveMin = 'FIVE_MIN',
  OneHour = 'ONE_HOUR',
  OneMin = 'ONE_MIN',
  TenMin = 'TEN_MIN'
}

export type PriceRule = {
  __typename?: 'PriceRule';
  activationLogs?: Maybe<ActivationLogsConnection>;
  conditions?: Maybe<ConditionsConnection>;
  createdAt: Scalars['DateTime']['output'];
  description?: Maybe<Scalars['String']['output']>;
  id: Scalars['ID']['output'];
  instrument: Instrument;
  isEnabled: Scalars['Boolean']['output'];
  lastTriggeredAt?: Maybe<Scalars['DateTime']['output']>;
  lastTriggeredPrice?: Maybe<Scalars['Decimal']['output']>;
  name: Scalars['String']['output'];
  notificationChannel: NotificationChannelType;
};


export type PriceRuleActivationLogsArgs = {
  after?: InputMaybe<Scalars['String']['input']>;
  before?: InputMaybe<Scalars['String']['input']>;
  first?: InputMaybe<Scalars['Int']['input']>;
  last?: InputMaybe<Scalars['Int']['input']>;
};


export type PriceRuleConditionsArgs = {
  after?: InputMaybe<Scalars['String']['input']>;
  before?: InputMaybe<Scalars['String']['input']>;
  first?: InputMaybe<Scalars['Int']['input']>;
  last?: InputMaybe<Scalars['Int']['input']>;
};

export type PriceRuleFilterInput = {
  activationLogs?: InputMaybe<ListFilterInputTypeOfPriceRuleTriggerLogFilterInput>;
  and?: InputMaybe<Array<PriceRuleFilterInput>>;
  conditions?: InputMaybe<ListFilterInputTypeOfPriceConditionFilterInput>;
  createdAt?: InputMaybe<DateTimeOperationFilterInput>;
  deletedAt?: InputMaybe<DateTimeOperationFilterInput>;
  description?: InputMaybe<StringOperationFilterInput>;
  entityId?: InputMaybe<UuidOperationFilterInput>;
  events?: InputMaybe<ListFilterInputTypeOfBaseEventFilterInput>;
  hasAttempted?: InputMaybe<BooleanOperationFilterInput>;
  id?: InputMaybe<LongOperationFilterInput>;
  instrument?: InputMaybe<InstrumentFilterInput>;
  instrumentId?: InputMaybe<LongOperationFilterInput>;
  isEnabled?: InputMaybe<BooleanOperationFilterInput>;
  lastTriggeredAt?: InputMaybe<DateTimeOperationFilterInput>;
  lastTriggeredPrice?: InputMaybe<DecimalOperationFilterInput>;
  modifiedAt?: InputMaybe<DateTimeOperationFilterInput>;
  name?: InputMaybe<StringOperationFilterInput>;
  notificationChannel?: InputMaybe<NotificationChannelTypeOperationFilterInput>;
  or?: InputMaybe<Array<PriceRuleFilterInput>>;
  user?: InputMaybe<UserFilterInput>;
  userId?: InputMaybe<StringOperationFilterInput>;
};

export type PriceRuleInput = {
  conditions?: InputMaybe<Array<InputMaybe<PriceConditionInput>>>;
  description: Scalars['String']['input'];
  id: Scalars['ID']['input'];
  instrumentId: Scalars['ID']['input'];
  isEnabled?: InputMaybe<Scalars['Boolean']['input']>;
  name: Scalars['String']['input'];
  notificationChannel?: InputMaybe<NotificationChannelType>;
};

export type PriceRuleTriggerLog = {
  __typename?: 'PriceRuleTriggerLog';
  id: Scalars['ID']['output'];
  price?: Maybe<Scalars['Decimal']['output']>;
  priceChange?: Maybe<Scalars['Decimal']['output']>;
  priceChangePercentage?: Maybe<Scalars['Decimal']['output']>;
  priceRuleSnapshot?: Maybe<Scalars['String']['output']>;
  triggeredAt: Scalars['DateTime']['output'];
};

export type PriceRuleTriggerLogFilterInput = {
  and?: InputMaybe<Array<PriceRuleTriggerLogFilterInput>>;
  createdAt?: InputMaybe<DateTimeOperationFilterInput>;
  deletedAt?: InputMaybe<DateTimeOperationFilterInput>;
  entityId?: InputMaybe<UuidOperationFilterInput>;
  events?: InputMaybe<ListFilterInputTypeOfBaseEventFilterInput>;
  id?: InputMaybe<LongOperationFilterInput>;
  modifiedAt?: InputMaybe<DateTimeOperationFilterInput>;
  or?: InputMaybe<Array<PriceRuleTriggerLogFilterInput>>;
  price?: InputMaybe<DecimalOperationFilterInput>;
  priceChange?: InputMaybe<DecimalOperationFilterInput>;
  priceChangePercentage?: InputMaybe<DecimalOperationFilterInput>;
  priceRule?: InputMaybe<PriceRuleFilterInput>;
  priceRuleSnapshot?: InputMaybe<JsonDocumentFilterInput>;
  triggeredAt?: InputMaybe<DateTimeOperationFilterInput>;
};

/** A connection to a list of items. */
export type PriceRulesConnection = {
  __typename?: 'PriceRulesConnection';
  /** A list of edges. */
  edges?: Maybe<Array<PriceRulesEdge>>;
  /** A flattened list of the nodes. */
  nodes?: Maybe<Array<PriceRule>>;
  /** Information to aid in pagination. */
  pageInfo: PageInfo;
  /** Identifies the total count of items in the connection. */
  totalCount: Scalars['Int']['output'];
};

/** An edge in a connection. */
export type PriceRulesEdge = {
  __typename?: 'PriceRulesEdge';
  /** A cursor for use in pagination. */
  cursor: Scalars['String']['output'];
  /** The item at the end of the edge. */
  node: PriceRule;
};

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
  instruments?: Maybe<InstrumentsConnection>;
  news: Array<News>;
  priceRule?: Maybe<PriceRule>;
  priceRules?: Maybe<PriceRulesConnection>;
  prices?: Maybe<PricesConnection>;
  user?: Maybe<User>;
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


export type QueryInstrumentsArgs = {
  after?: InputMaybe<Scalars['String']['input']>;
  before?: InputMaybe<Scalars['String']['input']>;
  first?: InputMaybe<Scalars['Int']['input']>;
  last?: InputMaybe<Scalars['Int']['input']>;
  where?: InputMaybe<InstrumentFilterInput>;
};


export type QueryPriceRuleArgs = {
  id: Scalars['UUID']['input'];
};


export type QueryPriceRulesArgs = {
  after?: InputMaybe<Scalars['String']['input']>;
  before?: InputMaybe<Scalars['String']['input']>;
  first?: InputMaybe<Scalars['Int']['input']>;
  last?: InputMaybe<Scalars['Int']['input']>;
  where?: InputMaybe<PriceRuleFilterInput>;
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

export type SubscriptionFilterInput = {
  and?: InputMaybe<Array<SubscriptionFilterInput>>;
  cancelAt?: InputMaybe<DateTimeOperationFilterInput>;
  cancelAtPeriodEnd?: InputMaybe<BooleanOperationFilterInput>;
  canceledAt?: InputMaybe<DateTimeOperationFilterInput>;
  createdAt?: InputMaybe<DateTimeOperationFilterInput>;
  currentPeriodEnd?: InputMaybe<DateTimeOperationFilterInput>;
  currentPeriodStart?: InputMaybe<DateTimeOperationFilterInput>;
  endedAt?: InputMaybe<DateTimeOperationFilterInput>;
  id?: InputMaybe<StringOperationFilterInput>;
  metadata?: InputMaybe<JsonDocumentFilterInput>;
  or?: InputMaybe<Array<SubscriptionFilterInput>>;
  priceId?: InputMaybe<StringOperationFilterInput>;
  quantity?: InputMaybe<LongOperationFilterInput>;
  status?: InputMaybe<StringOperationFilterInput>;
  trialEnd?: InputMaybe<DateTimeOperationFilterInput>;
  trialStart?: InputMaybe<DateTimeOperationFilterInput>;
  user?: InputMaybe<UserFilterInput>;
};

export type User = {
  __typename?: 'User';
  email: Scalars['String']['output'];
  id: Scalars['ID']['output'];
  notificationChannels?: Maybe<NotificationChannelsConnection>;
};


export type UserNotificationChannelsArgs = {
  after?: InputMaybe<Scalars['String']['input']>;
  before?: InputMaybe<Scalars['String']['input']>;
  first?: InputMaybe<Scalars['Int']['input']>;
  last?: InputMaybe<Scalars['Int']['input']>;
};

export type UserFilterInput = {
  and?: InputMaybe<Array<UserFilterInput>>;
  email?: InputMaybe<StringOperationFilterInput>;
  id?: InputMaybe<StringOperationFilterInput>;
  notificationChannels?: InputMaybe<ListFilterInputTypeOfUserNotificationChannelFilterInput>;
  or?: InputMaybe<Array<UserFilterInput>>;
  priceRules?: InputMaybe<ListFilterInputTypeOfPriceRuleFilterInput>;
  stripeCustomerId?: InputMaybe<StringOperationFilterInput>;
  subscriptions?: InputMaybe<ListFilterInputTypeOfSubscriptionFilterInput>;
};

export type UserNotificationChannel = {
  __typename?: 'UserNotificationChannel';
  channelType: NotificationChannelType;
  id: Scalars['ID']['output'];
  telegramChatId: Scalars['String']['output'];
  telegramUsername: Scalars['String']['output'];
};

export type UserNotificationChannelFilterInput = {
  and?: InputMaybe<Array<UserNotificationChannelFilterInput>>;
  channelType?: InputMaybe<NotificationChannelTypeOperationFilterInput>;
  createdAt?: InputMaybe<DateTimeOperationFilterInput>;
  deletedAt?: InputMaybe<DateTimeOperationFilterInput>;
  entityId?: InputMaybe<UuidOperationFilterInput>;
  events?: InputMaybe<ListFilterInputTypeOfBaseEventFilterInput>;
  id?: InputMaybe<LongOperationFilterInput>;
  modifiedAt?: InputMaybe<DateTimeOperationFilterInput>;
  or?: InputMaybe<Array<UserNotificationChannelFilterInput>>;
  telegramChatId?: InputMaybe<LongOperationFilterInput>;
  telegramUsername?: InputMaybe<StringOperationFilterInput>;
  user?: InputMaybe<UserFilterInput>;
};

export type UuidOperationFilterInput = {
  eq?: InputMaybe<Scalars['UUID']['input']>;
  gt?: InputMaybe<Scalars['UUID']['input']>;
  gte?: InputMaybe<Scalars['UUID']['input']>;
  in?: InputMaybe<Array<InputMaybe<Scalars['UUID']['input']>>>;
  lt?: InputMaybe<Scalars['UUID']['input']>;
  lte?: InputMaybe<Scalars['UUID']['input']>;
  neq?: InputMaybe<Scalars['UUID']['input']>;
  ngt?: InputMaybe<Scalars['UUID']['input']>;
  ngte?: InputMaybe<Scalars['UUID']['input']>;
  nin?: InputMaybe<Array<InputMaybe<Scalars['UUID']['input']>>>;
  nlt?: InputMaybe<Scalars['UUID']['input']>;
  nlte?: InputMaybe<Scalars['UUID']['input']>;
};

export type GetExchangesQueryVariables = Exact<{
  take: Scalars['Int']['input'];
}>;


export type GetExchangesQuery = { __typename?: 'Query', exchanges?: { __typename?: 'ExchangesCollectionSegment', totalCount: number, items?: Array<{ __typename?: 'Exchange', name: string, description?: string | null }> | null } | null };

export type MarketNewsQueryVariables = Exact<{ [key: string]: never; }>;


export type MarketNewsQuery = { __typename?: 'Query', news: Array<{ __typename?: 'News', id: string, headline: string, summary: string, url: string }> };

export type PriceItemFragment = { __typename?: 'Price', bucket: any, close: any, symbol: string, volume?: any | null } & { ' $fragmentName'?: 'PriceItemFragment' };

export type GetFocusedPriceLineQueryVariables = Exact<{ [key: string]: never; }>;


export type GetFocusedPriceLineQuery = { __typename?: 'Query', ETH?: { __typename?: 'PricesConnection', edges?: Array<{ __typename?: 'PricesEdge', node: (
        { __typename?: 'Price' }
        & { ' $fragmentRefs'?: { 'PriceItemFragment': PriceItemFragment } }
      ) }> | null } | null, BTC?: { __typename?: 'PricesConnection', edges?: Array<{ __typename?: 'PricesEdge', node: (
        { __typename?: 'Price' }
        & { ' $fragmentRefs'?: { 'PriceItemFragment': PriceItemFragment } }
      ) }> | null } | null };

export type GetFocusedPricesQueryVariables = Exact<{ [key: string]: never; }>;


export type GetFocusedPricesQuery = { __typename?: 'Query', ETH?: { __typename?: 'PricesConnection', edges?: Array<{ __typename?: 'PricesEdge', node: (
        { __typename?: 'Price' }
        & { ' $fragmentRefs'?: { 'PriceItemFragment': PriceItemFragment } }
      ) }> | null } | null, BTC?: { __typename?: 'PricesConnection', edges?: Array<{ __typename?: 'PricesEdge', node: (
        { __typename?: 'Price' }
        & { ' $fragmentRefs'?: { 'PriceItemFragment': PriceItemFragment } }
      ) }> | null } | null };

export type ActiveRulesQueryVariables = Exact<{
  first?: InputMaybe<Scalars['Int']['input']>;
}>;


export type ActiveRulesQuery = { __typename?: 'Query', priceRules?: { __typename?: 'PriceRulesConnection', nodes?: Array<{ __typename?: 'PriceRule', id: string, name: string, description?: string | null, lastTriggeredAt?: any | null, lastTriggeredPrice?: any | null, instrument: { __typename?: 'Instrument', id: string, baseAsset: string, quoteAsset: string, symbol: string } }> | null } | null };

export type CreatePriceRuleMutationVariables = Exact<{
  newRule: PriceRuleInput;
}>;


export type CreatePriceRuleMutation = { __typename?: 'Mutation', createPriceRule: { __typename?: 'PriceRule', id: string, name: string, description?: string | null } };

export type GetInstrumentsQueryVariables = Exact<{
  first: Scalars['Int']['input'];
}>;


export type GetInstrumentsQuery = { __typename?: 'Query', instruments?: { __typename?: 'InstrumentsConnection', totalCount: number, edges?: Array<{ __typename?: 'InstrumentsEdge', node: { __typename?: 'Instrument', symbol: string, id: string } }> | null } | null };

export type EditPriceRuleMutationVariables = Exact<{
  existingRule: PriceRuleInput;
}>;


export type EditPriceRuleMutation = { __typename?: 'Mutation', updatePriceRule: { __typename?: 'PriceRule', id: string, name: string, description?: string | null } };

export type GetPricesForSymbolQueryVariables = Exact<{
  symbol: Scalars['String']['input'];
  last: Scalars['Int']['input'];
  interval: PriceInterval;
}>;


export type GetPricesForSymbolQuery = { __typename?: 'Query', prices?: { __typename?: 'PricesConnection', nodes?: Array<{ __typename?: 'Price', close: any, high: any, low: any, open: any, symbol: string, volume?: any | null, timestamp: any }> | null } | null };

export type ToggleRuleStatusMutationVariables = Exact<{
  id: Scalars['UUID']['input'];
}>;


export type ToggleRuleStatusMutation = { __typename?: 'Mutation', togglePriceRule: { __typename?: 'PriceRule', id: string, isEnabled: boolean } };

export type GetPriceRulesQueryVariables = Exact<{
  first?: InputMaybe<Scalars['Int']['input']>;
}>;


export type GetPriceRulesQuery = { __typename?: 'Query', priceRules?: { __typename?: 'PriceRulesConnection', totalCount: number, edges?: Array<{ __typename?: 'PriceRulesEdge', node: { __typename?: 'PriceRule', description?: string | null, isEnabled: boolean, id: string, name: string, createdAt: any, instrument: { __typename?: 'Instrument', symbol: string } } }> | null, pageInfo: { __typename?: 'PageInfo', hasPreviousPage: boolean, hasNextPage: boolean } } | null };

export type DeletePriceRuleMutationVariables = Exact<{
  id: Scalars['UUID']['input'];
}>;


export type DeletePriceRuleMutation = { __typename?: 'Mutation', deletePriceRule: { __typename?: 'PriceRule', id: string } };

export type UserSettingsQueryVariables = Exact<{ [key: string]: never; }>;


export type UserSettingsQuery = { __typename?: 'Query', user?: { __typename?: 'User', email: string, notificationChannels?: { __typename?: 'NotificationChannelsConnection', nodes?: Array<{ __typename?: 'UserNotificationChannel', id: string, channelType: NotificationChannelType, telegramUsername: string }> | null } | null } | null };

export type DisconnectTelegramMutationVariables = Exact<{ [key: string]: never; }>;


export type DisconnectTelegramMutation = { __typename?: 'Mutation', deleteTelegramConnection: any };

export type ActivationLogFragment = { __typename?: 'PriceRuleTriggerLog', id: string, triggeredAt: any, price?: any | null } & { ' $fragmentName'?: 'ActivationLogFragment' };

export type GetPriceRuleQueryVariables = Exact<{
  id: Scalars['UUID']['input'];
}>;


export type GetPriceRuleQuery = { __typename?: 'Query', priceRule?: { __typename?: 'PriceRule', description?: string | null, id: string, isEnabled: boolean, name: string, createdAt: any, instrument: { __typename?: 'Instrument', id: string, symbol: string }, activationLogs?: { __typename?: 'ActivationLogsConnection', nodes?: Array<(
        { __typename?: 'PriceRuleTriggerLog' }
        & { ' $fragmentRefs'?: { 'ActivationLogFragment': ActivationLogFragment } }
      )> | null } | null, conditions?: { __typename?: 'ConditionsConnection', totalCount: number, edges?: Array<{ __typename?: 'ConditionsEdge', cursor: string, node: { __typename?: 'PriceCondition', value: any, additionalValues?: string | null, conditionType: ConditionType } }> | null } | null } | null };

export type SubscribeToPricesForSymbolSubscriptionVariables = Exact<{
  symbol: Scalars['String']['input'];
}>;


export type SubscribeToPricesForSymbolSubscription = { __typename?: 'Subscription', onPriceUpdated?: { __typename?: 'Price', close: any, high: any, low: any, open: any, symbol: string, volume?: any | null, timestamp: any } | null };

export const PriceItemFragmentDoc = {"kind":"Document","definitions":[{"kind":"FragmentDefinition","name":{"kind":"Name","value":"PriceItem"},"typeCondition":{"kind":"NamedType","name":{"kind":"Name","value":"Price"}},"selectionSet":{"kind":"SelectionSet","selections":[{"kind":"Field","name":{"kind":"Name","value":"bucket"}},{"kind":"Field","name":{"kind":"Name","value":"close"}},{"kind":"Field","name":{"kind":"Name","value":"symbol"}},{"kind":"Field","name":{"kind":"Name","value":"volume"}}]}}]} as unknown as DocumentNode<PriceItemFragment, unknown>;
export const ActivationLogFragmentDoc = {"kind":"Document","definitions":[{"kind":"FragmentDefinition","name":{"kind":"Name","value":"ActivationLog"},"typeCondition":{"kind":"NamedType","name":{"kind":"Name","value":"PriceRuleTriggerLog"}},"selectionSet":{"kind":"SelectionSet","selections":[{"kind":"Field","name":{"kind":"Name","value":"id"}},{"kind":"Field","name":{"kind":"Name","value":"triggeredAt"}},{"kind":"Field","name":{"kind":"Name","value":"price"}}]}}]} as unknown as DocumentNode<ActivationLogFragment, unknown>;
export const GetExchangesDocument = {"kind":"Document","definitions":[{"kind":"OperationDefinition","operation":"query","name":{"kind":"Name","value":"GetExchanges"},"variableDefinitions":[{"kind":"VariableDefinition","variable":{"kind":"Variable","name":{"kind":"Name","value":"take"}},"type":{"kind":"NonNullType","type":{"kind":"NamedType","name":{"kind":"Name","value":"Int"}}}}],"selectionSet":{"kind":"SelectionSet","selections":[{"kind":"Field","name":{"kind":"Name","value":"exchanges"},"arguments":[{"kind":"Argument","name":{"kind":"Name","value":"take"},"value":{"kind":"Variable","name":{"kind":"Name","value":"take"}}}],"selectionSet":{"kind":"SelectionSet","selections":[{"kind":"Field","name":{"kind":"Name","value":"totalCount"}},{"kind":"Field","name":{"kind":"Name","value":"items"},"selectionSet":{"kind":"SelectionSet","selections":[{"kind":"Field","name":{"kind":"Name","value":"name"}},{"kind":"Field","name":{"kind":"Name","value":"description"}}]}}]}}]}}]} as unknown as DocumentNode<GetExchangesQuery, GetExchangesQueryVariables>;
export const MarketNewsDocument = {"kind":"Document","definitions":[{"kind":"OperationDefinition","operation":"query","name":{"kind":"Name","value":"MarketNews"},"selectionSet":{"kind":"SelectionSet","selections":[{"kind":"Field","name":{"kind":"Name","value":"news"},"selectionSet":{"kind":"SelectionSet","selections":[{"kind":"Field","name":{"kind":"Name","value":"id"}},{"kind":"Field","name":{"kind":"Name","value":"headline"}},{"kind":"Field","name":{"kind":"Name","value":"summary"}},{"kind":"Field","name":{"kind":"Name","value":"url"}}]}}]}}]} as unknown as DocumentNode<MarketNewsQuery, MarketNewsQueryVariables>;
export const GetFocusedPriceLineDocument = {"kind":"Document","definitions":[{"kind":"OperationDefinition","operation":"query","name":{"kind":"Name","value":"GetFocusedPriceLine"},"selectionSet":{"kind":"SelectionSet","selections":[{"kind":"Field","alias":{"kind":"Name","value":"ETH"},"name":{"kind":"Name","value":"prices"},"arguments":[{"kind":"Argument","name":{"kind":"Name","value":"last"},"value":{"kind":"IntValue","value":"500"}},{"kind":"Argument","name":{"kind":"Name","value":"interval"},"value":{"kind":"EnumValue","value":"ONE_HOUR"}},{"kind":"Argument","name":{"kind":"Name","value":"where"},"value":{"kind":"ObjectValue","fields":[{"kind":"ObjectField","name":{"kind":"Name","value":"symbol"},"value":{"kind":"ObjectValue","fields":[{"kind":"ObjectField","name":{"kind":"Name","value":"eq"},"value":{"kind":"StringValue","value":"ETHUSDT","block":false}}]}}]}}],"selectionSet":{"kind":"SelectionSet","selections":[{"kind":"Field","name":{"kind":"Name","value":"edges"},"selectionSet":{"kind":"SelectionSet","selections":[{"kind":"Field","name":{"kind":"Name","value":"node"},"selectionSet":{"kind":"SelectionSet","selections":[{"kind":"FragmentSpread","name":{"kind":"Name","value":"PriceItem"}}]}}]}}]}},{"kind":"Field","alias":{"kind":"Name","value":"BTC"},"name":{"kind":"Name","value":"prices"},"arguments":[{"kind":"Argument","name":{"kind":"Name","value":"last"},"value":{"kind":"IntValue","value":"500"}},{"kind":"Argument","name":{"kind":"Name","value":"interval"},"value":{"kind":"EnumValue","value":"ONE_HOUR"}},{"kind":"Argument","name":{"kind":"Name","value":"where"},"value":{"kind":"ObjectValue","fields":[{"kind":"ObjectField","name":{"kind":"Name","value":"symbol"},"value":{"kind":"ObjectValue","fields":[{"kind":"ObjectField","name":{"kind":"Name","value":"eq"},"value":{"kind":"StringValue","value":"BTCUSDT","block":false}}]}}]}}],"selectionSet":{"kind":"SelectionSet","selections":[{"kind":"Field","name":{"kind":"Name","value":"edges"},"selectionSet":{"kind":"SelectionSet","selections":[{"kind":"Field","name":{"kind":"Name","value":"node"},"selectionSet":{"kind":"SelectionSet","selections":[{"kind":"FragmentSpread","name":{"kind":"Name","value":"PriceItem"}}]}}]}}]}}]}},{"kind":"FragmentDefinition","name":{"kind":"Name","value":"PriceItem"},"typeCondition":{"kind":"NamedType","name":{"kind":"Name","value":"Price"}},"selectionSet":{"kind":"SelectionSet","selections":[{"kind":"Field","name":{"kind":"Name","value":"bucket"}},{"kind":"Field","name":{"kind":"Name","value":"close"}},{"kind":"Field","name":{"kind":"Name","value":"symbol"}},{"kind":"Field","name":{"kind":"Name","value":"volume"}}]}}]} as unknown as DocumentNode<GetFocusedPriceLineQuery, GetFocusedPriceLineQueryVariables>;
export const GetFocusedPricesDocument = {"kind":"Document","definitions":[{"kind":"OperationDefinition","operation":"query","name":{"kind":"Name","value":"GetFocusedPrices"},"selectionSet":{"kind":"SelectionSet","selections":[{"kind":"Field","alias":{"kind":"Name","value":"ETH"},"name":{"kind":"Name","value":"prices"},"arguments":[{"kind":"Argument","name":{"kind":"Name","value":"last"},"value":{"kind":"IntValue","value":"1"}},{"kind":"Argument","name":{"kind":"Name","value":"interval"},"value":{"kind":"EnumValue","value":"ONE_MIN"}},{"kind":"Argument","name":{"kind":"Name","value":"where"},"value":{"kind":"ObjectValue","fields":[{"kind":"ObjectField","name":{"kind":"Name","value":"symbol"},"value":{"kind":"ObjectValue","fields":[{"kind":"ObjectField","name":{"kind":"Name","value":"eq"},"value":{"kind":"StringValue","value":"ETHUSDT","block":false}}]}}]}}],"selectionSet":{"kind":"SelectionSet","selections":[{"kind":"Field","name":{"kind":"Name","value":"edges"},"selectionSet":{"kind":"SelectionSet","selections":[{"kind":"Field","name":{"kind":"Name","value":"node"},"selectionSet":{"kind":"SelectionSet","selections":[{"kind":"FragmentSpread","name":{"kind":"Name","value":"PriceItem"}}]}}]}}]}},{"kind":"Field","alias":{"kind":"Name","value":"BTC"},"name":{"kind":"Name","value":"prices"},"arguments":[{"kind":"Argument","name":{"kind":"Name","value":"last"},"value":{"kind":"IntValue","value":"1"}},{"kind":"Argument","name":{"kind":"Name","value":"interval"},"value":{"kind":"EnumValue","value":"ONE_MIN"}},{"kind":"Argument","name":{"kind":"Name","value":"where"},"value":{"kind":"ObjectValue","fields":[{"kind":"ObjectField","name":{"kind":"Name","value":"symbol"},"value":{"kind":"ObjectValue","fields":[{"kind":"ObjectField","name":{"kind":"Name","value":"eq"},"value":{"kind":"StringValue","value":"BTCUSDT","block":false}}]}}]}}],"selectionSet":{"kind":"SelectionSet","selections":[{"kind":"Field","name":{"kind":"Name","value":"edges"},"selectionSet":{"kind":"SelectionSet","selections":[{"kind":"Field","name":{"kind":"Name","value":"node"},"selectionSet":{"kind":"SelectionSet","selections":[{"kind":"FragmentSpread","name":{"kind":"Name","value":"PriceItem"}}]}}]}}]}}]}},{"kind":"FragmentDefinition","name":{"kind":"Name","value":"PriceItem"},"typeCondition":{"kind":"NamedType","name":{"kind":"Name","value":"Price"}},"selectionSet":{"kind":"SelectionSet","selections":[{"kind":"Field","name":{"kind":"Name","value":"bucket"}},{"kind":"Field","name":{"kind":"Name","value":"close"}},{"kind":"Field","name":{"kind":"Name","value":"symbol"}},{"kind":"Field","name":{"kind":"Name","value":"volume"}}]}}]} as unknown as DocumentNode<GetFocusedPricesQuery, GetFocusedPricesQueryVariables>;
export const ActiveRulesDocument = {"kind":"Document","definitions":[{"kind":"OperationDefinition","operation":"query","name":{"kind":"Name","value":"ActiveRules"},"variableDefinitions":[{"kind":"VariableDefinition","variable":{"kind":"Variable","name":{"kind":"Name","value":"first"}},"type":{"kind":"NamedType","name":{"kind":"Name","value":"Int"}}}],"selectionSet":{"kind":"SelectionSet","selections":[{"kind":"Field","name":{"kind":"Name","value":"priceRules"},"arguments":[{"kind":"Argument","name":{"kind":"Name","value":"first"},"value":{"kind":"Variable","name":{"kind":"Name","value":"first"}}},{"kind":"Argument","name":{"kind":"Name","value":"where"},"value":{"kind":"ObjectValue","fields":[{"kind":"ObjectField","name":{"kind":"Name","value":"isEnabled"},"value":{"kind":"ObjectValue","fields":[{"kind":"ObjectField","name":{"kind":"Name","value":"eq"},"value":{"kind":"BooleanValue","value":true}}]}}]}}],"selectionSet":{"kind":"SelectionSet","selections":[{"kind":"Field","name":{"kind":"Name","value":"nodes"},"selectionSet":{"kind":"SelectionSet","selections":[{"kind":"Field","name":{"kind":"Name","value":"id"}},{"kind":"Field","name":{"kind":"Name","value":"name"}},{"kind":"Field","name":{"kind":"Name","value":"description"}},{"kind":"Field","name":{"kind":"Name","value":"lastTriggeredAt"}},{"kind":"Field","name":{"kind":"Name","value":"lastTriggeredPrice"}},{"kind":"Field","name":{"kind":"Name","value":"instrument"},"selectionSet":{"kind":"SelectionSet","selections":[{"kind":"Field","name":{"kind":"Name","value":"id"}},{"kind":"Field","name":{"kind":"Name","value":"baseAsset"}},{"kind":"Field","name":{"kind":"Name","value":"quoteAsset"}},{"kind":"Field","name":{"kind":"Name","value":"symbol"}}]}}]}}]}}]}}]} as unknown as DocumentNode<ActiveRulesQuery, ActiveRulesQueryVariables>;
export const CreatePriceRuleDocument = {"kind":"Document","definitions":[{"kind":"OperationDefinition","operation":"mutation","name":{"kind":"Name","value":"CreatePriceRule"},"variableDefinitions":[{"kind":"VariableDefinition","variable":{"kind":"Variable","name":{"kind":"Name","value":"newRule"}},"type":{"kind":"NonNullType","type":{"kind":"NamedType","name":{"kind":"Name","value":"PriceRuleInput"}}}}],"selectionSet":{"kind":"SelectionSet","selections":[{"kind":"Field","name":{"kind":"Name","value":"createPriceRule"},"arguments":[{"kind":"Argument","name":{"kind":"Name","value":"input"},"value":{"kind":"Variable","name":{"kind":"Name","value":"newRule"}}}],"selectionSet":{"kind":"SelectionSet","selections":[{"kind":"Field","name":{"kind":"Name","value":"id"}},{"kind":"Field","name":{"kind":"Name","value":"name"}},{"kind":"Field","name":{"kind":"Name","value":"description"}}]}}]}}]} as unknown as DocumentNode<CreatePriceRuleMutation, CreatePriceRuleMutationVariables>;
export const GetInstrumentsDocument = {"kind":"Document","definitions":[{"kind":"OperationDefinition","operation":"query","name":{"kind":"Name","value":"GetInstruments"},"variableDefinitions":[{"kind":"VariableDefinition","variable":{"kind":"Variable","name":{"kind":"Name","value":"first"}},"type":{"kind":"NonNullType","type":{"kind":"NamedType","name":{"kind":"Name","value":"Int"}}}}],"selectionSet":{"kind":"SelectionSet","selections":[{"kind":"Field","name":{"kind":"Name","value":"instruments"},"arguments":[{"kind":"Argument","name":{"kind":"Name","value":"first"},"value":{"kind":"Variable","name":{"kind":"Name","value":"first"}}}],"selectionSet":{"kind":"SelectionSet","selections":[{"kind":"Field","name":{"kind":"Name","value":"totalCount"}},{"kind":"Field","name":{"kind":"Name","value":"edges"},"selectionSet":{"kind":"SelectionSet","selections":[{"kind":"Field","name":{"kind":"Name","value":"node"},"selectionSet":{"kind":"SelectionSet","selections":[{"kind":"Field","name":{"kind":"Name","value":"symbol"}},{"kind":"Field","name":{"kind":"Name","value":"id"}}]}}]}}]}}]}}]} as unknown as DocumentNode<GetInstrumentsQuery, GetInstrumentsQueryVariables>;
export const EditPriceRuleDocument = {"kind":"Document","definitions":[{"kind":"OperationDefinition","operation":"mutation","name":{"kind":"Name","value":"editPriceRule"},"variableDefinitions":[{"kind":"VariableDefinition","variable":{"kind":"Variable","name":{"kind":"Name","value":"existingRule"}},"type":{"kind":"NonNullType","type":{"kind":"NamedType","name":{"kind":"Name","value":"PriceRuleInput"}}}}],"selectionSet":{"kind":"SelectionSet","selections":[{"kind":"Field","name":{"kind":"Name","value":"updatePriceRule"},"arguments":[{"kind":"Argument","name":{"kind":"Name","value":"input"},"value":{"kind":"Variable","name":{"kind":"Name","value":"existingRule"}}}],"selectionSet":{"kind":"SelectionSet","selections":[{"kind":"Field","name":{"kind":"Name","value":"id"}},{"kind":"Field","name":{"kind":"Name","value":"name"}},{"kind":"Field","name":{"kind":"Name","value":"description"}}]}}]}}]} as unknown as DocumentNode<EditPriceRuleMutation, EditPriceRuleMutationVariables>;
export const GetPricesForSymbolDocument = {"kind":"Document","definitions":[{"kind":"OperationDefinition","operation":"query","name":{"kind":"Name","value":"GetPricesForSymbol"},"variableDefinitions":[{"kind":"VariableDefinition","variable":{"kind":"Variable","name":{"kind":"Name","value":"symbol"}},"type":{"kind":"NonNullType","type":{"kind":"NamedType","name":{"kind":"Name","value":"String"}}}},{"kind":"VariableDefinition","variable":{"kind":"Variable","name":{"kind":"Name","value":"last"}},"type":{"kind":"NonNullType","type":{"kind":"NamedType","name":{"kind":"Name","value":"Int"}}}},{"kind":"VariableDefinition","variable":{"kind":"Variable","name":{"kind":"Name","value":"interval"}},"type":{"kind":"NonNullType","type":{"kind":"NamedType","name":{"kind":"Name","value":"PriceInterval"}}}}],"selectionSet":{"kind":"SelectionSet","selections":[{"kind":"Field","name":{"kind":"Name","value":"prices"},"arguments":[{"kind":"Argument","name":{"kind":"Name","value":"last"},"value":{"kind":"Variable","name":{"kind":"Name","value":"last"}}},{"kind":"Argument","name":{"kind":"Name","value":"where"},"value":{"kind":"ObjectValue","fields":[{"kind":"ObjectField","name":{"kind":"Name","value":"symbol"},"value":{"kind":"ObjectValue","fields":[{"kind":"ObjectField","name":{"kind":"Name","value":"eq"},"value":{"kind":"Variable","name":{"kind":"Name","value":"symbol"}}}]}}]}},{"kind":"Argument","name":{"kind":"Name","value":"order"},"value":{"kind":"ObjectValue","fields":[{"kind":"ObjectField","name":{"kind":"Name","value":"bucket"},"value":{"kind":"EnumValue","value":"ASC"}}]}},{"kind":"Argument","name":{"kind":"Name","value":"interval"},"value":{"kind":"Variable","name":{"kind":"Name","value":"interval"}}}],"selectionSet":{"kind":"SelectionSet","selections":[{"kind":"Field","name":{"kind":"Name","value":"nodes"},"selectionSet":{"kind":"SelectionSet","selections":[{"kind":"Field","alias":{"kind":"Name","value":"timestamp"},"name":{"kind":"Name","value":"bucket"}},{"kind":"Field","name":{"kind":"Name","value":"close"}},{"kind":"Field","name":{"kind":"Name","value":"high"}},{"kind":"Field","name":{"kind":"Name","value":"low"}},{"kind":"Field","name":{"kind":"Name","value":"open"}},{"kind":"Field","name":{"kind":"Name","value":"symbol"}},{"kind":"Field","name":{"kind":"Name","value":"volume"}}]}}]}}]}}]} as unknown as DocumentNode<GetPricesForSymbolQuery, GetPricesForSymbolQueryVariables>;
export const ToggleRuleStatusDocument = {"kind":"Document","definitions":[{"kind":"OperationDefinition","operation":"mutation","name":{"kind":"Name","value":"ToggleRuleStatus"},"variableDefinitions":[{"kind":"VariableDefinition","variable":{"kind":"Variable","name":{"kind":"Name","value":"id"}},"type":{"kind":"NonNullType","type":{"kind":"NamedType","name":{"kind":"Name","value":"UUID"}}}}],"selectionSet":{"kind":"SelectionSet","selections":[{"kind":"Field","name":{"kind":"Name","value":"togglePriceRule"},"arguments":[{"kind":"Argument","name":{"kind":"Name","value":"id"},"value":{"kind":"Variable","name":{"kind":"Name","value":"id"}}}],"selectionSet":{"kind":"SelectionSet","selections":[{"kind":"Field","name":{"kind":"Name","value":"id"}},{"kind":"Field","name":{"kind":"Name","value":"isEnabled"}}]}}]}}]} as unknown as DocumentNode<ToggleRuleStatusMutation, ToggleRuleStatusMutationVariables>;
export const GetPriceRulesDocument = {"kind":"Document","definitions":[{"kind":"OperationDefinition","operation":"query","name":{"kind":"Name","value":"GetPriceRules"},"variableDefinitions":[{"kind":"VariableDefinition","variable":{"kind":"Variable","name":{"kind":"Name","value":"first"}},"type":{"kind":"NamedType","name":{"kind":"Name","value":"Int"}}}],"selectionSet":{"kind":"SelectionSet","selections":[{"kind":"Field","name":{"kind":"Name","value":"priceRules"},"arguments":[{"kind":"Argument","name":{"kind":"Name","value":"first"},"value":{"kind":"Variable","name":{"kind":"Name","value":"first"}}}],"selectionSet":{"kind":"SelectionSet","selections":[{"kind":"Field","name":{"kind":"Name","value":"edges"},"selectionSet":{"kind":"SelectionSet","selections":[{"kind":"Field","name":{"kind":"Name","value":"node"},"selectionSet":{"kind":"SelectionSet","selections":[{"kind":"Field","name":{"kind":"Name","value":"description"}},{"kind":"Field","name":{"kind":"Name","value":"isEnabled"}},{"kind":"Field","name":{"kind":"Name","value":"id"}},{"kind":"Field","name":{"kind":"Name","value":"name"}},{"kind":"Field","name":{"kind":"Name","value":"instrument"},"selectionSet":{"kind":"SelectionSet","selections":[{"kind":"Field","name":{"kind":"Name","value":"symbol"}}]}},{"kind":"Field","name":{"kind":"Name","value":"createdAt"}}]}}]}},{"kind":"Field","name":{"kind":"Name","value":"totalCount"}},{"kind":"Field","name":{"kind":"Name","value":"pageInfo"},"selectionSet":{"kind":"SelectionSet","selections":[{"kind":"Field","name":{"kind":"Name","value":"hasPreviousPage"}},{"kind":"Field","name":{"kind":"Name","value":"hasNextPage"}}]}}]}}]}}]} as unknown as DocumentNode<GetPriceRulesQuery, GetPriceRulesQueryVariables>;
export const DeletePriceRuleDocument = {"kind":"Document","definitions":[{"kind":"OperationDefinition","operation":"mutation","name":{"kind":"Name","value":"DeletePriceRule"},"variableDefinitions":[{"kind":"VariableDefinition","variable":{"kind":"Variable","name":{"kind":"Name","value":"id"}},"type":{"kind":"NonNullType","type":{"kind":"NamedType","name":{"kind":"Name","value":"UUID"}}}}],"selectionSet":{"kind":"SelectionSet","selections":[{"kind":"Field","name":{"kind":"Name","value":"deletePriceRule"},"arguments":[{"kind":"Argument","name":{"kind":"Name","value":"id"},"value":{"kind":"Variable","name":{"kind":"Name","value":"id"}}}],"selectionSet":{"kind":"SelectionSet","selections":[{"kind":"Field","name":{"kind":"Name","value":"id"}}]}}]}}]} as unknown as DocumentNode<DeletePriceRuleMutation, DeletePriceRuleMutationVariables>;
export const UserSettingsDocument = {"kind":"Document","definitions":[{"kind":"OperationDefinition","operation":"query","name":{"kind":"Name","value":"UserSettings"},"selectionSet":{"kind":"SelectionSet","selections":[{"kind":"Field","name":{"kind":"Name","value":"user"},"selectionSet":{"kind":"SelectionSet","selections":[{"kind":"Field","name":{"kind":"Name","value":"email"}},{"kind":"Field","name":{"kind":"Name","value":"notificationChannels"},"arguments":[{"kind":"Argument","name":{"kind":"Name","value":"first"},"value":{"kind":"IntValue","value":"10"}}],"selectionSet":{"kind":"SelectionSet","selections":[{"kind":"Field","name":{"kind":"Name","value":"nodes"},"selectionSet":{"kind":"SelectionSet","selections":[{"kind":"Field","name":{"kind":"Name","value":"id"}},{"kind":"Field","name":{"kind":"Name","value":"channelType"}},{"kind":"Field","name":{"kind":"Name","value":"telegramUsername"}}]}}]}}]}}]}}]} as unknown as DocumentNode<UserSettingsQuery, UserSettingsQueryVariables>;
export const DisconnectTelegramDocument = {"kind":"Document","definitions":[{"kind":"OperationDefinition","operation":"mutation","name":{"kind":"Name","value":"DisconnectTelegram"},"selectionSet":{"kind":"SelectionSet","selections":[{"kind":"Field","name":{"kind":"Name","value":"deleteTelegramConnection"}}]}}]} as unknown as DocumentNode<DisconnectTelegramMutation, DisconnectTelegramMutationVariables>;
export const GetPriceRuleDocument = {"kind":"Document","definitions":[{"kind":"OperationDefinition","operation":"query","name":{"kind":"Name","value":"GetPriceRule"},"variableDefinitions":[{"kind":"VariableDefinition","variable":{"kind":"Variable","name":{"kind":"Name","value":"id"}},"type":{"kind":"NonNullType","type":{"kind":"NamedType","name":{"kind":"Name","value":"UUID"}}}}],"selectionSet":{"kind":"SelectionSet","selections":[{"kind":"Field","name":{"kind":"Name","value":"priceRule"},"arguments":[{"kind":"Argument","name":{"kind":"Name","value":"id"},"value":{"kind":"Variable","name":{"kind":"Name","value":"id"}}}],"selectionSet":{"kind":"SelectionSet","selections":[{"kind":"Field","name":{"kind":"Name","value":"description"}},{"kind":"Field","name":{"kind":"Name","value":"id"}},{"kind":"Field","name":{"kind":"Name","value":"isEnabled"}},{"kind":"Field","name":{"kind":"Name","value":"name"}},{"kind":"Field","name":{"kind":"Name","value":"instrument"},"selectionSet":{"kind":"SelectionSet","selections":[{"kind":"Field","name":{"kind":"Name","value":"id"}},{"kind":"Field","name":{"kind":"Name","value":"symbol"}}]}},{"kind":"Field","name":{"kind":"Name","value":"createdAt"}},{"kind":"Field","name":{"kind":"Name","value":"activationLogs"},"arguments":[{"kind":"Argument","name":{"kind":"Name","value":"first"},"value":{"kind":"IntValue","value":"10"}}],"selectionSet":{"kind":"SelectionSet","selections":[{"kind":"Field","name":{"kind":"Name","value":"nodes"},"selectionSet":{"kind":"SelectionSet","selections":[{"kind":"FragmentSpread","name":{"kind":"Name","value":"ActivationLog"}}]}}]}},{"kind":"Field","name":{"kind":"Name","value":"conditions"},"arguments":[{"kind":"Argument","name":{"kind":"Name","value":"first"},"value":{"kind":"IntValue","value":"10"}}],"selectionSet":{"kind":"SelectionSet","selections":[{"kind":"Field","name":{"kind":"Name","value":"totalCount"}},{"kind":"Field","name":{"kind":"Name","value":"edges"},"selectionSet":{"kind":"SelectionSet","selections":[{"kind":"Field","name":{"kind":"Name","value":"cursor"}},{"kind":"Field","name":{"kind":"Name","value":"node"},"selectionSet":{"kind":"SelectionSet","selections":[{"kind":"Field","name":{"kind":"Name","value":"value"}},{"kind":"Field","name":{"kind":"Name","value":"additionalValues"}},{"kind":"Field","name":{"kind":"Name","value":"conditionType"}}]}}]}}]}}]}}]}},{"kind":"FragmentDefinition","name":{"kind":"Name","value":"ActivationLog"},"typeCondition":{"kind":"NamedType","name":{"kind":"Name","value":"PriceRuleTriggerLog"}},"selectionSet":{"kind":"SelectionSet","selections":[{"kind":"Field","name":{"kind":"Name","value":"id"}},{"kind":"Field","name":{"kind":"Name","value":"triggeredAt"}},{"kind":"Field","name":{"kind":"Name","value":"price"}}]}}]} as unknown as DocumentNode<GetPriceRuleQuery, GetPriceRuleQueryVariables>;
export const SubscribeToPricesForSymbolDocument = {"kind":"Document","definitions":[{"kind":"OperationDefinition","operation":"subscription","name":{"kind":"Name","value":"SubscribeToPricesForSymbol"},"variableDefinitions":[{"kind":"VariableDefinition","variable":{"kind":"Variable","name":{"kind":"Name","value":"symbol"}},"type":{"kind":"NonNullType","type":{"kind":"NamedType","name":{"kind":"Name","value":"String"}}}}],"selectionSet":{"kind":"SelectionSet","selections":[{"kind":"Field","name":{"kind":"Name","value":"onPriceUpdated"},"arguments":[{"kind":"Argument","name":{"kind":"Name","value":"symbol"},"value":{"kind":"Variable","name":{"kind":"Name","value":"symbol"}}}],"selectionSet":{"kind":"SelectionSet","selections":[{"kind":"Field","alias":{"kind":"Name","value":"timestamp"},"name":{"kind":"Name","value":"bucket"}},{"kind":"Field","name":{"kind":"Name","value":"close"}},{"kind":"Field","name":{"kind":"Name","value":"high"}},{"kind":"Field","name":{"kind":"Name","value":"low"}},{"kind":"Field","name":{"kind":"Name","value":"open"}},{"kind":"Field","name":{"kind":"Name","value":"symbol"}},{"kind":"Field","name":{"kind":"Name","value":"volume"}}]}}]}}]} as unknown as DocumentNode<SubscribeToPricesForSymbolSubscription, SubscribeToPricesForSymbolSubscriptionVariables>;
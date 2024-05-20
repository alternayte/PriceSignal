using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations.PostgreSQL
{
    /// <inheritdoc />
    public partial class HyperTableConfiguration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("SELECT create_hypertable('instrument_prices', by_range('timestamp'), migrate_data => true);");
            migrationBuilder.Sql("CREATE INDEX symbol_exchange_timestamp_idx ON instrument_prices(symbol,timestamp DESC);");

            migrationBuilder.Sql("""
                                 ALTER TABLE instrument_prices SET (
                                     timescaledb.compress,
                                     timescaledb.compress_orderby = 'timestamp DESC',
                                     timescaledb.compress_segmentby = 'symbol, exchange_id'
                                 );
                                 
                                 SELECT add_compression_policy('instrument_prices', INTERVAL '2 weeks');
                                 
                                 """);
            
            
            migrationBuilder.Sql("""
                                 CREATE MATERIALIZED VIEW one_min_candle
                                 WITH (timescaledb.continuous,timescaledb.materialized_only = false) AS
                                    SELECT
                                        symbol,
                                        exchange_id,
                                        time_bucket('1 minute', timestamp) AS bucket,
                                        first(price, timestamp) AS open,
                                        max(price) AS high,
                                        min(price) AS low,
                                        last(price, timestamp) AS close,
                                        sum(volume) AS volume
                                    FROM instrument_prices
                                    GROUP BY symbol, exchange_id, bucket;
                                 """,true);
            
            migrationBuilder.Sql("""
                                 CREATE MATERIALIZED VIEW five_min_candle
                                 WITH (timescaledb.continuous,timescaledb.materialized_only = false) AS
                                    SELECT
                                        symbol,
                                        exchange_id,
                                        time_bucket('5 minutes', timestamp) AS bucket,
                                        first(price, timestamp) AS open,
                                        max(price) AS high,
                                        min(price) AS low,
                                        last(price, timestamp) AS close,
                                        sum(volume) AS volume
                                    FROM instrument_prices
                                    GROUP BY symbol, exchange_id, bucket;
                                 """,true);
            
            migrationBuilder.Sql("""
                                 CREATE MATERIALIZED VIEW ten_min_candle
                                 WITH (timescaledb.continuous,timescaledb.materialized_only = false) AS
                                    SELECT
                                        symbol,
                                        exchange_id,
                                        time_bucket('10 minutes', timestamp) AS bucket,
                                        first(price, timestamp) AS open,
                                        max(price) AS high,
                                        min(price) AS low,
                                        last(price, timestamp) AS close,
                                        sum(volume) AS volume
                                    FROM instrument_prices
                                    GROUP BY symbol, exchange_id, bucket;
                                 """,true);
            
            migrationBuilder.Sql("""
                                 CREATE MATERIALIZED VIEW fifteen_min_candle
                                 WITH (timescaledb.continuous,timescaledb.materialized_only = false) AS
                                    SELECT
                                        symbol,
                                        exchange_id,
                                        time_bucket('15 minutes', timestamp) AS bucket,
                                        first(price, timestamp) AS open,
                                        max(price) AS high,
                                        min(price) AS low,
                                        last(price, timestamp) AS close,
                                        sum(volume) AS volume
                                    FROM instrument_prices
                                    GROUP BY symbol, exchange_id, bucket;
                                 """,true);
            
            migrationBuilder.Sql("""
                                 CREATE MATERIALIZED VIEW one_hour_candle
                                 WITH (timescaledb.continuous,timescaledb.materialized_only = false) AS
                                    SELECT
                                        symbol,
                                        exchange_id,
                                        time_bucket('1 hour', timestamp) AS bucket,
                                        first(price, timestamp) AS open,
                                        max(price) AS high,
                                        min(price) AS low,
                                        last(price, timestamp) AS close,
                                        sum(volume) AS volume
                                    FROM instrument_prices
                                    GROUP BY symbol, exchange_id, bucket;
                                 """,true);
            
            migrationBuilder.Sql("""
                                 SELECT add_continuous_aggregate_policy('one_min_candle', 
                                 start_offset => INTERVAL '1 day', 
                                 end_offset => INTERVAL '1 minute', 
                                 schedule_interval => INTERVAL '1 minute');
                                 """);
            
            migrationBuilder.Sql("""
                                 SELECT add_continuous_aggregate_policy('five_min_candle',
                                 start_offset => INTERVAL '1 day',
                                 end_offset => INTERVAL '5 minutes',
                                 schedule_interval => INTERVAL '5 minutes');
                                 """);
            
            migrationBuilder.Sql("""
                                 SELECT add_continuous_aggregate_policy('ten_min_candle',
                                 start_offset => INTERVAL '1 day',
                                 end_offset => INTERVAL '10 minutes',
                                 schedule_interval => INTERVAL '10 minutes');
                                 """);
            
            migrationBuilder.Sql("""
                                 SELECT add_continuous_aggregate_policy('fifteen_min_candle',
                                 start_offset => INTERVAL '1 day',
                                 end_offset => INTERVAL '15 minute',
                                 schedule_interval => INTERVAL '15 minutes');
                                 """);
            
            migrationBuilder.Sql("""
                                 SELECT add_continuous_aggregate_policy('one_hour_candle',
                                 start_offset => INTERVAL '1 day',
                                 end_offset => INTERVAL '1 hour',
                                 schedule_interval => INTERVAL '1 hour');
                                 """);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP INDEX symbol_exchange_timestamp_idx;");
            migrationBuilder.Sql("DROP MATERIALIZED VIEW one_min_candle;");
            migrationBuilder.Sql("DROP MATERIALIZED VIEW five_min_candle;");
            migrationBuilder.Sql("DROP MATERIALIZED VIEW ten_min_candle;");
            migrationBuilder.Sql("DROP MATERIALIZED VIEW fifteen_min_candle;");
            migrationBuilder.Sql("DROP MATERIALIZED VIEW one_hour_candle;");
            
            migrationBuilder.Sql("SELECT remove_compression_policy('instrument_prices');");
            migrationBuilder.Sql("""
                                 ALTER TABLE instrument_prices SET (
                                     timescaledb.compress = false
                                 );
                                 """);
            migrationBuilder.Sql("SELECT remove_continuous_aggregate_policy('one_min_candle',if_exists=> true);");
            migrationBuilder.Sql("SELECT remove_continuous_aggregate_policy('five_min_candle',if_exists=> true);");
            migrationBuilder.Sql("SELECT remove_continuous_aggregate_policy('ten_min_candle',if_exists=> true);");
            migrationBuilder.Sql("SELECT remove_continuous_aggregate_policy('fifteen_min_candle',if_exists=> true);");
            migrationBuilder.Sql("SELECT remove_continuous_aggregate_policy('one_hour_candle',if_exists=> true);");

        }
    }
}

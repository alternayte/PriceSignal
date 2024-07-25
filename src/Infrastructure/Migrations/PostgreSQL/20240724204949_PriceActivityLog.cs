using System;
using System.Text.Json;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Infrastructure.Migrations.PostgreSQL
{
    /// <inheritdoc />
    public partial class PriceActivityLog : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("Npgsql:Enum:notification_channel_type", "none,sms,email,webhook,telegram,push_notification")
                .Annotation("Npgsql:PostgresExtension:uuid-ossp", ",,")
                .OldAnnotation("Npgsql:PostgresExtension:uuid-ossp", ",,");

            migrationBuilder.AddColumn<bool>(
                name: "is_enabled",
                table: "price_rules",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "last_triggered_at",
                table: "price_rules",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "notification_channel",
                table: "price_rules",
                type: "notification_channel_type",
                nullable: false,
                defaultValue: "none");

            migrationBuilder.CreateTable(
                name: "price_rule_trigger_log",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    price = table.Column<double>(type: "double precision", nullable: true),
                    price_change = table.Column<double>(type: "double precision", nullable: true),
                    price_change_percentage = table.Column<double>(type: "double precision", nullable: true),
                    triggered_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    price_rule_id = table.Column<long>(type: "bigint", nullable: false),
                    price_rule_snapshot = table.Column<JsonDocument>(type: "jsonb", nullable: false),
                    entity_id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuid_generate_v4()"),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()"),
                    modified_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true, defaultValueSql: "now()"),
                    deleted_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_price_rule_trigger_log", x => x.id);
                    table.ForeignKey(
                        name: "fk_price_rule_trigger_log_price_rules_price_rule_id",
                        column: x => x.price_rule_id,
                        principalTable: "price_rules",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_price_rule_trigger_log_price_rule_id",
                table: "price_rule_trigger_log",
                column: "price_rule_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "price_rule_trigger_log");

            migrationBuilder.DropColumn(
                name: "is_enabled",
                table: "price_rules");

            migrationBuilder.DropColumn(
                name: "last_triggered_at",
                table: "price_rules");

            migrationBuilder.DropColumn(
                name: "notification_channel",
                table: "price_rules");

            migrationBuilder.AlterDatabase()
                .Annotation("Npgsql:PostgresExtension:uuid-ossp", ",,")
                .OldAnnotation("Npgsql:Enum:notification_channel_type", "none,sms,email,webhook,telegram,push_notification")
                .OldAnnotation("Npgsql:PostgresExtension:uuid-ossp", ",,");
        }
    }
}

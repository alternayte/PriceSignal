using System;
using System.Text.Json;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations.PostgreSQL
{
    /// <inheritdoc />
    public partial class AddUsers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_price_rule_trigger_log_price_rules_price_rule_id",
                table: "price_rule_trigger_log");

            migrationBuilder.DropPrimaryKey(
                name: "pk_price_rule_trigger_log",
                table: "price_rule_trigger_log");

            migrationBuilder.RenameTable(
                name: "price_rule_trigger_log",
                newName: "price_rule_trigger_logs");

            migrationBuilder.RenameIndex(
                name: "ix_price_rule_trigger_log_price_rule_id",
                table: "price_rule_trigger_logs",
                newName: "ix_price_rule_trigger_logs_price_rule_id");

            migrationBuilder.AddColumn<double>(
                name: "last_triggered_price",
                table: "price_rules",
                type: "double precision",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "user_id",
                table: "price_rules",
                type: "character varying(255)",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "pk_price_rule_trigger_logs",
                table: "price_rule_trigger_logs",
                column: "id");

            migrationBuilder.CreateTable(
                name: "users",
                columns: table => new
                {
                    id = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    email = table.Column<string>(type: "text", nullable: false),
                    stripe_customer_id = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_users", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "subscription",
                columns: table => new
                {
                    id = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    status = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: true),
                    metadata = table.Column<JsonDocument>(type: "jsonb", nullable: true),
                    price_id = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    quantity = table.Column<long>(type: "bigint", nullable: true),
                    cancel_at_period_end = table.Column<bool>(type: "boolean", nullable: true),
                    current_period_start = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    current_period_end = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true, defaultValueSql: "now()"),
                    ended_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    canceled_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    cancel_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    trial_start = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    trial_end = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    user_id = table.Column<string>(type: "character varying(255)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_subscription", x => x.id);
                    table.ForeignKey(
                        name: "fk_subscription_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_price_rules_user_id",
                table: "price_rules",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "ix_subscription_id",
                table: "subscription",
                column: "id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_subscription_user_id",
                table: "subscription",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "ix_users_email",
                table: "users",
                column: "email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_users_id",
                table: "users",
                column: "id",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "fk_price_rule_trigger_logs_price_rules_price_rule_id",
                table: "price_rule_trigger_logs",
                column: "price_rule_id",
                principalTable: "price_rules",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_price_rules_users_user_id",
                table: "price_rules",
                column: "user_id",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_price_rule_trigger_logs_price_rules_price_rule_id",
                table: "price_rule_trigger_logs");

            migrationBuilder.DropForeignKey(
                name: "fk_price_rules_users_user_id",
                table: "price_rules");

            migrationBuilder.DropTable(
                name: "subscription");

            migrationBuilder.DropTable(
                name: "users");

            migrationBuilder.DropIndex(
                name: "ix_price_rules_user_id",
                table: "price_rules");

            migrationBuilder.DropPrimaryKey(
                name: "pk_price_rule_trigger_logs",
                table: "price_rule_trigger_logs");

            migrationBuilder.DropColumn(
                name: "last_triggered_price",
                table: "price_rules");

            migrationBuilder.DropColumn(
                name: "user_id",
                table: "price_rules");

            migrationBuilder.RenameTable(
                name: "price_rule_trigger_logs",
                newName: "price_rule_trigger_log");

            migrationBuilder.RenameIndex(
                name: "ix_price_rule_trigger_logs_price_rule_id",
                table: "price_rule_trigger_log",
                newName: "ix_price_rule_trigger_log_price_rule_id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_price_rule_trigger_log",
                table: "price_rule_trigger_log",
                column: "id");

            migrationBuilder.AddForeignKey(
                name: "fk_price_rule_trigger_log_price_rules_price_rule_id",
                table: "price_rule_trigger_log",
                column: "price_rule_id",
                principalTable: "price_rules",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

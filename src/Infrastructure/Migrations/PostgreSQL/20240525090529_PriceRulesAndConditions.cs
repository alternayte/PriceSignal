using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Infrastructure.Migrations.PostgreSQL
{
    /// <inheritdoc />
    public partial class PriceRulesAndConditions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "price_rules",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    description = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: false),
                    instrument_id = table.Column<long>(type: "bigint", nullable: false),
                    entity_id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuid_generate_v4()"),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    modified_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    deleted_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_price_rules", x => x.id);
                    table.ForeignKey(
                        name: "fk_price_rules_instruments_instrument_id",
                        column: x => x.instrument_id,
                        principalTable: "instruments",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "price_conditions",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    condition_type = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    value = table.Column<double>(type: "double precision", nullable: false),
                    additional_value = table.Column<string>(type: "jsonb", nullable: true),
                    rule_id = table.Column<long>(type: "bigint", nullable: false),
                    entity_id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuid_generate_v4()"),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    modified_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    deleted_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_price_conditions", x => x.id);
                    table.ForeignKey(
                        name: "fk_price_conditions_price_rules_rule_id",
                        column: x => x.rule_id,
                        principalTable: "price_rules",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_price_conditions_rule_id",
                table: "price_conditions",
                column: "rule_id");

            migrationBuilder.CreateIndex(
                name: "ix_price_rules_instrument_id",
                table: "price_rules",
                column: "instrument_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "price_conditions");

            migrationBuilder.DropTable(
                name: "price_rules");
        }
    }
}

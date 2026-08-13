using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EventApp.Infrastructure.Events.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitialEvents : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "events");

            migrationBuilder.CreateTable(
                name: "events",
                schema: "events",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    venue_name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    venue_address = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: false),
                    venue_capacity = table.Column<int>(type: "integer", nullable: false),
                    expected_attendees = table.Column<int>(type: "integer", nullable: false),
                    start_date = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    end_date = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    description = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: false),
                    owner_name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    owner_legal_id = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    status = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    deleted_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_events", x => x.id);
                    table.CheckConstraint("ck_events_end_date_after_start_date", "end_date > start_date");
                    table.CheckConstraint("ck_events_expected_attendees_non_negative", "expected_attendees >= 0");
                    table.CheckConstraint("ck_events_expected_attendees_within_capacity", "expected_attendees <= venue_capacity");
                    table.CheckConstraint("ck_events_venue_capacity_positive", "venue_capacity > 0");
                });

            migrationBuilder.CreateIndex(
                name: "ix_events_owner_legal_id",
                schema: "events",
                table: "events",
                column: "owner_legal_id");

            migrationBuilder.CreateIndex(
                name: "ix_events_start_date",
                schema: "events",
                table: "events",
                column: "start_date");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "events",
                schema: "events");
        }
    }
}

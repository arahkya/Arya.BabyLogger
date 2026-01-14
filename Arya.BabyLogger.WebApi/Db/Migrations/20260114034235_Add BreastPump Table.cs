using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Arya.BabyLogger.WebApi.Db.Migrations
{
    /// <inheritdoc />
    public partial class AddBreastPumpTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BreastPumps",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    PumpTime = table.Column<DateTime>(type: "TEXT", nullable: false),
                    AmountML = table.Column<int>(type: "INTEGER", nullable: false),
                    Note = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BreastPumps", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "BreastPumps",
                columns: new[] { "Id", "AmountML", "Note", "PumpTime" },
                values: new object[,]
                {
                    { new Guid("33333333-3333-3333-3333-333333333301"), 120, "Morning pump", new DateTime(2026, 1, 12, 5, 15, 0, 0, DateTimeKind.Utc) },
                    { new Guid("33333333-3333-3333-3333-333333333302"), 90, "Mid-morning", new DateTime(2026, 1, 12, 8, 45, 0, 0, DateTimeKind.Utc) },
                    { new Guid("33333333-3333-3333-3333-333333333303"), 110, "Noon session", new DateTime(2026, 1, 12, 12, 10, 0, 0, DateTimeKind.Utc) },
                    { new Guid("33333333-3333-3333-3333-333333333304"), 100, "Afternoon pump", new DateTime(2026, 1, 12, 15, 30, 0, 0, DateTimeKind.Utc) },
                    { new Guid("33333333-3333-3333-3333-333333333305"), 130, "Evening pump", new DateTime(2026, 1, 12, 19, 5, 0, 0, DateTimeKind.Utc) },
                    { new Guid("33333333-3333-3333-3333-333333333306"), 115, "Morning pump", new DateTime(2026, 1, 11, 6, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("33333333-3333-3333-3333-333333333307"), 95, "Mid-morning", new DateTime(2026, 1, 11, 9, 20, 0, 0, DateTimeKind.Utc) },
                    { new Guid("33333333-3333-3333-3333-333333333308"), 105, "Noon session", new DateTime(2026, 1, 11, 13, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("33333333-3333-3333-3333-333333333309"), 100, "Afternoon pump", new DateTime(2026, 1, 11, 16, 10, 0, 0, DateTimeKind.Utc) },
                    { new Guid("33333333-3333-3333-3333-333333333310"), 125, "Evening pump", new DateTime(2026, 1, 11, 20, 5, 0, 0, DateTimeKind.Utc) },
                    { new Guid("33333333-3333-3333-3333-333333333311"), 118, "Morning pump", new DateTime(2026, 1, 10, 5, 30, 0, 0, DateTimeKind.Utc) },
                    { new Guid("33333333-3333-3333-3333-333333333312"), 92, "Mid-morning", new DateTime(2026, 1, 10, 9, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("33333333-3333-3333-3333-333333333313"), 108, "Noon session", new DateTime(2026, 1, 10, 12, 40, 0, 0, DateTimeKind.Utc) },
                    { new Guid("33333333-3333-3333-3333-333333333314"), 98, "Afternoon pump", new DateTime(2026, 1, 10, 15, 50, 0, 0, DateTimeKind.Utc) },
                    { new Guid("33333333-3333-3333-3333-333333333315"), 132, "Evening pump", new DateTime(2026, 1, 10, 19, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("33333333-3333-3333-3333-333333333316"), 112, "Morning pump", new DateTime(2026, 1, 9, 6, 10, 0, 0, DateTimeKind.Utc) },
                    { new Guid("33333333-3333-3333-3333-333333333317"), 88, "Mid-morning", new DateTime(2026, 1, 9, 9, 35, 0, 0, DateTimeKind.Utc) },
                    { new Guid("33333333-3333-3333-3333-333333333318"), 107, "Noon session", new DateTime(2026, 1, 9, 13, 5, 0, 0, DateTimeKind.Utc) },
                    { new Guid("33333333-3333-3333-3333-333333333319"), 97, "Afternoon pump", new DateTime(2026, 1, 9, 16, 25, 0, 0, DateTimeKind.Utc) },
                    { new Guid("33333333-3333-3333-3333-333333333320"), 128, "Evening pump", new DateTime(2026, 1, 9, 20, 15, 0, 0, DateTimeKind.Utc) }
                });

            migrationBuilder.CreateIndex(
                name: "IX_BreastPumps_PumpTime",
                table: "BreastPumps",
                column: "PumpTime");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BreastPumps");
        }
    }
}

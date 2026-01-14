using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Arya.BabyLogger.WebApi.Db.Migrations
{
    /// <inheritdoc />
    public partial class AddSleepTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Sleeps",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    SleepStartTime = table.Column<DateTime>(type: "TEXT", nullable: false),
                    SleepEndTime = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Note = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sleeps", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Sleeps",
                columns: new[] { "Id", "Note", "SleepEndTime", "SleepStartTime" },
                values: new object[,]
                {
                    { new Guid("22222222-2222-2222-2222-222222222201"), "Overnight sleep", new DateTime(2026, 1, 12, 2, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 1, 12, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("22222222-2222-2222-2222-222222222202"), "Night wake", new DateTime(2026, 1, 12, 4, 30, 0, 0, DateTimeKind.Utc), new DateTime(2026, 1, 12, 3, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("22222222-2222-2222-2222-222222222203"), "Morning nap", new DateTime(2026, 1, 12, 7, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 1, 12, 6, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("22222222-2222-2222-2222-222222222204"), "Overnight sleep", new DateTime(2026, 1, 11, 2, 15, 0, 0, DateTimeKind.Utc), new DateTime(2026, 1, 11, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("22222222-2222-2222-2222-222222222205"), "Early morning nap", new DateTime(2026, 1, 11, 5, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 1, 11, 4, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("22222222-2222-2222-2222-222222222206"), "Morning nap", new DateTime(2026, 1, 11, 8, 20, 0, 0, DateTimeKind.Utc), new DateTime(2026, 1, 11, 7, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("22222222-2222-2222-2222-222222222207"), "Overnight sleep", new DateTime(2026, 1, 10, 2, 10, 0, 0, DateTimeKind.Utc), new DateTime(2026, 1, 10, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("22222222-2222-2222-2222-222222222208"), "Night wake", new DateTime(2026, 1, 10, 4, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 1, 10, 3, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("22222222-2222-2222-2222-222222222209"), "Morning nap", new DateTime(2026, 1, 10, 7, 15, 0, 0, DateTimeKind.Utc), new DateTime(2026, 1, 10, 6, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("22222222-2222-2222-2222-222222222210"), "Overnight sleep", new DateTime(2026, 1, 9, 1, 50, 0, 0, DateTimeKind.Utc), new DateTime(2026, 1, 9, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("22222222-2222-2222-2222-222222222211"), "Night wake", new DateTime(2026, 1, 9, 4, 10, 0, 0, DateTimeKind.Utc), new DateTime(2026, 1, 9, 3, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("22222222-2222-2222-2222-222222222212"), "Morning nap", new DateTime(2026, 1, 9, 7, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 1, 9, 6, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("22222222-2222-2222-2222-222222222213"), "Overnight sleep", new DateTime(2026, 1, 8, 2, 25, 0, 0, DateTimeKind.Utc), new DateTime(2026, 1, 8, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("22222222-2222-2222-2222-222222222214"), "Night wake", new DateTime(2026, 1, 8, 4, 5, 0, 0, DateTimeKind.Utc), new DateTime(2026, 1, 8, 3, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("22222222-2222-2222-2222-222222222215"), "Morning nap", new DateTime(2026, 1, 8, 7, 35, 0, 0, DateTimeKind.Utc), new DateTime(2026, 1, 8, 6, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("22222222-2222-2222-2222-222222222216"), "Overnight sleep", new DateTime(2026, 1, 7, 2, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 1, 7, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("22222222-2222-2222-2222-222222222217"), "Night wake", new DateTime(2026, 1, 7, 4, 20, 0, 0, DateTimeKind.Utc), new DateTime(2026, 1, 7, 3, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("22222222-2222-2222-2222-222222222218"), "Morning nap", new DateTime(2026, 1, 7, 7, 10, 0, 0, DateTimeKind.Utc), new DateTime(2026, 1, 7, 6, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("22222222-2222-2222-2222-222222222219"), "Overnight sleep", new DateTime(2026, 1, 6, 2, 30, 0, 0, DateTimeKind.Utc), new DateTime(2026, 1, 6, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("22222222-2222-2222-2222-222222222220"), "Night wake", new DateTime(2026, 1, 6, 4, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 1, 6, 3, 0, 0, 0, DateTimeKind.Utc) }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Sleeps_SleepEndTime",
                table: "Sleeps",
                column: "SleepEndTime");

            migrationBuilder.CreateIndex(
                name: "IX_Sleeps_SleepStartTime",
                table: "Sleeps",
                column: "SleepStartTime");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Sleeps");
        }
    }
}

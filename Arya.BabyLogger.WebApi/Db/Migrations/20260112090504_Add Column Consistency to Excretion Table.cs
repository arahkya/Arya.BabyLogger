using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Arya.BabyLogger.WebApi.Db.Migrations
{
    /// <inheritdoc />
    public partial class AddColumnConsistencytoExcretionTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Consistency",
                table: "Excretions",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.InsertData(
                table: "Excretions",
                columns: new[] { "Id", "Consistency", "ExcretionColor", "ExcretionDateTime", "ExcretionLevel", "Note" },
                values: new object[,]
                {
                    { new Guid("11111111-1111-1111-1111-111111111101"), "Watery", "#FFFF00", new DateTime(2026, 1, 12, 6, 0, 0, 0, DateTimeKind.Unspecified), 1, "Normal" },
                    { new Guid("11111111-1111-1111-1111-111111111102"), "Soft", "#8B7355", new DateTime(2026, 1, 11, 6, 20, 0, 0, DateTimeKind.Unspecified), 2, "Soft stool" },
                    { new Guid("11111111-1111-1111-1111-111111111103"), "Hard", "#A9A9A9", new DateTime(2026, 1, 10, 6, 40, 0, 0, DateTimeKind.Unspecified), 3, "Slightly hard" },
                    { new Guid("11111111-1111-1111-1111-111111111104"), "Paste", "#DAA520", new DateTime(2026, 1, 9, 6, 0, 0, 0, DateTimeKind.Unspecified), 4, "Watery" },
                    { new Guid("11111111-1111-1111-1111-111111111105"), "Liquid", "#808080", new DateTime(2026, 1, 8, 6, 20, 0, 0, DateTimeKind.Unspecified), 5, "Normal consistency" },
                    { new Guid("11111111-1111-1111-1111-111111111106"), "Watery", "#FFFF00", new DateTime(2026, 1, 7, 6, 40, 0, 0, DateTimeKind.Unspecified), 6, null },
                    { new Guid("11111111-1111-1111-1111-111111111107"), "Soft", "#8B7355", new DateTime(2026, 1, 6, 6, 0, 0, 0, DateTimeKind.Unspecified), 7, null },
                    { new Guid("11111111-1111-1111-1111-111111111108"), "Hard", "#A9A9A9", new DateTime(2026, 1, 12, 9, 20, 0, 0, DateTimeKind.Unspecified), 8, "Changed diet" },
                    { new Guid("11111111-1111-1111-1111-111111111109"), "Paste", "#DAA520", new DateTime(2026, 1, 11, 9, 40, 0, 0, DateTimeKind.Unspecified), 9, "Morning" },
                    { new Guid("11111111-1111-1111-1111-111111111110"), "Liquid", "#808080", new DateTime(2026, 1, 10, 9, 0, 0, 0, DateTimeKind.Unspecified), 10, "After feeding" },
                    { new Guid("11111111-1111-1111-1111-111111111111"), "Watery", "#FFFF00", new DateTime(2026, 1, 9, 9, 20, 0, 0, DateTimeKind.Unspecified), 1, null },
                    { new Guid("11111111-1111-1111-1111-111111111112"), "Soft", "#8B7355", new DateTime(2026, 1, 8, 9, 40, 0, 0, DateTimeKind.Unspecified), 2, null },
                    { new Guid("11111111-1111-1111-1111-111111111113"), "Hard", "#A9A9A9", new DateTime(2026, 1, 7, 9, 0, 0, 0, DateTimeKind.Unspecified), 3, "Healthy" },
                    { new Guid("11111111-1111-1111-1111-111111111114"), "Paste", "#DAA520", new DateTime(2026, 1, 6, 9, 20, 0, 0, DateTimeKind.Unspecified), 4, "Monitor" },
                    { new Guid("11111111-1111-1111-1111-111111111115"), "Liquid", "#808080", new DateTime(2026, 1, 12, 12, 40, 0, 0, DateTimeKind.Unspecified), 5, null },
                    { new Guid("11111111-1111-1111-1111-111111111116"), "Watery", "#FFFF00", new DateTime(2026, 1, 11, 12, 0, 0, 0, DateTimeKind.Unspecified), 6, "Normal" },
                    { new Guid("11111111-1111-1111-1111-111111111117"), "Soft", "#8B7355", new DateTime(2026, 1, 10, 12, 20, 0, 0, DateTimeKind.Unspecified), 7, "Soft stool" },
                    { new Guid("11111111-1111-1111-1111-111111111118"), "Hard", "#A9A9A9", new DateTime(2026, 1, 9, 12, 40, 0, 0, DateTimeKind.Unspecified), 8, "Slightly hard" },
                    { new Guid("11111111-1111-1111-1111-111111111119"), "Paste", "#DAA520", new DateTime(2026, 1, 8, 12, 0, 0, 0, DateTimeKind.Unspecified), 9, "Watery" },
                    { new Guid("11111111-1111-1111-1111-111111111120"), "Liquid", "#808080", new DateTime(2026, 1, 7, 12, 20, 0, 0, DateTimeKind.Unspecified), 10, "Normal consistency" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Excretions",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111101"));

            migrationBuilder.DeleteData(
                table: "Excretions",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111102"));

            migrationBuilder.DeleteData(
                table: "Excretions",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111103"));

            migrationBuilder.DeleteData(
                table: "Excretions",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111104"));

            migrationBuilder.DeleteData(
                table: "Excretions",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111105"));

            migrationBuilder.DeleteData(
                table: "Excretions",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111106"));

            migrationBuilder.DeleteData(
                table: "Excretions",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111107"));

            migrationBuilder.DeleteData(
                table: "Excretions",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111108"));

            migrationBuilder.DeleteData(
                table: "Excretions",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111109"));

            migrationBuilder.DeleteData(
                table: "Excretions",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111110"));

            migrationBuilder.DeleteData(
                table: "Excretions",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111111"));

            migrationBuilder.DeleteData(
                table: "Excretions",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111112"));

            migrationBuilder.DeleteData(
                table: "Excretions",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111113"));

            migrationBuilder.DeleteData(
                table: "Excretions",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111114"));

            migrationBuilder.DeleteData(
                table: "Excretions",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111115"));

            migrationBuilder.DeleteData(
                table: "Excretions",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111116"));

            migrationBuilder.DeleteData(
                table: "Excretions",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111117"));

            migrationBuilder.DeleteData(
                table: "Excretions",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111118"));

            migrationBuilder.DeleteData(
                table: "Excretions",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111119"));

            migrationBuilder.DeleteData(
                table: "Excretions",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111120"));

            migrationBuilder.DropColumn(
                name: "Consistency",
                table: "Excretions");
        }
    }
}

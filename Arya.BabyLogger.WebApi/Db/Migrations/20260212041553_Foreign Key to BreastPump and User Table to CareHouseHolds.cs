using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Arya.BabyLogger.WebApi.Db.Migrations
{
    /// <inheritdoc />
    public partial class ForeignKeytoBreastPumpandUserTabletoCareHouseHolds : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BreastPumps_CareHouseholds_CareHouseholdId",
                table: "BreastPumps");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_CareHouseholds_CareHouseholdId",
                table: "Users");

            migrationBuilder.DeleteData(
                table: "BreastPumps",
                keyColumn: "Id",
                keyValue: new Guid("33333333-3333-3333-3333-333333333301"));

            migrationBuilder.DeleteData(
                table: "BreastPumps",
                keyColumn: "Id",
                keyValue: new Guid("33333333-3333-3333-3333-333333333302"));

            migrationBuilder.DeleteData(
                table: "BreastPumps",
                keyColumn: "Id",
                keyValue: new Guid("33333333-3333-3333-3333-333333333303"));

            migrationBuilder.DeleteData(
                table: "BreastPumps",
                keyColumn: "Id",
                keyValue: new Guid("33333333-3333-3333-3333-333333333304"));

            migrationBuilder.DeleteData(
                table: "BreastPumps",
                keyColumn: "Id",
                keyValue: new Guid("33333333-3333-3333-3333-333333333305"));

            migrationBuilder.DeleteData(
                table: "BreastPumps",
                keyColumn: "Id",
                keyValue: new Guid("33333333-3333-3333-3333-333333333306"));

            migrationBuilder.DeleteData(
                table: "BreastPumps",
                keyColumn: "Id",
                keyValue: new Guid("33333333-3333-3333-3333-333333333307"));

            migrationBuilder.DeleteData(
                table: "BreastPumps",
                keyColumn: "Id",
                keyValue: new Guid("33333333-3333-3333-3333-333333333308"));

            migrationBuilder.DeleteData(
                table: "BreastPumps",
                keyColumn: "Id",
                keyValue: new Guid("33333333-3333-3333-3333-333333333309"));

            migrationBuilder.DeleteData(
                table: "BreastPumps",
                keyColumn: "Id",
                keyValue: new Guid("33333333-3333-3333-3333-333333333310"));

            migrationBuilder.DeleteData(
                table: "BreastPumps",
                keyColumn: "Id",
                keyValue: new Guid("33333333-3333-3333-3333-333333333311"));

            migrationBuilder.DeleteData(
                table: "BreastPumps",
                keyColumn: "Id",
                keyValue: new Guid("33333333-3333-3333-3333-333333333312"));

            migrationBuilder.DeleteData(
                table: "BreastPumps",
                keyColumn: "Id",
                keyValue: new Guid("33333333-3333-3333-3333-333333333313"));

            migrationBuilder.DeleteData(
                table: "BreastPumps",
                keyColumn: "Id",
                keyValue: new Guid("33333333-3333-3333-3333-333333333314"));

            migrationBuilder.DeleteData(
                table: "BreastPumps",
                keyColumn: "Id",
                keyValue: new Guid("33333333-3333-3333-3333-333333333315"));

            migrationBuilder.DeleteData(
                table: "BreastPumps",
                keyColumn: "Id",
                keyValue: new Guid("33333333-3333-3333-3333-333333333316"));

            migrationBuilder.DeleteData(
                table: "BreastPumps",
                keyColumn: "Id",
                keyValue: new Guid("33333333-3333-3333-3333-333333333317"));

            migrationBuilder.DeleteData(
                table: "BreastPumps",
                keyColumn: "Id",
                keyValue: new Guid("33333333-3333-3333-3333-333333333318"));

            migrationBuilder.DeleteData(
                table: "BreastPumps",
                keyColumn: "Id",
                keyValue: new Guid("33333333-3333-3333-3333-333333333319"));

            migrationBuilder.DeleteData(
                table: "BreastPumps",
                keyColumn: "Id",
                keyValue: new Guid("33333333-3333-3333-3333-333333333320"));

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("44444444-4444-4444-4444-444444444401"));

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("44444444-4444-4444-4444-444444444402"));

            migrationBuilder.AlterColumn<Guid>(
                name: "CareHouseholdId",
                table: "Users",
                type: "TEXT",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "CareHouseholdId",
                table: "BreastPumps",
                type: "TEXT",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_BreastPumps_CareHouseholds_CareHouseholdId",
                table: "BreastPumps",
                column: "CareHouseholdId",
                principalTable: "CareHouseholds",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Users_CareHouseholds_CareHouseholdId",
                table: "Users",
                column: "CareHouseholdId",
                principalTable: "CareHouseholds",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BreastPumps_CareHouseholds_CareHouseholdId",
                table: "BreastPumps");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_CareHouseholds_CareHouseholdId",
                table: "Users");

            migrationBuilder.AlterColumn<Guid>(
                name: "CareHouseholdId",
                table: "Users",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "TEXT");

            migrationBuilder.AlterColumn<Guid>(
                name: "CareHouseholdId",
                table: "BreastPumps",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "TEXT");

            migrationBuilder.InsertData(
                table: "BreastPumps",
                columns: new[] { "Id", "AmountML", "CareHouseholdId", "Note", "PumpTime" },
                values: new object[,]
                {
                    { new Guid("33333333-3333-3333-3333-333333333301"), 120, null, "Morning pump", new DateTime(2026, 1, 12, 5, 15, 0, 0, DateTimeKind.Utc) },
                    { new Guid("33333333-3333-3333-3333-333333333302"), 90, null, "Mid-morning", new DateTime(2026, 1, 12, 8, 45, 0, 0, DateTimeKind.Utc) },
                    { new Guid("33333333-3333-3333-3333-333333333303"), 110, null, "Noon session", new DateTime(2026, 1, 12, 12, 10, 0, 0, DateTimeKind.Utc) },
                    { new Guid("33333333-3333-3333-3333-333333333304"), 100, null, "Afternoon pump", new DateTime(2026, 1, 12, 15, 30, 0, 0, DateTimeKind.Utc) },
                    { new Guid("33333333-3333-3333-3333-333333333305"), 130, null, "Evening pump", new DateTime(2026, 1, 12, 19, 5, 0, 0, DateTimeKind.Utc) },
                    { new Guid("33333333-3333-3333-3333-333333333306"), 115, null, "Morning pump", new DateTime(2026, 1, 11, 6, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("33333333-3333-3333-3333-333333333307"), 95, null, "Mid-morning", new DateTime(2026, 1, 11, 9, 20, 0, 0, DateTimeKind.Utc) },
                    { new Guid("33333333-3333-3333-3333-333333333308"), 105, null, "Noon session", new DateTime(2026, 1, 11, 13, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("33333333-3333-3333-3333-333333333309"), 100, null, "Afternoon pump", new DateTime(2026, 1, 11, 16, 10, 0, 0, DateTimeKind.Utc) },
                    { new Guid("33333333-3333-3333-3333-333333333310"), 125, null, "Evening pump", new DateTime(2026, 1, 11, 20, 5, 0, 0, DateTimeKind.Utc) },
                    { new Guid("33333333-3333-3333-3333-333333333311"), 118, null, "Morning pump", new DateTime(2026, 1, 10, 5, 30, 0, 0, DateTimeKind.Utc) },
                    { new Guid("33333333-3333-3333-3333-333333333312"), 92, null, "Mid-morning", new DateTime(2026, 1, 10, 9, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("33333333-3333-3333-3333-333333333313"), 108, null, "Noon session", new DateTime(2026, 1, 10, 12, 40, 0, 0, DateTimeKind.Utc) },
                    { new Guid("33333333-3333-3333-3333-333333333314"), 98, null, "Afternoon pump", new DateTime(2026, 1, 10, 15, 50, 0, 0, DateTimeKind.Utc) },
                    { new Guid("33333333-3333-3333-3333-333333333315"), 132, null, "Evening pump", new DateTime(2026, 1, 10, 19, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("33333333-3333-3333-3333-333333333316"), 112, null, "Morning pump", new DateTime(2026, 1, 9, 6, 10, 0, 0, DateTimeKind.Utc) },
                    { new Guid("33333333-3333-3333-3333-333333333317"), 88, null, "Mid-morning", new DateTime(2026, 1, 9, 9, 35, 0, 0, DateTimeKind.Utc) },
                    { new Guid("33333333-3333-3333-3333-333333333318"), 107, null, "Noon session", new DateTime(2026, 1, 9, 13, 5, 0, 0, DateTimeKind.Utc) },
                    { new Guid("33333333-3333-3333-3333-333333333319"), 97, null, "Afternoon pump", new DateTime(2026, 1, 9, 16, 25, 0, 0, DateTimeKind.Utc) },
                    { new Guid("33333333-3333-3333-3333-333333333320"), 128, null, "Evening pump", new DateTime(2026, 1, 9, 20, 15, 0, 0, DateTimeKind.Utc) }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "CareHouseholdId", "Email", "PasswordHash", "Username" },
                values: new object[,]
                {
                    { new Guid("44444444-4444-4444-4444-444444444401"), null, "arahk@outlook.com", "a2697f4143cbb043c514129a7bb96a53f48ac829a1413ad6aa09beb10b54f622", "Arahk8986" },
                    { new Guid("44444444-4444-4444-4444-444444444402"), null, "wiparat500267@gmail.com", "85dfffbb42725a20b1c6cc3c78073f39a19281230bc558e6e252f1f6a67973c8", "wiparat500267" }
                });

            migrationBuilder.AddForeignKey(
                name: "FK_BreastPumps_CareHouseholds_CareHouseholdId",
                table: "BreastPumps",
                column: "CareHouseholdId",
                principalTable: "CareHouseholds",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_CareHouseholds_CareHouseholdId",
                table: "Users",
                column: "CareHouseholdId",
                principalTable: "CareHouseholds",
                principalColumn: "Id");
        }
    }
}

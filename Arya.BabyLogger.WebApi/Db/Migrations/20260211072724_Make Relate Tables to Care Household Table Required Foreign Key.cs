using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Arya.BabyLogger.WebApi.Db.Migrations
{
    /// <inheritdoc />
    public partial class MakeRelateTablestoCareHouseholdTableRequiredForeignKey : Migration
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

            migrationBuilder.UpdateData(
                table: "BreastPumps",
                keyColumn: "Id",
                keyValue: new Guid("33333333-3333-3333-3333-333333333301"),
                column: "CareHouseholdId",
                value: new Guid("571b31b8-6b65-48cf-9e8f-81de3f29a6b4"));

            migrationBuilder.UpdateData(
                table: "BreastPumps",
                keyColumn: "Id",
                keyValue: new Guid("33333333-3333-3333-3333-333333333302"),
                column: "CareHouseholdId",
                value: new Guid("571b31b8-6b65-48cf-9e8f-81de3f29a6b4"));

            migrationBuilder.UpdateData(
                table: "BreastPumps",
                keyColumn: "Id",
                keyValue: new Guid("33333333-3333-3333-3333-333333333303"),
                column: "CareHouseholdId",
                value: new Guid("571b31b8-6b65-48cf-9e8f-81de3f29a6b4"));

            migrationBuilder.UpdateData(
                table: "BreastPumps",
                keyColumn: "Id",
                keyValue: new Guid("33333333-3333-3333-3333-333333333304"),
                column: "CareHouseholdId",
                value: new Guid("571b31b8-6b65-48cf-9e8f-81de3f29a6b4"));

            migrationBuilder.UpdateData(
                table: "BreastPumps",
                keyColumn: "Id",
                keyValue: new Guid("33333333-3333-3333-3333-333333333305"),
                column: "CareHouseholdId",
                value: new Guid("571b31b8-6b65-48cf-9e8f-81de3f29a6b4"));

            migrationBuilder.UpdateData(
                table: "BreastPumps",
                keyColumn: "Id",
                keyValue: new Guid("33333333-3333-3333-3333-333333333306"),
                column: "CareHouseholdId",
                value: new Guid("571b31b8-6b65-48cf-9e8f-81de3f29a6b4"));

            migrationBuilder.UpdateData(
                table: "BreastPumps",
                keyColumn: "Id",
                keyValue: new Guid("33333333-3333-3333-3333-333333333307"),
                column: "CareHouseholdId",
                value: new Guid("571b31b8-6b65-48cf-9e8f-81de3f29a6b4"));

            migrationBuilder.UpdateData(
                table: "BreastPumps",
                keyColumn: "Id",
                keyValue: new Guid("33333333-3333-3333-3333-333333333308"),
                column: "CareHouseholdId",
                value: new Guid("571b31b8-6b65-48cf-9e8f-81de3f29a6b4"));

            migrationBuilder.UpdateData(
                table: "BreastPumps",
                keyColumn: "Id",
                keyValue: new Guid("33333333-3333-3333-3333-333333333309"),
                column: "CareHouseholdId",
                value: new Guid("571b31b8-6b65-48cf-9e8f-81de3f29a6b4"));

            migrationBuilder.UpdateData(
                table: "BreastPumps",
                keyColumn: "Id",
                keyValue: new Guid("33333333-3333-3333-3333-333333333310"),
                column: "CareHouseholdId",
                value: new Guid("571b31b8-6b65-48cf-9e8f-81de3f29a6b4"));

            migrationBuilder.UpdateData(
                table: "BreastPumps",
                keyColumn: "Id",
                keyValue: new Guid("33333333-3333-3333-3333-333333333311"),
                column: "CareHouseholdId",
                value: new Guid("571b31b8-6b65-48cf-9e8f-81de3f29a6b4"));

            migrationBuilder.UpdateData(
                table: "BreastPumps",
                keyColumn: "Id",
                keyValue: new Guid("33333333-3333-3333-3333-333333333312"),
                column: "CareHouseholdId",
                value: new Guid("571b31b8-6b65-48cf-9e8f-81de3f29a6b4"));

            migrationBuilder.UpdateData(
                table: "BreastPumps",
                keyColumn: "Id",
                keyValue: new Guid("33333333-3333-3333-3333-333333333313"),
                column: "CareHouseholdId",
                value: new Guid("571b31b8-6b65-48cf-9e8f-81de3f29a6b4"));

            migrationBuilder.UpdateData(
                table: "BreastPumps",
                keyColumn: "Id",
                keyValue: new Guid("33333333-3333-3333-3333-333333333314"),
                column: "CareHouseholdId",
                value: new Guid("571b31b8-6b65-48cf-9e8f-81de3f29a6b4"));

            migrationBuilder.UpdateData(
                table: "BreastPumps",
                keyColumn: "Id",
                keyValue: new Guid("33333333-3333-3333-3333-333333333315"),
                column: "CareHouseholdId",
                value: new Guid("571b31b8-6b65-48cf-9e8f-81de3f29a6b4"));

            migrationBuilder.UpdateData(
                table: "BreastPumps",
                keyColumn: "Id",
                keyValue: new Guid("33333333-3333-3333-3333-333333333316"),
                column: "CareHouseholdId",
                value: new Guid("571b31b8-6b65-48cf-9e8f-81de3f29a6b4"));

            migrationBuilder.UpdateData(
                table: "BreastPumps",
                keyColumn: "Id",
                keyValue: new Guid("33333333-3333-3333-3333-333333333317"),
                column: "CareHouseholdId",
                value: new Guid("571b31b8-6b65-48cf-9e8f-81de3f29a6b4"));

            migrationBuilder.UpdateData(
                table: "BreastPumps",
                keyColumn: "Id",
                keyValue: new Guid("33333333-3333-3333-3333-333333333318"),
                column: "CareHouseholdId",
                value: new Guid("571b31b8-6b65-48cf-9e8f-81de3f29a6b4"));

            migrationBuilder.UpdateData(
                table: "BreastPumps",
                keyColumn: "Id",
                keyValue: new Guid("33333333-3333-3333-3333-333333333319"),
                column: "CareHouseholdId",
                value: new Guid("571b31b8-6b65-48cf-9e8f-81de3f29a6b4"));

            migrationBuilder.UpdateData(
                table: "BreastPumps",
                keyColumn: "Id",
                keyValue: new Guid("33333333-3333-3333-3333-333333333320"),
                column: "CareHouseholdId",
                value: new Guid("571b31b8-6b65-48cf-9e8f-81de3f29a6b4"));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("44444444-4444-4444-4444-444444444401"),
                column: "CareHouseholdId",
                value: new Guid("571b31b8-6b65-48cf-9e8f-81de3f29a6b4"));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("44444444-4444-4444-4444-444444444402"),
                column: "CareHouseholdId",
                value: new Guid("571b31b8-6b65-48cf-9e8f-81de3f29a6b4"));

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

            migrationBuilder.UpdateData(
                table: "BreastPumps",
                keyColumn: "Id",
                keyValue: new Guid("33333333-3333-3333-3333-333333333301"),
                column: "CareHouseholdId",
                value: null);

            migrationBuilder.UpdateData(
                table: "BreastPumps",
                keyColumn: "Id",
                keyValue: new Guid("33333333-3333-3333-3333-333333333302"),
                column: "CareHouseholdId",
                value: null);

            migrationBuilder.UpdateData(
                table: "BreastPumps",
                keyColumn: "Id",
                keyValue: new Guid("33333333-3333-3333-3333-333333333303"),
                column: "CareHouseholdId",
                value: null);

            migrationBuilder.UpdateData(
                table: "BreastPumps",
                keyColumn: "Id",
                keyValue: new Guid("33333333-3333-3333-3333-333333333304"),
                column: "CareHouseholdId",
                value: null);

            migrationBuilder.UpdateData(
                table: "BreastPumps",
                keyColumn: "Id",
                keyValue: new Guid("33333333-3333-3333-3333-333333333305"),
                column: "CareHouseholdId",
                value: null);

            migrationBuilder.UpdateData(
                table: "BreastPumps",
                keyColumn: "Id",
                keyValue: new Guid("33333333-3333-3333-3333-333333333306"),
                column: "CareHouseholdId",
                value: null);

            migrationBuilder.UpdateData(
                table: "BreastPumps",
                keyColumn: "Id",
                keyValue: new Guid("33333333-3333-3333-3333-333333333307"),
                column: "CareHouseholdId",
                value: null);

            migrationBuilder.UpdateData(
                table: "BreastPumps",
                keyColumn: "Id",
                keyValue: new Guid("33333333-3333-3333-3333-333333333308"),
                column: "CareHouseholdId",
                value: null);

            migrationBuilder.UpdateData(
                table: "BreastPumps",
                keyColumn: "Id",
                keyValue: new Guid("33333333-3333-3333-3333-333333333309"),
                column: "CareHouseholdId",
                value: null);

            migrationBuilder.UpdateData(
                table: "BreastPumps",
                keyColumn: "Id",
                keyValue: new Guid("33333333-3333-3333-3333-333333333310"),
                column: "CareHouseholdId",
                value: null);

            migrationBuilder.UpdateData(
                table: "BreastPumps",
                keyColumn: "Id",
                keyValue: new Guid("33333333-3333-3333-3333-333333333311"),
                column: "CareHouseholdId",
                value: null);

            migrationBuilder.UpdateData(
                table: "BreastPumps",
                keyColumn: "Id",
                keyValue: new Guid("33333333-3333-3333-3333-333333333312"),
                column: "CareHouseholdId",
                value: null);

            migrationBuilder.UpdateData(
                table: "BreastPumps",
                keyColumn: "Id",
                keyValue: new Guid("33333333-3333-3333-3333-333333333313"),
                column: "CareHouseholdId",
                value: null);

            migrationBuilder.UpdateData(
                table: "BreastPumps",
                keyColumn: "Id",
                keyValue: new Guid("33333333-3333-3333-3333-333333333314"),
                column: "CareHouseholdId",
                value: null);

            migrationBuilder.UpdateData(
                table: "BreastPumps",
                keyColumn: "Id",
                keyValue: new Guid("33333333-3333-3333-3333-333333333315"),
                column: "CareHouseholdId",
                value: null);

            migrationBuilder.UpdateData(
                table: "BreastPumps",
                keyColumn: "Id",
                keyValue: new Guid("33333333-3333-3333-3333-333333333316"),
                column: "CareHouseholdId",
                value: null);

            migrationBuilder.UpdateData(
                table: "BreastPumps",
                keyColumn: "Id",
                keyValue: new Guid("33333333-3333-3333-3333-333333333317"),
                column: "CareHouseholdId",
                value: null);

            migrationBuilder.UpdateData(
                table: "BreastPumps",
                keyColumn: "Id",
                keyValue: new Guid("33333333-3333-3333-3333-333333333318"),
                column: "CareHouseholdId",
                value: null);

            migrationBuilder.UpdateData(
                table: "BreastPumps",
                keyColumn: "Id",
                keyValue: new Guid("33333333-3333-3333-3333-333333333319"),
                column: "CareHouseholdId",
                value: null);

            migrationBuilder.UpdateData(
                table: "BreastPumps",
                keyColumn: "Id",
                keyValue: new Guid("33333333-3333-3333-3333-333333333320"),
                column: "CareHouseholdId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("44444444-4444-4444-4444-444444444401"),
                column: "CareHouseholdId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("44444444-4444-4444-4444-444444444402"),
                column: "CareHouseholdId",
                value: null);

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

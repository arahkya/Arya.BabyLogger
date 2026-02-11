using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Arya.BabyLogger.WebApi.Db.Migrations
{
    /// <inheritdoc />
    public partial class CreateCareHouseholdTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "CareHouseholdId",
                table: "Users",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "CareHouseholdId",
                table: "BreastPumps",
                type: "TEXT",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "CareHouseholds",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CareHouseholds", x => x.Id);
                });

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

            migrationBuilder.CreateIndex(
                name: "IX_Users_CareHouseholdId",
                table: "Users",
                column: "CareHouseholdId");

            migrationBuilder.CreateIndex(
                name: "IX_BreastPumps_CareHouseholdId",
                table: "BreastPumps",
                column: "CareHouseholdId");

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BreastPumps_CareHouseholds_CareHouseholdId",
                table: "BreastPumps");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_CareHouseholds_CareHouseholdId",
                table: "Users");

            migrationBuilder.DropTable(
                name: "CareHouseholds");

            migrationBuilder.DropIndex(
                name: "IX_Users_CareHouseholdId",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_BreastPumps_CareHouseholdId",
                table: "BreastPumps");

            migrationBuilder.DropColumn(
                name: "CareHouseholdId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "CareHouseholdId",
                table: "BreastPumps");
        }
    }
}

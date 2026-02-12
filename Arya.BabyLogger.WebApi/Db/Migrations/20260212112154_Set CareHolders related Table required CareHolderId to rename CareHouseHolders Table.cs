using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Arya.BabyLogger.WebApi.Db.Migrations
{
    /// <inheritdoc />
    public partial class SetCareHoldersrelatedTablerequiredCareHolderIdtorenameCareHouseHoldersTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BreastPumps_CareHolders_CareHolderId",
                table: "BreastPumps");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_CareHolders_CareHolderId",
                table: "Users");

            migrationBuilder.AlterColumn<string>(
                name: "CareHolderId",
                table: "Users",
                type: "TEXT",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CareHolderId",
                table: "BreastPumps",
                type: "TEXT",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_BreastPumps_CareHolders_CareHolderId",
                table: "BreastPumps",
                column: "CareHolderId",
                principalTable: "CareHolders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Users_CareHolders_CareHolderId",
                table: "Users",
                column: "CareHolderId",
                principalTable: "CareHolders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BreastPumps_CareHolders_CareHolderId",
                table: "BreastPumps");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_CareHolders_CareHolderId",
                table: "Users");

            migrationBuilder.AlterColumn<string>(
                name: "CareHolderId",
                table: "Users",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT");

            migrationBuilder.AlterColumn<string>(
                name: "CareHolderId",
                table: "BreastPumps",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT");

            migrationBuilder.AddForeignKey(
                name: "FK_BreastPumps_CareHolders_CareHolderId",
                table: "BreastPumps",
                column: "CareHolderId",
                principalTable: "CareHolders",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_CareHolders_CareHolderId",
                table: "Users",
                column: "CareHolderId",
                principalTable: "CareHolders",
                principalColumn: "Id");
        }
    }
}

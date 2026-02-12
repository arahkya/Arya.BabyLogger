using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Arya.BabyLogger.WebApi.Db.Migrations
{
    /// <inheritdoc />
    public partial class RemoveCareHouseHoldsTable : Migration
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

            migrationBuilder.DropTable(
                name: "CareHouseholds");

            migrationBuilder.DropColumn(
                name: "CareHouseholdId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "CareHouseholdId",
                table: "BreastPumps");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CareHouseholdId",
                table: "Users",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "CareHouseholdId",
                table: "BreastPumps",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "CareHouseholds",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CareHouseholds", x => x.Id);
                });

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
    }
}

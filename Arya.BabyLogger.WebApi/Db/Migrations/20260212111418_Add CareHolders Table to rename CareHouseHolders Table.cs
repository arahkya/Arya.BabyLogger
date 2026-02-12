using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Arya.BabyLogger.WebApi.Db.Migrations
{
    /// <inheritdoc />
    public partial class AddCareHoldersTabletorenameCareHouseHoldersTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CareHolderId",
                table: "Users",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CareHolderId",
                table: "BreastPumps",
                type: "TEXT",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "CareHolders",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CareHolders", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Users_CareHolderId",
                table: "Users",
                column: "CareHolderId");

            migrationBuilder.CreateIndex(
                name: "IX_BreastPumps_CareHolderId",
                table: "BreastPumps",
                column: "CareHolderId");

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BreastPumps_CareHolders_CareHolderId",
                table: "BreastPumps");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_CareHolders_CareHolderId",
                table: "Users");

            migrationBuilder.DropTable(
                name: "CareHolders");

            migrationBuilder.DropIndex(
                name: "IX_Users_CareHolderId",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_BreastPumps_CareHolderId",
                table: "BreastPumps");

            migrationBuilder.DropColumn(
                name: "CareHolderId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "CareHolderId",
                table: "BreastPumps");
        }
    }
}

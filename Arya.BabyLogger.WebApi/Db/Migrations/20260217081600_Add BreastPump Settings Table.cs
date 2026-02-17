using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Arya.BabyLogger.WebApi.Db.Migrations
{
    /// <inheritdoc />
    public partial class AddBreastPumpSettingsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BreastPumpSettings",
                columns: table => new
                {
                    CareHolderId = table.Column<string>(type: "TEXT", nullable: false),
                    PumpIntervalHours = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BreastPumpSettings", x => x.CareHolderId);
                    table.ForeignKey(
                        name: "FK_BreastPumpSettings_CareHolders_CareHolderId",
                        column: x => x.CareHolderId,
                        principalTable: "CareHolders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BreastPumpSettings");
        }
    }
}

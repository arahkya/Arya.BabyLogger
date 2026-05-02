using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Arya.BabyLogger.WebApi.Db.Migrations
{
    /// <inheritdoc />
    public partial class InitialSqlServer : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CareHolders",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    InviteCode = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    InviteUserEmail = table.Column<string>(type: "nvarchar(70)", maxLength: 70, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CareHolders", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Excretions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ExcretionDateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ExcretionLevel = table.Column<int>(type: "int", nullable: false),
                    ExcretionColor = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Consistency = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Note = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Excretions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Feeds",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Time = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Note = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Amount = table.Column<double>(type: "float", nullable: false),
                    Unit = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Feeds", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Sleeps",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SleepStartTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SleepEndTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Note = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sleeps", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BreastPumps",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PumpTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AmountML = table.Column<int>(type: "int", nullable: false),
                    Note = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CareHolderId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BreastPumps", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BreastPumps_CareHolders_CareHolderId",
                        column: x => x.CareHolderId,
                        principalTable: "CareHolders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BreastPumpSettings",
                columns: table => new
                {
                    CareHolderId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PumpIntervalHours = table.Column<int>(type: "int", nullable: false)
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

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Username = table.Column<string>(type: "nvarchar(12)", maxLength: 12, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    CareHolderId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Users_CareHolders_CareHolderId",
                        column: x => x.CareHolderId,
                        principalTable: "CareHolders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ResetPasswordRequests",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SecretCode = table.Column<string>(type: "nvarchar(6)", maxLength: 6, nullable: false),
                    CreatedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ResetPasswordRequests", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ResetPasswordRequests_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BreastPumps_CareHolderId",
                table: "BreastPumps",
                column: "CareHolderId");

            migrationBuilder.CreateIndex(
                name: "IX_BreastPumps_PumpTime",
                table: "BreastPumps",
                column: "PumpTime");

            migrationBuilder.CreateIndex(
                name: "IX_ResetPasswordRequests_UserId",
                table: "ResetPasswordRequests",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Sleeps_SleepEndTime",
                table: "Sleeps",
                column: "SleepEndTime");

            migrationBuilder.CreateIndex(
                name: "IX_Sleeps_SleepStartTime",
                table: "Sleeps",
                column: "SleepStartTime");

            migrationBuilder.CreateIndex(
                name: "IX_Users_CareHolderId",
                table: "Users",
                column: "CareHolderId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BreastPumps");

            migrationBuilder.DropTable(
                name: "BreastPumpSettings");

            migrationBuilder.DropTable(
                name: "Excretions");

            migrationBuilder.DropTable(
                name: "Feeds");

            migrationBuilder.DropTable(
                name: "ResetPasswordRequests");

            migrationBuilder.DropTable(
                name: "Sleeps");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "CareHolders");
        }
    }
}

using System;
using Microsoft.EntityFrameworkCore.Migrations;
using RingSoft.DevLogix.DataAccess;

#nullable disable

namespace RingSoft.DevLogix.Sqlite.Migrations
{
    /// <inheritdoc />
    public partial class SysPrefs : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SystemPreferences",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SystemPreferences", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SystemPreferencesHolidays",
                columns: table => new
                {
                    SystemPreferencesId = table.Column<int>(type: "integer", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime", nullable: false),
                    Name = table.Column<string>(type: "nvarchar", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SystemPreferencesHolidays", x => new { x.SystemPreferencesId, x.Date });
                    table.ForeignKey(
                        name: "FK_SystemPreferencesHolidays_SystemPreferences_SystemPreferencesId",
                        column: x => x.SystemPreferencesId,
                        principalTable: "SystemPreferences",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SystemPreferencesHolidays");

            migrationBuilder.DropTable(
                name: "SystemPreferences");
        }
    }
}

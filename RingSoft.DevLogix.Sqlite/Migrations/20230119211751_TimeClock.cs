using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RingSoft.DevLogix.Sqlite.Migrations
{
    /// <inheritdoc />
    public partial class TimeClock : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TimeClocks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    PunchInDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    PunchOutDate = table.Column<DateTime>(type: "TEXT", nullable: true),
                    MinutesSpent = table.Column<decimal>(type: "TEXT", nullable: true),
                    UserId = table.Column<int>(type: "integer", nullable: false),
                    ErrorId = table.Column<int>(type: "integer", nullable: true),
                    Notes = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TimeClocks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TimeClocks_Errors_ErrorId",
                        column: x => x.ErrorId,
                        principalTable: "Errors",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_TimeClocks_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TimeClocks_ErrorId",
                table: "TimeClocks",
                column: "ErrorId");

            migrationBuilder.CreateIndex(
                name: "IX_TimeClocks_UserId",
                table: "TimeClocks",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TimeClocks");
        }
    }
}

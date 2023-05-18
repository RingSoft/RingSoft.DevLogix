using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RingSoft.DevLogix.Sqlite.Migrations
{
    /// <inheritdoc />
    public partial class TimeClockTestingOutline : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TestingOutlineId",
                table: "TimeClocks",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_TimeClocks_TestingOutlineId",
                table: "TimeClocks",
                column: "TestingOutlineId");

            migrationBuilder.AddForeignKey(
                name: "FK_TimeClocks_TestingOutlines_TestingOutlineId",
                table: "TimeClocks",
                column: "TestingOutlineId",
                principalTable: "TestingOutlines",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TimeClocks_TestingOutlines_TestingOutlineId",
                table: "TimeClocks");

            migrationBuilder.DropIndex(
                name: "IX_TimeClocks_TestingOutlineId",
                table: "TimeClocks");

            migrationBuilder.DropColumn(
                name: "TestingOutlineId",
                table: "TimeClocks");
        }
    }
}

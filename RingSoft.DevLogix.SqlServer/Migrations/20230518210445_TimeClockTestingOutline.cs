using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RingSoft.DevLogix.SqlServer.Migrations
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

            migrationBuilder.AlterColumn<decimal>(
                name: "PercentWidth",
                table: "AdvancedFindColumns",
                type: "numeric(38,17)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(18,0)");

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

            migrationBuilder.AlterColumn<decimal>(
                name: "PercentWidth",
                table: "AdvancedFindColumns",
                type: "numeric(18,0)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(38,17)");
        }
    }
}

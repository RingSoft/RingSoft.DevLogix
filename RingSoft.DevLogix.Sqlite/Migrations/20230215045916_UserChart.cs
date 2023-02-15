using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RingSoft.DevLogix.Sqlite.Migrations
{
    /// <inheritdoc />
    public partial class UserChart : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DefaultChartId",
                table: "Users",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_DefaultChartId",
                table: "Users",
                column: "DefaultChartId");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_DevLogixCharts_DefaultChartId",
                table: "Users",
                column: "DefaultChartId",
                principalTable: "DevLogixCharts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_DevLogixCharts_DefaultChartId",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_DefaultChartId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "DefaultChartId",
                table: "Users");
        }
    }
}

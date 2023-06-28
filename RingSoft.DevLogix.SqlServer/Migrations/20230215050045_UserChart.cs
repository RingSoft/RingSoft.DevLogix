using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RingSoft.DevLogix.SqlServer.Migrations
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

            migrationBuilder.AlterColumn<double>(
                name: "PercentWidth",
                table: "AdvancedFindColumns",
                type: "numeric(38,17)",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "numeric(18,0)");

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

            migrationBuilder.AlterColumn<double>(
                name: "PercentWidth",
                table: "AdvancedFindColumns",
                type: "numeric(18,0)",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "numeric(38,17)");
        }
    }
}

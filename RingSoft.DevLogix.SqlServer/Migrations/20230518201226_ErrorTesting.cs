using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RingSoft.DevLogix.SqlServer.Migrations
{
    /// <inheritdoc />
    public partial class ErrorTesting : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TestingOutlineId",
                table: "Errors",
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
                name: "IX_Errors_TestingOutlineId",
                table: "Errors",
                column: "TestingOutlineId");

            migrationBuilder.AddForeignKey(
                name: "FK_Errors_TestingOutlines_TestingOutlineId",
                table: "Errors",
                column: "TestingOutlineId",
                principalTable: "TestingOutlines",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Errors_TestingOutlines_TestingOutlineId",
                table: "Errors");

            migrationBuilder.DropIndex(
                name: "IX_Errors_TestingOutlineId",
                table: "Errors");

            migrationBuilder.DropColumn(
                name: "TestingOutlineId",
                table: "Errors");

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

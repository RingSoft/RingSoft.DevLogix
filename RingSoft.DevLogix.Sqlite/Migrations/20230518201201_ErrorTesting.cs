using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RingSoft.DevLogix.Sqlite.Migrations
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
        }
    }
}

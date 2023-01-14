using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RingSoft.DevLogix.Sqlite.Migrations
{
    /// <inheritdoc />
    public partial class FoundUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "FixedByByUserId",
                table: "Errors",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "FoundByUserId",
                table: "Errors",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Errors_FixedByByUserId",
                table: "Errors",
                column: "FixedByByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Errors_FoundByUserId",
                table: "Errors",
                column: "FoundByUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Errors_Users_FixedByByUserId",
                table: "Errors",
                column: "FixedByByUserId",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Errors_Users_FoundByUserId",
                table: "Errors",
                column: "FoundByUserId",
                principalTable: "Users",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Errors_Users_FixedByByUserId",
                table: "Errors");

            migrationBuilder.DropForeignKey(
                name: "FK_Errors_Users_FoundByUserId",
                table: "Errors");

            migrationBuilder.DropIndex(
                name: "IX_Errors_FixedByByUserId",
                table: "Errors");

            migrationBuilder.DropIndex(
                name: "IX_Errors_FoundByUserId",
                table: "Errors");

            migrationBuilder.DropColumn(
                name: "FixedByByUserId",
                table: "Errors");

            migrationBuilder.DropColumn(
                name: "FoundByUserId",
                table: "Errors");
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RingSoft.DevLogix.Sqlite.Migrations
{
    /// <inheritdoc />
    public partial class Supervisor : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SupervisorId",
                table: "Users",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_SupervisorId",
                table: "Users",
                column: "SupervisorId");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Users_SupervisorId",
                table: "Users",
                column: "SupervisorId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_Users_SupervisorId",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_SupervisorId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "SupervisorId",
                table: "Users");
        }
    }
}

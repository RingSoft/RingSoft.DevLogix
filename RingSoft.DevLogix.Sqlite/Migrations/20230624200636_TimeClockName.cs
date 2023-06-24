using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RingSoft.DevLogix.Sqlite.Migrations
{
    /// <inheritdoc />
    public partial class TimeClockName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Customer_Users_AssignedUserId",
                table: "Customer");

            migrationBuilder.DropIndex(
                name: "IX_Customer_AssignedUserId",
                table: "Customer");

            migrationBuilder.DropColumn(
                name: "AssignedUserId",
                table: "Customer");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "TimeClocks",
                type: "nvarchar",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "Customer",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Customer_UserId",
                table: "Customer",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Customer_Users_UserId",
                table: "Customer",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Customer_Users_UserId",
                table: "Customer");

            migrationBuilder.DropIndex(
                name: "IX_Customer_UserId",
                table: "Customer");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "TimeClocks");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Customer");

            migrationBuilder.AddColumn<int>(
                name: "AssignedUserId",
                table: "Customer",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Customer_AssignedUserId",
                table: "Customer",
                column: "AssignedUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Customer_Users_AssignedUserId",
                table: "Customer",
                column: "AssignedUserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}

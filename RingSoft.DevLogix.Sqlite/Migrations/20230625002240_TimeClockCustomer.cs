using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RingSoft.DevLogix.Sqlite.Migrations
{
    /// <inheritdoc />
    public partial class TimeClockCustomer : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CustomerId",
                table: "TimeClocks",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_TimeClocks_CustomerId",
                table: "TimeClocks",
                column: "CustomerId");

            migrationBuilder.AddForeignKey(
                name: "FK_TimeClocks_Customer_CustomerId",
                table: "TimeClocks",
                column: "CustomerId",
                principalTable: "Customer",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TimeClocks_Customer_CustomerId",
                table: "TimeClocks");

            migrationBuilder.DropIndex(
                name: "IX_TimeClocks_CustomerId",
                table: "TimeClocks");

            migrationBuilder.DropColumn(
                name: "CustomerId",
                table: "TimeClocks");
        }
    }
}

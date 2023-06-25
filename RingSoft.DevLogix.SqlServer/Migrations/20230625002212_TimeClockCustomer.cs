using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RingSoft.DevLogix.SqlServer.Migrations
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

            migrationBuilder.AlterColumn<decimal>(
                name: "PercentWidth",
                table: "AdvancedFindColumns",
                type: "numeric(38,17)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(18,0)");

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

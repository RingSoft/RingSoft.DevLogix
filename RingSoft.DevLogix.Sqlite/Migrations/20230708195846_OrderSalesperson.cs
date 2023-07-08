using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RingSoft.DevLogix.Sqlite.Migrations
{
    /// <inheritdoc />
    public partial class OrderSalesperson : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SalespersonId",
                table: "Order",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Order_SalespersonId",
                table: "Order",
                column: "SalespersonId");

            migrationBuilder.AddForeignKey(
                name: "FK_Order_Users_SalespersonId",
                table: "Order",
                column: "SalespersonId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Order_Users_SalespersonId",
                table: "Order");

            migrationBuilder.DropIndex(
                name: "IX_Order_SalespersonId",
                table: "Order");

            migrationBuilder.DropColumn(
                name: "SalespersonId",
                table: "Order");
        }
    }
}

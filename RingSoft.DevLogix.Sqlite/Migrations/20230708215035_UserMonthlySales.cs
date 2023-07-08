using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RingSoft.DevLogix.Sqlite.Migrations
{
    /// <inheritdoc />
    public partial class UserMonthlySales : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TotalCost",
                table: "Customer",
                newName: "MinutesSpent");

            migrationBuilder.AddColumn<double>(
                name: "MonthlySalesQuota",
                table: "Users",
                type: "numeric",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "TotalSales",
                table: "Users",
                type: "numeric",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AlterColumn<int>(
                name: "SalespersonId",
                table: "Order",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "UserMonthlySales",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "integer", nullable: false),
                    MonthEnding = table.Column<DateTime>(type: "datetime", nullable: false),
                    Quota = table.Column<double>(type: "numeric", nullable: false),
                    TotalSales = table.Column<double>(type: "numeric", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserMonthlySales", x => new { x.UserId, x.MonthEnding });
                    table.ForeignKey(
                        name: "FK_UserMonthlySales_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserMonthlySales");

            migrationBuilder.DropColumn(
                name: "MonthlySalesQuota",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "TotalSales",
                table: "Users");

            migrationBuilder.RenameColumn(
                name: "MinutesSpent",
                table: "Customer",
                newName: "TotalCost");

            migrationBuilder.AlterColumn<int>(
                name: "SalespersonId",
                table: "Order",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");
        }
    }
}

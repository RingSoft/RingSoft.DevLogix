using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RingSoft.DevLogix.Sqlite.Migrations
{
    /// <inheritdoc />
    public partial class CustomerNulls : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<double>(
                name: "SupportMinutesSpent",
                table: "Customer",
                type: "numeric",
                nullable: false,
                defaultValue: 0.0,
                oldClrType: typeof(double),
                oldType: "numeric",
                oldNullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "SupportCost",
                table: "Customer",
                type: "numeric",
                nullable: false,
                defaultValue: 0.0,
                oldClrType: typeof(double),
                oldType: "numeric",
                oldNullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<double>(
                name: "SupportMinutesSpent",
                table: "Customer",
                type: "numeric",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "numeric");

            migrationBuilder.AlterColumn<double>(
                name: "SupportCost",
                table: "Customer",
                type: "numeric",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "numeric");
        }
    }
}

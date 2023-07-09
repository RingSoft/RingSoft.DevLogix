using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RingSoft.DevLogix.Sqlite.Migrations
{
    /// <inheritdoc />
    public partial class SupportTicketContactName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<double>(
                name: "MinutesSpent",
                table: "SupportTicket",
                type: "numeric",
                nullable: false,
                defaultValue: 0.0,
                oldClrType: typeof(double),
                oldType: "numeric",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ContactName",
                table: "SupportTicket",
                type: "nvarchar",
                maxLength: 50,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ContactName",
                table: "SupportTicket");

            migrationBuilder.AlterColumn<double>(
                name: "MinutesSpent",
                table: "SupportTicket",
                type: "numeric",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "numeric");
        }
    }
}

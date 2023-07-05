using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RingSoft.DevLogix.Sqlite.Migrations
{
    /// <inheritdoc />
    public partial class SupportTicketCost : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "CustomerMinutesSpent",
                table: "Users",
                type: "numeric",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "SupportTicketsMinutesSpent",
                table: "Users",
                type: "numeric",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<int>(
                name: "SupportTicketId",
                table: "TimeClocks",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_TimeClocks_SupportTicketId",
                table: "TimeClocks",
                column: "SupportTicketId");

            migrationBuilder.AddForeignKey(
                name: "FK_TimeClocks_SupportTicket_SupportTicketId",
                table: "TimeClocks",
                column: "SupportTicketId",
                principalTable: "SupportTicket",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TimeClocks_SupportTicket_SupportTicketId",
                table: "TimeClocks");

            migrationBuilder.DropIndex(
                name: "IX_TimeClocks_SupportTicketId",
                table: "TimeClocks");

            migrationBuilder.DropColumn(
                name: "CustomerMinutesSpent",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "SupportTicketsMinutesSpent",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "SupportTicketId",
                table: "TimeClocks");
        }
    }
}

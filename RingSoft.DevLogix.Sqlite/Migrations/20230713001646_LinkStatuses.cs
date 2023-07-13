using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RingSoft.DevLogix.Sqlite.Migrations
{
    /// <inheritdoc />
    public partial class LinkStatuses : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "StatusId",
                table: "SupportTicket",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "StatusId",
                table: "Customer",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_SupportTicket_StatusId",
                table: "SupportTicket",
                column: "StatusId");

            migrationBuilder.CreateIndex(
                name: "IX_Customer_StatusId",
                table: "Customer",
                column: "StatusId");

            migrationBuilder.AddForeignKey(
                name: "FK_Customer_CustomerStatus_StatusId",
                table: "Customer",
                column: "StatusId",
                principalTable: "CustomerStatus",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_SupportTicket_SupportTicketStatus_StatusId",
                table: "SupportTicket",
                column: "StatusId",
                principalTable: "SupportTicketStatus",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Customer_CustomerStatus_StatusId",
                table: "Customer");

            migrationBuilder.DropForeignKey(
                name: "FK_SupportTicket_SupportTicketStatus_StatusId",
                table: "SupportTicket");

            migrationBuilder.DropIndex(
                name: "IX_SupportTicket_StatusId",
                table: "SupportTicket");

            migrationBuilder.DropIndex(
                name: "IX_Customer_StatusId",
                table: "Customer");

            migrationBuilder.DropColumn(
                name: "StatusId",
                table: "SupportTicket");

            migrationBuilder.DropColumn(
                name: "StatusId",
                table: "Customer");
        }
    }
}

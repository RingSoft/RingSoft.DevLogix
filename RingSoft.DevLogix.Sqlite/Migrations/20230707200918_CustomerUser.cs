using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RingSoft.DevLogix.Sqlite.Migrations
{
    /// <inheritdoc />
    public partial class CustomerUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CustomerUser",
                columns: table => new
                {
                    CustomerId = table.Column<int>(type: "integer", nullable: false),
                    UserId = table.Column<int>(type: "integer", nullable: false),
                    MinutesSpent = table.Column<double>(type: "numeric", nullable: false),
                    Cost = table.Column<double>(type: "numeric", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomerUser", x => new { x.CustomerId, x.UserId });
                    table.ForeignKey(
                        name: "FK_CustomerUser_Customer_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customer",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CustomerUser_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CustomerUser_UserId",
                table: "CustomerUser",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CustomerUser");
        }
    }
}

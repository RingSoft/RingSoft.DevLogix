using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RingSoft.DevLogix.Sqlite.Migrations
{
    /// <inheritdoc />
    public partial class TerritorySales : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SalespersonId",
                table: "Territory",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Territory_SalespersonId",
                table: "Territory",
                column: "SalespersonId");

            migrationBuilder.AddForeignKey(
                name: "FK_Territory_Users_SalespersonId",
                table: "Territory",
                column: "SalespersonId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Territory_Users_SalespersonId",
                table: "Territory");

            migrationBuilder.DropIndex(
                name: "IX_Territory_SalespersonId",
                table: "Territory");

            migrationBuilder.DropColumn(
                name: "SalespersonId",
                table: "Territory");
        }
    }
}

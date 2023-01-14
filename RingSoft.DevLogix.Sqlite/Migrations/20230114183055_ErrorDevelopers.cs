using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RingSoft.DevLogix.Sqlite.Migrations
{
    /// <inheritdoc />
    public partial class ErrorDevelopers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Errors_Users_FixedByByUserId",
                table: "Errors");

            migrationBuilder.DropIndex(
                name: "IX_Errors_FixedByByUserId",
                table: "Errors");

            migrationBuilder.DropColumn(
                name: "FixedByByUserId",
                table: "Errors");

            migrationBuilder.CreateTable(
                name: "ErrorDevelopers",
                columns: table => new
                {
                    ErrorId = table.Column<int>(type: "integer", nullable: false),
                    DeveloperId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ErrorDevelopers", x => new { x.ErrorId, x.DeveloperId });
                    table.ForeignKey(
                        name: "FK_ErrorDevelopers_Errors_ErrorId",
                        column: x => x.ErrorId,
                        principalTable: "Errors",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ErrorDevelopers_Users_DeveloperId",
                        column: x => x.DeveloperId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_ErrorDevelopers_DeveloperId",
                table: "ErrorDevelopers",
                column: "DeveloperId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ErrorDevelopers");

            migrationBuilder.AddColumn<int>(
                name: "FixedByByUserId",
                table: "Errors",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Errors_FixedByByUserId",
                table: "Errors",
                column: "FixedByByUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Errors_Users_FixedByByUserId",
                table: "Errors",
                column: "FixedByByUserId",
                principalTable: "Users",
                principalColumn: "Id");
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RingSoft.DevLogix.Sqlite.Migrations
{
    /// <inheritdoc />
    public partial class CostModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "MinutesSpent",
                table: "TestingOutlines",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<double>(
                name: "TotalCost",
                table: "TestingOutlines",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.CreateTable(
                name: "TestingOutlineCosts",
                columns: table => new
                {
                    TestingOutlineId = table.Column<int>(type: "integer", nullable: false),
                    UserId = table.Column<int>(type: "integer", nullable: false),
                    TimeSpent = table.Column<double>(type: "numeric", nullable: false),
                    Cost = table.Column<double>(type: "numeric", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TestingOutlineCosts", x => new { x.TestingOutlineId, x.UserId });
                    table.ForeignKey(
                        name: "FK_TestingOutlineCosts_TestingOutlines_TestingOutlineId",
                        column: x => x.TestingOutlineId,
                        principalTable: "TestingOutlines",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TestingOutlineCosts_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TestingOutlineCosts_UserId",
                table: "TestingOutlineCosts",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TestingOutlineCosts");

            migrationBuilder.DropColumn(
                name: "MinutesSpent",
                table: "TestingOutlines");

            migrationBuilder.DropColumn(
                name: "TotalCost",
                table: "TestingOutlines");
        }
    }
}

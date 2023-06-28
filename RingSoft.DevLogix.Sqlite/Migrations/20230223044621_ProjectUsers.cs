using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RingSoft.DevLogix.Sqlite.Migrations
{
    /// <inheritdoc />
    public partial class ProjectUsers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ProjectUsers",
                columns: table => new
                {
                    ProjectId = table.Column<int>(type: "integer", nullable: false),
                    UserId = table.Column<int>(type: "integer", nullable: false),
                    MinutesSpent = table.Column<double>(type: "numeric", nullable: false),
                    Cost = table.Column<double>(type: "numeric", nullable: false),
                    IsStandard = table.Column<bool>(type: "bit", nullable: false),
                    SundayHours = table.Column<double>(type: "numeric", nullable: true),
                    MondayHours = table.Column<double>(type: "numeric", nullable: true),
                    TuesdayHours = table.Column<double>(type: "numeric", nullable: true),
                    WednesdayHours = table.Column<double>(type: "numeric", nullable: true),
                    ThursdayHours = table.Column<double>(type: "numeric", nullable: true),
                    FridayHours = table.Column<double>(type: "numeric", nullable: true),
                    SaturdayHours = table.Column<double>(type: "numeric", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectUsers", x => new { x.ProjectId, x.UserId });
                    table.ForeignKey(
                        name: "FK_ProjectUsers_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProjectUsers_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProjectUsers_UserId",
                table: "ProjectUsers",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProjectUsers");
        }
    }
}

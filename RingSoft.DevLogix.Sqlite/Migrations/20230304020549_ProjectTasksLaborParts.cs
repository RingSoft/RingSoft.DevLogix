using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RingSoft.DevLogix.Sqlite.Migrations
{
    /// <inheritdoc />
    public partial class ProjectTasksLaborParts : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ProjectTaskLaborParts",
                columns: table => new
                {
                    ProjectTaskId = table.Column<int>(type: "integer", nullable: false),
                    DetailId = table.Column<int>(type: "integer", nullable: false),
                    LaborPartId = table.Column<int>(type: "integer", nullable: true),
                    Description = table.Column<string>(type: "nvarchar", maxLength: 50, nullable: true),
                    Quantity = table.Column<int>(type: "integer", nullable: true),
                    MinutesCost = table.Column<decimal>(type: "numeric", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectTaskLaborParts", x => new { x.ProjectTaskId, x.DetailId });
                    table.ForeignKey(
                        name: "FK_ProjectTaskLaborParts_LaborParts_LaborPartId",
                        column: x => x.LaborPartId,
                        principalTable: "LaborParts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProjectTaskLaborParts_ProjectTasks_ProjectTaskId",
                        column: x => x.ProjectTaskId,
                        principalTable: "ProjectTasks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProjectTaskLaborParts_LaborPartId",
                table: "ProjectTaskLaborParts",
                column: "LaborPartId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProjectTaskLaborParts");
        }
    }
}

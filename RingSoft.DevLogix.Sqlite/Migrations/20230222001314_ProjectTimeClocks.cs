using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RingSoft.DevLogix.Sqlite.Migrations
{
    /// <inheritdoc />
    public partial class ProjectTimeClocks : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ProjectId",
                table: "TimeClocks",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_TimeClocks_ProjectId",
                table: "TimeClocks",
                column: "ProjectId");

            migrationBuilder.AddForeignKey(
                name: "FK_TimeClocks_Projects_ProjectId",
                table: "TimeClocks",
                column: "ProjectId",
                principalTable: "Projects",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TimeClocks_Projects_ProjectId",
                table: "TimeClocks");

            migrationBuilder.DropIndex(
                name: "IX_TimeClocks_ProjectId",
                table: "TimeClocks");

            migrationBuilder.DropColumn(
                name: "ProjectId",
                table: "TimeClocks");
        }
    }
}

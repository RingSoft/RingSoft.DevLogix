using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RingSoft.DevLogix.Sqlite.Migrations
{
    /// <inheritdoc />
    public partial class TimeClockProjectTask : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TimeClocks_Projects_ProjectId",
                table: "TimeClocks");

            migrationBuilder.RenameColumn(
                name: "ProjectId",
                table: "TimeClocks",
                newName: "ProjectTaskId");

            migrationBuilder.RenameIndex(
                name: "IX_TimeClocks_ProjectId",
                table: "TimeClocks",
                newName: "IX_TimeClocks_ProjectTaskId");

            migrationBuilder.AddForeignKey(
                name: "FK_TimeClocks_ProjectTasks_ProjectTaskId",
                table: "TimeClocks",
                column: "ProjectTaskId",
                principalTable: "ProjectTasks",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TimeClocks_ProjectTasks_ProjectTaskId",
                table: "TimeClocks");

            migrationBuilder.RenameColumn(
                name: "ProjectTaskId",
                table: "TimeClocks",
                newName: "ProjectId");

            migrationBuilder.RenameIndex(
                name: "IX_TimeClocks_ProjectTaskId",
                table: "TimeClocks",
                newName: "IX_TimeClocks_ProjectId");

            migrationBuilder.AddForeignKey(
                name: "FK_TimeClocks_Projects_ProjectId",
                table: "TimeClocks",
                column: "ProjectId",
                principalTable: "Projects",
                principalColumn: "Id");
        }
    }
}

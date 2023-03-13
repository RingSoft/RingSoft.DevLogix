using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RingSoft.DevLogix.SqlServer.Migrations
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

            migrationBuilder.AlterColumn<decimal>(
                name: "PercentWidth",
                table: "AdvancedFindColumns",
                type: "numeric(38,17)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(18,0)");

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

            migrationBuilder.AlterColumn<decimal>(
                name: "PercentWidth",
                table: "AdvancedFindColumns",
                type: "numeric(18,0)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(38,17)");

            migrationBuilder.AddForeignKey(
                name: "FK_TimeClocks_Projects_ProjectId",
                table: "TimeClocks",
                column: "ProjectId",
                principalTable: "Projects",
                principalColumn: "Id");
        }
    }
}

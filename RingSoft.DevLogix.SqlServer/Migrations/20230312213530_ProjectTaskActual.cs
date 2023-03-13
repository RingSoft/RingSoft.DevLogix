using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RingSoft.DevLogix.SqlServer.Migrations
{
    /// <inheritdoc />
    public partial class ProjectTaskActual : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "Cost",
                table: "ProjectTasks",
                type: "numeric(18,0)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "MinutesSpent",
                table: "ProjectTasks",
                type: "numeric(18,0)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AlterColumn<decimal>(
                name: "PercentWidth",
                table: "AdvancedFindColumns",
                type: "numeric(38,17)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(18,0)");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Cost",
                table: "ProjectTasks");

            migrationBuilder.DropColumn(
                name: "MinutesSpent",
                table: "ProjectTasks");

            migrationBuilder.AlterColumn<decimal>(
                name: "PercentWidth",
                table: "AdvancedFindColumns",
                type: "numeric(18,0)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(38,17)");
        }
    }
}

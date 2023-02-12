using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RingSoft.DevLogix.Sqlite.Migrations
{
    /// <inheritdoc />
    public partial class Chart : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DevLogixCharts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "nvarchar", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DevLogixCharts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DevLogixChartsBars",
                columns: table => new
                {
                    ChartId = table.Column<int>(type: "integer", nullable: false),
                    BarId = table.Column<int>(type: "integer", nullable: false),
                    Name = table.Column<string>(type: "nvarchar", maxLength: 50, nullable: false),
                    AdvancedFindId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DevLogixChartsBars", x => new { x.ChartId, x.BarId });
                    table.ForeignKey(
                        name: "FK_DevLogixChartsBars_DevLogixCharts_ChartId",
                        column: x => x.ChartId,
                        principalTable: "DevLogixCharts",
                        principalColumn: "Id");
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DevLogixChartsBars");

            migrationBuilder.DropTable(
                name: "DevLogixCharts");
        }
    }
}

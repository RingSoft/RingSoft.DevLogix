using Microsoft.EntityFrameworkCore.Migrations;

namespace RingSoft.DevLogix.SqlServer.Migrations
{
    public partial class LookupPath : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Path",
                table: "AdvancedFindFilters",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Path",
                table: "AdvancedFindColumns",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Path",
                table: "AdvancedFindFilters");

            migrationBuilder.DropColumn(
                name: "Path",
                table: "AdvancedFindColumns");
        }
    }
}

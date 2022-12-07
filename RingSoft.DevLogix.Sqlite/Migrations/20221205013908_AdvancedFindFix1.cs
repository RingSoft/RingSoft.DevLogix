using Microsoft.EntityFrameworkCore.Migrations;

namespace RingSoft.DevLogix.Sqlite.Migrations
{
    public partial class AdvancedFindFix1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Path",
                table: "AdvancedFindColumns");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Path",
                table: "AdvancedFindColumns",
                type: "nvarchar",
                maxLength: 1000,
                nullable: true);
        }
    }
}

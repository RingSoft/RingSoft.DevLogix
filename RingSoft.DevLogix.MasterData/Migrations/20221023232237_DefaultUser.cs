using Microsoft.EntityFrameworkCore.Migrations;

namespace RingSoft.DevLogix.MasterData.Migrations
{
    public partial class DefaultUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DefaultUser",
                table: "Organizations",
                type: "integer",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DefaultUser",
                table: "Organizations");
        }
    }
}

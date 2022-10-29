using Microsoft.EntityFrameworkCore.Migrations;

namespace RingSoft.DevLogix.MasterData.Migrations
{
    public partial class MigrateDb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "MigrateDb",
                table: "Organizations",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MigrateDb",
                table: "Organizations");
        }
    }
}

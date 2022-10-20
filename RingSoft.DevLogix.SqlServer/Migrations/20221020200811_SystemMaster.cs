using Microsoft.EntityFrameworkCore.Migrations;

namespace RingSoft.DevLogix.SqlServer.Migrations
{
    public partial class SystemMaster : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "FormulaDisplayValue",
                table: "AdvancedFindFilters",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar",
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "SystemMaster",
                columns: table => new
                {
                    OrganizationName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SystemMaster", x => x.OrganizationName);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SystemMaster");

            migrationBuilder.AlterColumn<string>(
                name: "FormulaDisplayValue",
                table: "AdvancedFindFilters",
                type: "nvarchar",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50,
                oldNullable: true);
        }
    }
}

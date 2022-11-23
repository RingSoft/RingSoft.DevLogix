using Microsoft.EntityFrameworkCore.Migrations;

namespace RingSoft.DevLogix.Sqlite.Migrations
{
    public partial class ErrorStatus1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_ErrorStatus",
                table: "ErrorStatus");

            migrationBuilder.RenameTable(
                name: "ErrorStatus",
                newName: "ErrorStatuses");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ErrorStatuses",
                table: "ErrorStatuses",
                column: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_ErrorStatuses",
                table: "ErrorStatuses");

            migrationBuilder.RenameTable(
                name: "ErrorStatuses",
                newName: "ErrorStatus");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ErrorStatus",
                table: "ErrorStatus",
                column: "Id");
        }
    }
}

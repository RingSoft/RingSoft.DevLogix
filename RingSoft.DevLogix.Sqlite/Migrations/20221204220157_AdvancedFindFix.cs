using Microsoft.EntityFrameworkCore.Migrations;

namespace RingSoft.DevLogix.Sqlite.Migrations
{
    public partial class AdvancedFindFix : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AdvancedFindColumns_AdvancedFinds_AdvancedFindId",
                table: "AdvancedFindColumns");

            migrationBuilder.AlterColumn<string>(
                name: "PrimaryFieldName",
                table: "AdvancedFindColumns",
                type: "nvarchar",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Path",
                table: "AdvancedFindColumns",
                type: "nvarchar",
                maxLength: 1000,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldMaxLength: 1000,
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_AdvancedFindColumns_AdvancedFinds_AdvancedFindId",
                table: "AdvancedFindColumns",
                column: "AdvancedFindId",
                principalTable: "AdvancedFinds",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AdvancedFindColumns_AdvancedFinds_AdvancedFindId",
                table: "AdvancedFindColumns");

            migrationBuilder.AlterColumn<string>(
                name: "PrimaryFieldName",
                table: "AdvancedFindColumns",
                type: "TEXT",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar",
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Path",
                table: "AdvancedFindColumns",
                type: "TEXT",
                maxLength: 1000,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar",
                oldMaxLength: 1000,
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_AdvancedFindColumns_AdvancedFinds_AdvancedFindId",
                table: "AdvancedFindColumns",
                column: "AdvancedFindId",
                principalTable: "AdvancedFinds",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

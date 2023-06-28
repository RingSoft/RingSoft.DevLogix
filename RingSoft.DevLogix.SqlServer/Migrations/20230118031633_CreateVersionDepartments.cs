using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RingSoft.DevLogix.SqlServer.Migrations
{
    /// <inheritdoc />
    public partial class CreateVersionDepartments : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ArchiveDepartmentId",
                table: "Products",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CreateDepartmentId",
                table: "Products",
                type: "integer",
                nullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "PercentWidth",
                table: "AdvancedFindColumns",
                type: "numeric(38,17)",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "numeric(18,0)");

            migrationBuilder.CreateIndex(
                name: "IX_Products_ArchiveDepartmentId",
                table: "Products",
                column: "ArchiveDepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_CreateDepartmentId",
                table: "Products",
                column: "CreateDepartmentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Departments_ArchiveDepartmentId",
                table: "Products",
                column: "ArchiveDepartmentId",
                principalTable: "Departments",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Departments_CreateDepartmentId",
                table: "Products",
                column: "CreateDepartmentId",
                principalTable: "Departments",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_Departments_ArchiveDepartmentId",
                table: "Products");

            migrationBuilder.DropForeignKey(
                name: "FK_Products_Departments_CreateDepartmentId",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_Products_ArchiveDepartmentId",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_Products_CreateDepartmentId",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "ArchiveDepartmentId",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "CreateDepartmentId",
                table: "Products");

            migrationBuilder.AlterColumn<double>(
                name: "PercentWidth",
                table: "AdvancedFindColumns",
                type: "numeric(18,0)",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "numeric(38,17)");
        }
    }
}

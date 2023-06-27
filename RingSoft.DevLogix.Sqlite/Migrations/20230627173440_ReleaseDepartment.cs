using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RingSoft.DevLogix.Sqlite.Migrations
{
    /// <inheritdoc />
    public partial class ReleaseDepartment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DepartmentId",
                table: "ProductVersions",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "VersionDate",
                table: "ProductVersions",
                type: "datetime",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ReleaseLevel",
                table: "Departments",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_ProductVersions_DepartmentId",
                table: "ProductVersions",
                column: "DepartmentId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductVersions_Departments_DepartmentId",
                table: "ProductVersions",
                column: "DepartmentId",
                principalTable: "Departments",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductVersions_Departments_DepartmentId",
                table: "ProductVersions");

            migrationBuilder.DropIndex(
                name: "IX_ProductVersions_DepartmentId",
                table: "ProductVersions");

            migrationBuilder.DropColumn(
                name: "DepartmentId",
                table: "ProductVersions");

            migrationBuilder.DropColumn(
                name: "VersionDate",
                table: "ProductVersions");

            migrationBuilder.DropColumn(
                name: "ReleaseLevel",
                table: "Departments");
        }
    }
}

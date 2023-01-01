using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RingSoft.DevLogix.SqlServer.Migrations
{
    /// <inheritdoc />
    public partial class Deployment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ArchivePath",
                table: "Products",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "InstallerFileName",
                table: "Products",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "FtpAddress",
                table: "Departments",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FtpPassword",
                table: "Departments",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FtpUsername",
                table: "Departments",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Error",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ErrorId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ErrorDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ErrorStatusId = table.Column<int>(type: "integer", nullable: false),
                    ProductId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Error", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Error_ErrorStatuses_ErrorStatusId",
                        column: x => x.ErrorStatusId,
                        principalTable: "ErrorStatuses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Error_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Error_ErrorStatusId",
                table: "Error",
                column: "ErrorStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_Error_ProductId",
                table: "Error",
                column: "ProductId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Error");

            migrationBuilder.DropColumn(
                name: "ArchivePath",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "InstallerFileName",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "FtpAddress",
                table: "Departments");

            migrationBuilder.DropColumn(
                name: "FtpPassword",
                table: "Departments");

            migrationBuilder.DropColumn(
                name: "FtpUsername",
                table: "Departments");
        }
    }
}

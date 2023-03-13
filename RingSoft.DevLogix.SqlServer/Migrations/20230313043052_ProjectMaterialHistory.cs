using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RingSoft.DevLogix.SqlServer.Migrations
{
    /// <inheritdoc />
    public partial class ProjectMaterialHistory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "PercentWidth",
                table: "AdvancedFindColumns",
                type: "numeric(38,17)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(18,0)");

            migrationBuilder.CreateTable(
                name: "ProjectMaterialHistory",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProjectMaterialId = table.Column<int>(type: "integer", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime", nullable: false),
                    Quantity = table.Column<decimal>(type: "numeric(18,0)", nullable: false),
                    Cost = table.Column<decimal>(type: "numeric(18,0)", nullable: false),
                    UserId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectMaterialHistory", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProjectMaterialHistory_ProjectMaterials_ProjectMaterialId",
                        column: x => x.ProjectMaterialId,
                        principalTable: "ProjectMaterials",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProjectMaterialHistory_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProjectMaterialHistory_ProjectMaterialId",
                table: "ProjectMaterialHistory",
                column: "ProjectMaterialId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectMaterialHistory_UserId",
                table: "ProjectMaterialHistory",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProjectMaterialHistory");

            migrationBuilder.AlterColumn<decimal>(
                name: "PercentWidth",
                table: "AdvancedFindColumns",
                type: "numeric(18,0)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(38,17)");
        }
    }
}

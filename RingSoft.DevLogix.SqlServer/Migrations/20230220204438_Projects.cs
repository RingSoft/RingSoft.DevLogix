using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RingSoft.DevLogix.SqlServer.Migrations
{
    /// <inheritdoc />
    public partial class Projects : Migration
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
                name: "Projects",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Deadline = table.Column<DateTime>(type: "datetime", nullable: false),
                    OriginalDeadline = table.Column<DateTime>(type: "datetime", nullable: false),
                    ProductId = table.Column<int>(type: "integer", nullable: true),
                    SundayHours = table.Column<decimal>(type: "numeric(18,0)", nullable: true),
                    MondayHours = table.Column<decimal>(type: "numeric(18,0)", nullable: true),
                    TuesdayHours = table.Column<decimal>(type: "numeric(18,0)", nullable: true),
                    WednesdayHours = table.Column<decimal>(type: "numeric(18,0)", nullable: true),
                    ThursdayHours = table.Column<decimal>(type: "numeric(18,0)", nullable: true),
                    FridayHours = table.Column<decimal>(type: "numeric(18,0)", nullable: true),
                    SaturdayHours = table.Column<decimal>(type: "numeric(18,0)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Projects", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Projects_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Projects_ProductId",
                table: "Projects",
                column: "ProductId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Projects");

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

using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RingSoft.DevLogix.Sqlite.Migrations
{
    /// <inheritdoc />
    public partial class Projects : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Projects",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "nvarchar", maxLength: 50, nullable: false),
                    Deadline = table.Column<DateTime>(type: "datetime", nullable: false),
                    OriginalDeadline = table.Column<DateTime>(type: "datetime", nullable: false),
                    ProductId = table.Column<int>(type: "integer", nullable: true),
                    SundayHours = table.Column<double>(type: "numeric", nullable: true),
                    MondayHours = table.Column<double>(type: "numeric", nullable: true),
                    TuesdayHours = table.Column<double>(type: "numeric", nullable: true),
                    WednesdayHours = table.Column<double>(type: "numeric", nullable: true),
                    ThursdayHours = table.Column<double>(type: "numeric", nullable: true),
                    FridayHours = table.Column<double>(type: "numeric", nullable: true),
                    SaturdayHours = table.Column<double>(type: "numeric", nullable: true)
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
        }
    }
}

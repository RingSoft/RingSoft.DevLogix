using Microsoft.EntityFrameworkCore.Migrations;

namespace RingSoft.DevLogix.SqlServer.Migrations
{
    public partial class ProductVersions : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AdvancedFindColumns_AdvancedFinds_AdvancedFindId",
                table: "AdvancedFindColumns");

            migrationBuilder.CreateTable(
                name: "ProductVersions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Description = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ProductId = table.Column<int>(type: "integer", nullable: false),
                    Notes = table.Column<string>(type: "ntext", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductVersions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductVersions_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProductVersions_ProductId",
                table: "ProductVersions",
                column: "ProductId");

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

            migrationBuilder.DropTable(
                name: "ProductVersions");

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

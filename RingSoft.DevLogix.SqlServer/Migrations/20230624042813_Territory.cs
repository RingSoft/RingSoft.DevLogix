using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RingSoft.DevLogix.SqlServer.Migrations
{
    /// <inheritdoc />
    public partial class Territory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SalesCost",
                table: "Customer");

            migrationBuilder.DropColumn(
                name: "SalesMinutesSpent",
                table: "Customer");

            migrationBuilder.AddColumn<int>(
                name: "TerritoryId",
                table: "Customer",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<double>(
                name: "PercentWidth",
                table: "AdvancedFindColumns",
                type: "numeric(38,17)",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "numeric(18,0)");

            migrationBuilder.CreateTable(
                name: "Territory",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Territory", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Customer_TerritoryId",
                table: "Customer",
                column: "TerritoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Customer_Territory_TerritoryId",
                table: "Customer",
                column: "TerritoryId",
                principalTable: "Territory",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Customer_Territory_TerritoryId",
                table: "Customer");

            migrationBuilder.DropTable(
                name: "Territory");

            migrationBuilder.DropIndex(
                name: "IX_Customer_TerritoryId",
                table: "Customer");

            migrationBuilder.DropColumn(
                name: "TerritoryId",
                table: "Customer");

            migrationBuilder.AddColumn<double>(
                name: "SalesCost",
                table: "Customer",
                type: "numeric(18,0)",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "SalesMinutesSpent",
                table: "Customer",
                type: "numeric(18,0)",
                nullable: true);

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

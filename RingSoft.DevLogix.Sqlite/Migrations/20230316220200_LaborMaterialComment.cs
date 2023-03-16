using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RingSoft.DevLogix.Sqlite.Migrations
{
    /// <inheritdoc />
    public partial class LaborMaterialComment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Comment",
                table: "MaterialParts",
                type: "ntext",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Comment",
                table: "LaborParts",
                type: "ntext",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Comment",
                table: "MaterialParts");

            migrationBuilder.DropColumn(
                name: "Comment",
                table: "LaborParts");
        }
    }
}

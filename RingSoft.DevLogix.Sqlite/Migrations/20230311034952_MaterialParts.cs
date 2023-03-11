using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RingSoft.DevLogix.Sqlite.Migrations
{
    /// <inheritdoc />
    public partial class MaterialParts : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MaterialParts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "nvarchar", maxLength: 50, nullable: false),
                    Cost = table.Column<decimal>(type: "numeric", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MaterialParts", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MaterialParts");
        }
    }
}

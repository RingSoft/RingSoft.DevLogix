using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RingSoft.DevLogix.Sqlite.Migrations
{
    /// <inheritdoc />
    public partial class ProjectMaterialParts : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ProjectMaterialParts",
                columns: table => new
                {
                    ProjectMaterialId = table.Column<int>(type: "integer", nullable: false),
                    DetailId = table.Column<int>(type: "integer", nullable: false),
                    LineType = table.Column<byte>(type: "smallint", nullable: false),
                    RowId = table.Column<string>(type: "nvarchar", maxLength: 50, nullable: false),
                    ParentRowId = table.Column<string>(type: "nvarchar", maxLength: 50, nullable: true),
                    CommentCrLf = table.Column<bool>(type: "bit", nullable: true),
                    MaterialPartId = table.Column<int>(type: "integer", nullable: true),
                    Description = table.Column<string>(type: "nvarchar", maxLength: 50, nullable: true),
                    Quantity = table.Column<int>(type: "integer", nullable: true),
                    Cost = table.Column<double>(type: "numeric", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectMaterialParts", x => new { x.ProjectMaterialId, x.DetailId });
                    table.ForeignKey(
                        name: "FK_ProjectMaterialParts_MaterialParts_MaterialPartId",
                        column: x => x.MaterialPartId,
                        principalTable: "MaterialParts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProjectMaterialParts_ProjectMaterials_ProjectMaterialId",
                        column: x => x.ProjectMaterialId,
                        principalTable: "ProjectMaterials",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProjectMaterialParts_MaterialPartId",
                table: "ProjectMaterialParts",
                column: "MaterialPartId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProjectMaterialParts");
        }
    }
}

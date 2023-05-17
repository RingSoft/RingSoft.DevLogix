using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RingSoft.DevLogix.Sqlite.Migrations
{
    /// <inheritdoc />
    public partial class TestingOutlineDetails : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TestingOutlineDetails",
                columns: table => new
                {
                    TestingOutlineId = table.Column<int>(type: "integer", nullable: false),
                    DetailId = table.Column<int>(type: "integer", nullable: false),
                    Step = table.Column<string>(type: "nvarchar", maxLength: 50, nullable: false),
                    IsComplete = table.Column<bool>(type: "bit", nullable: false),
                    CompletedVersionId = table.Column<int>(type: "integer", nullable: true),
                    TestingTemplateId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TestingOutlineDetails", x => new { x.TestingOutlineId, x.DetailId });
                    table.ForeignKey(
                        name: "FK_TestingOutlineDetails_ProductVersions_CompletedVersionId",
                        column: x => x.CompletedVersionId,
                        principalTable: "ProductVersions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TestingOutlineDetails_TestingOutlines_TestingOutlineId",
                        column: x => x.TestingOutlineId,
                        principalTable: "TestingOutlines",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TestingOutlineDetails_TestingTemplates_TestingTemplateId",
                        column: x => x.TestingTemplateId,
                        principalTable: "TestingTemplates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TestingOutlineDetails_CompletedVersionId",
                table: "TestingOutlineDetails",
                column: "CompletedVersionId");

            migrationBuilder.CreateIndex(
                name: "IX_TestingOutlineDetails_TestingTemplateId",
                table: "TestingOutlineDetails",
                column: "TestingTemplateId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TestingOutlineDetails");
        }
    }
}

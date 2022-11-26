using Microsoft.EntityFrameworkCore.Migrations;

namespace RingSoft.DevLogix.Sqlite.Migrations
{
    public partial class Departments : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Departments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Description = table.Column<string>(type: "nvarchar", maxLength: 50, nullable: false),
                    ErrorFixStatusId = table.Column<int>(type: "integer", nullable: false),
                    ErrorPassStatusId = table.Column<int>(type: "integer", nullable: false),
                    ErrorFailStatusId = table.Column<int>(type: "integer", nullable: false),
                    FixText = table.Column<string>(type: "nvarchar", maxLength: 50, nullable: true),
                    PassText = table.Column<string>(type: "nvarchar", maxLength: 50, nullable: false),
                    FailText = table.Column<string>(type: "nvarchar", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Departments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Departments_ErrorStatuses_ErrorFailStatusId",
                        column: x => x.ErrorFailStatusId,
                        principalTable: "ErrorStatuses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Departments_ErrorStatuses_ErrorFixStatusId",
                        column: x => x.ErrorFixStatusId,
                        principalTable: "ErrorStatuses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Departments_ErrorStatuses_ErrorPassStatusId",
                        column: x => x.ErrorPassStatusId,
                        principalTable: "ErrorStatuses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Departments_ErrorFailStatusId",
                table: "Departments",
                column: "ErrorFailStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_Departments_ErrorFixStatusId",
                table: "Departments",
                column: "ErrorFixStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_Departments_ErrorPassStatusId",
                table: "Departments",
                column: "ErrorPassStatusId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Departments");
        }
    }
}

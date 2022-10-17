using Microsoft.EntityFrameworkCore.Migrations;

namespace RingSoft.DevLogix.SqlServer.Migrations
{
    public partial class Startup : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AdvancedFinds",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Table = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    FromFormula = table.Column<string>(type: "ntext", nullable: true),
                    RefreshRate = table.Column<byte>(type: "tinyint", nullable: true),
                    RefreshValue = table.Column<int>(type: "integer", nullable: true),
                    RefreshCondition = table.Column<byte>(type: "tinyint", nullable: true),
                    YellowAlert = table.Column<int>(type: "integer", nullable: true),
                    RedAlert = table.Column<int>(type: "integer", nullable: true),
                    Disabled = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AdvancedFinds", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AdvancedFindColumns",
                columns: table => new
                {
                    AdvancedFindId = table.Column<int>(type: "integer", nullable: false),
                    ColumnId = table.Column<int>(type: "integer", nullable: false),
                    TableName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    FieldName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    PrimaryTableName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    PrimaryFieldName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Caption = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    PercentWidth = table.Column<decimal>(type: "numeric(38,17)", nullable: false),
                    Formula = table.Column<string>(type: "ntext", nullable: true),
                    FieldDataType = table.Column<byte>(type: "tinyint", nullable: false),
                    DecimalFormatType = table.Column<byte>(type: "tinyint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AdvancedFindColumns", x => new { x.AdvancedFindId, x.ColumnId });
                    table.ForeignKey(
                        name: "FK_AdvancedFindColumns_AdvancedFinds_AdvancedFindId",
                        column: x => x.AdvancedFindId,
                        principalTable: "AdvancedFinds",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AdvancedFindFilters",
                columns: table => new
                {
                    AdvancedFindId = table.Column<int>(type: "integer", nullable: false),
                    FilterId = table.Column<int>(type: "integer", nullable: false),
                    LeftParentheses = table.Column<byte>(type: "tinyint", nullable: false),
                    TableName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    FieldName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    PrimaryTableName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    PrimaryFieldName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Operand = table.Column<byte>(type: "tinyint", nullable: false),
                    SearchForValue = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Formula = table.Column<string>(type: "ntext", nullable: true),
                    FormulaDataType = table.Column<byte>(type: "tinyint", nullable: false),
                    FormulaDisplayValue = table.Column<string>(type: "nvarchar", nullable: true),
                    SearchForAdvancedFindId = table.Column<int>(type: "integer", nullable: true),
                    CustomDate = table.Column<bool>(type: "bit", nullable: false),
                    RightParentheses = table.Column<byte>(type: "tinyint", nullable: false),
                    EndLogic = table.Column<byte>(type: "tinyint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AdvancedFindFilters", x => new { x.AdvancedFindId, x.FilterId });
                    table.ForeignKey(
                        name: "FK_AdvancedFindFilters_AdvancedFinds_AdvancedFindId",
                        column: x => x.AdvancedFindId,
                        principalTable: "AdvancedFinds",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AdvancedFindFilters_AdvancedFinds_SearchForAdvancedFindId",
                        column: x => x.SearchForAdvancedFindId,
                        principalTable: "AdvancedFinds",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_AdvancedFindFilters_SearchForAdvancedFindId",
                table: "AdvancedFindFilters",
                column: "SearchForAdvancedFindId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AdvancedFindColumns");

            migrationBuilder.DropTable(
                name: "AdvancedFindFilters");

            migrationBuilder.DropTable(
                name: "AdvancedFinds");
        }
    }
}

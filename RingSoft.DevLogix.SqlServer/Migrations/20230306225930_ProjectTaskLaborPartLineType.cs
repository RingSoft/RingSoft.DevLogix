using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RingSoft.DevLogix.SqlServer.Migrations
{
    /// <inheritdoc />
    public partial class ProjectTaskLaborPartLineType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "CommentCrLf",
                table: "ProjectTaskLaborParts",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<byte>(
                name: "LineType",
                table: "ProjectTaskLaborParts",
                type: "tinyint",
                nullable: false,
                defaultValue: (byte)0);

            migrationBuilder.AddColumn<string>(
                name: "ParentRowId",
                table: "ProjectTaskLaborParts",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RowId",
                table: "ProjectTaskLaborParts",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<double>(
                name: "PercentWidth",
                table: "AdvancedFindColumns",
                type: "numeric(38,17)",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "numeric(18,0)");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CommentCrLf",
                table: "ProjectTaskLaborParts");

            migrationBuilder.DropColumn(
                name: "LineType",
                table: "ProjectTaskLaborParts");

            migrationBuilder.DropColumn(
                name: "ParentRowId",
                table: "ProjectTaskLaborParts");

            migrationBuilder.DropColumn(
                name: "RowId",
                table: "ProjectTaskLaborParts");

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

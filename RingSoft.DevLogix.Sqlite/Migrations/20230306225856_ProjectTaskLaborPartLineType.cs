using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RingSoft.DevLogix.Sqlite.Migrations
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
                type: "smallint",
                nullable: false,
                defaultValue: (byte)0);

            migrationBuilder.AddColumn<string>(
                name: "ParentRowId",
                table: "ProjectTaskLaborParts",
                type: "nvarchar",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RowId",
                table: "ProjectTaskLaborParts",
                type: "nvarchar",
                maxLength: 50,
                nullable: false,
                defaultValue: "");
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
        }
    }
}

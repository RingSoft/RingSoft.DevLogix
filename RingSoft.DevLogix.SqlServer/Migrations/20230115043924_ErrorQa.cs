using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RingSoft.DevLogix.SqlServer.Migrations
{
    /// <inheritdoc />
    public partial class ErrorQa : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_ErrorDevelopers",
                table: "ErrorDevelopers");

            migrationBuilder.DropColumn(
                name: "FixedDate",
                table: "Errors");

            migrationBuilder.DropColumn(
                name: "PassedDate",
                table: "Errors");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "ErrorDevelopers",
                type: "integer",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<DateTime>(
                name: "DateFixed",
                table: "ErrorDevelopers",
                type: "datetime",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AlterColumn<double>(
                name: "PercentWidth",
                table: "AdvancedFindColumns",
                type: "numeric(38,17)",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "numeric(18,0)");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ErrorDevelopers",
                table: "ErrorDevelopers",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "ErrorTesters",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ErrorId = table.Column<int>(type: "integer", nullable: false),
                    TesterId = table.Column<int>(type: "integer", nullable: false),
                    NewStatusId = table.Column<int>(type: "integer", nullable: false),
                    DateChanged = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ErrorTesters", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ErrorTesters_ErrorStatuses_NewStatusId",
                        column: x => x.NewStatusId,
                        principalTable: "ErrorStatuses",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ErrorTesters_Errors_ErrorId",
                        column: x => x.ErrorId,
                        principalTable: "Errors",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ErrorTesters_Users_TesterId",
                        column: x => x.TesterId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_ErrorDevelopers_ErrorId",
                table: "ErrorDevelopers",
                column: "ErrorId");

            migrationBuilder.CreateIndex(
                name: "IX_ErrorTesters_ErrorId",
                table: "ErrorTesters",
                column: "ErrorId");

            migrationBuilder.CreateIndex(
                name: "IX_ErrorTesters_NewStatusId",
                table: "ErrorTesters",
                column: "NewStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_ErrorTesters_TesterId",
                table: "ErrorTesters",
                column: "TesterId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ErrorTesters");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ErrorDevelopers",
                table: "ErrorDevelopers");

            migrationBuilder.DropIndex(
                name: "IX_ErrorDevelopers_ErrorId",
                table: "ErrorDevelopers");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "ErrorDevelopers");

            migrationBuilder.DropColumn(
                name: "DateFixed",
                table: "ErrorDevelopers");

            migrationBuilder.AddColumn<DateTime>(
                name: "FixedDate",
                table: "Errors",
                type: "datetime",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "PassedDate",
                table: "Errors",
                type: "datetime",
                nullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "PercentWidth",
                table: "AdvancedFindColumns",
                type: "numeric(18,0)",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "numeric(38,17)");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ErrorDevelopers",
                table: "ErrorDevelopers",
                columns: new[] { "ErrorId", "DeveloperId" });
        }
    }
}

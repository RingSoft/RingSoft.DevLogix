using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RingSoft.DevLogix.SqlServer.Migrations
{
    /// <inheritdoc />
    public partial class Errors : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Error_ErrorStatuses_ErrorStatusId",
                table: "Error");

            migrationBuilder.DropForeignKey(
                name: "FK_Error_Products_ProductId",
                table: "Error");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Error",
                table: "Error");

            migrationBuilder.RenameTable(
                name: "Error",
                newName: "Errors");

            migrationBuilder.RenameIndex(
                name: "IX_Error_ProductId",
                table: "Errors",
                newName: "IX_Errors_ProductId");

            migrationBuilder.RenameIndex(
                name: "IX_Error_ErrorStatusId",
                table: "Errors",
                newName: "IX_Errors_ErrorStatusId");

            migrationBuilder.AlterColumn<decimal>(
                name: "PercentWidth",
                table: "AdvancedFindColumns",
                type: "numeric(38,17)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(18,0)");

            migrationBuilder.AlterColumn<string>(
                name: "ErrorId",
                table: "Errors",
                type: "nvarchar",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<DateTime>(
                name: "ErrorDate",
                table: "Errors",
                type: "datetime",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "Errors",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .Annotation("SqlServer:Identity", "1, 1")
                .OldAnnotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<int>(
                name: "AssignedDeveloperId",
                table: "Errors",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "AssignedTesterId",
                table: "Errors",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Errors",
                type: "ntext",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "ErrorPriorityId",
                table: "Errors",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "FixedDate",
                table: "Errors",
                type: "datetime",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "FixedVersionId",
                table: "Errors",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "FoundVersionId",
                table: "Errors",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "PassedDate",
                table: "Errors",
                type: "datetime",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Resolution",
                table: "Errors",
                type: "ntext",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Errors",
                table: "Errors",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Errors_AssignedDeveloperId",
                table: "Errors",
                column: "AssignedDeveloperId");

            migrationBuilder.CreateIndex(
                name: "IX_Errors_AssignedTesterId",
                table: "Errors",
                column: "AssignedTesterId");

            migrationBuilder.CreateIndex(
                name: "IX_Errors_ErrorPriorityId",
                table: "Errors",
                column: "ErrorPriorityId");

            migrationBuilder.CreateIndex(
                name: "IX_Errors_FixedVersionId",
                table: "Errors",
                column: "FixedVersionId");

            migrationBuilder.CreateIndex(
                name: "IX_Errors_FoundVersionId",
                table: "Errors",
                column: "FoundVersionId");

            migrationBuilder.AddForeignKey(
                name: "FK_Errors_ErrorPriorities_ErrorPriorityId",
                table: "Errors",
                column: "ErrorPriorityId",
                principalTable: "ErrorPriorities",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Errors_ErrorStatuses_ErrorStatusId",
                table: "Errors",
                column: "ErrorStatusId",
                principalTable: "ErrorStatuses",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Errors_ProductVersions_FixedVersionId",
                table: "Errors",
                column: "FixedVersionId",
                principalTable: "ProductVersions",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Errors_ProductVersions_FoundVersionId",
                table: "Errors",
                column: "FoundVersionId",
                principalTable: "ProductVersions",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Errors_Products_ProductId",
                table: "Errors",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Errors_Users_AssignedDeveloperId",
                table: "Errors",
                column: "AssignedDeveloperId",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Errors_Users_AssignedTesterId",
                table: "Errors",
                column: "AssignedTesterId",
                principalTable: "Users",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Errors_ErrorPriorities_ErrorPriorityId",
                table: "Errors");

            migrationBuilder.DropForeignKey(
                name: "FK_Errors_ErrorStatuses_ErrorStatusId",
                table: "Errors");

            migrationBuilder.DropForeignKey(
                name: "FK_Errors_ProductVersions_FixedVersionId",
                table: "Errors");

            migrationBuilder.DropForeignKey(
                name: "FK_Errors_ProductVersions_FoundVersionId",
                table: "Errors");

            migrationBuilder.DropForeignKey(
                name: "FK_Errors_Products_ProductId",
                table: "Errors");

            migrationBuilder.DropForeignKey(
                name: "FK_Errors_Users_AssignedDeveloperId",
                table: "Errors");

            migrationBuilder.DropForeignKey(
                name: "FK_Errors_Users_AssignedTesterId",
                table: "Errors");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Errors",
                table: "Errors");

            migrationBuilder.DropIndex(
                name: "IX_Errors_AssignedDeveloperId",
                table: "Errors");

            migrationBuilder.DropIndex(
                name: "IX_Errors_AssignedTesterId",
                table: "Errors");

            migrationBuilder.DropIndex(
                name: "IX_Errors_ErrorPriorityId",
                table: "Errors");

            migrationBuilder.DropIndex(
                name: "IX_Errors_FixedVersionId",
                table: "Errors");

            migrationBuilder.DropIndex(
                name: "IX_Errors_FoundVersionId",
                table: "Errors");

            migrationBuilder.DropColumn(
                name: "AssignedDeveloperId",
                table: "Errors");

            migrationBuilder.DropColumn(
                name: "AssignedTesterId",
                table: "Errors");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "Errors");

            migrationBuilder.DropColumn(
                name: "ErrorPriorityId",
                table: "Errors");

            migrationBuilder.DropColumn(
                name: "FixedDate",
                table: "Errors");

            migrationBuilder.DropColumn(
                name: "FixedVersionId",
                table: "Errors");

            migrationBuilder.DropColumn(
                name: "FoundVersionId",
                table: "Errors");

            migrationBuilder.DropColumn(
                name: "PassedDate",
                table: "Errors");

            migrationBuilder.DropColumn(
                name: "Resolution",
                table: "Errors");

            migrationBuilder.RenameTable(
                name: "Errors",
                newName: "Error");

            migrationBuilder.RenameIndex(
                name: "IX_Errors_ProductId",
                table: "Error",
                newName: "IX_Error_ProductId");

            migrationBuilder.RenameIndex(
                name: "IX_Errors_ErrorStatusId",
                table: "Error",
                newName: "IX_Error_ErrorStatusId");

            migrationBuilder.AlterColumn<decimal>(
                name: "PercentWidth",
                table: "AdvancedFindColumns",
                type: "numeric(18,0)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(38,17)");

            migrationBuilder.AlterColumn<string>(
                name: "ErrorId",
                table: "Error",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar");

            migrationBuilder.AlterColumn<DateTime>(
                name: "ErrorDate",
                table: "Error",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "Error",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .Annotation("SqlServer:Identity", "1, 1")
                .OldAnnotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Error",
                table: "Error",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Error_ErrorStatuses_ErrorStatusId",
                table: "Error",
                column: "ErrorStatusId",
                principalTable: "ErrorStatuses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Error_Products_ProductId",
                table: "Error",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

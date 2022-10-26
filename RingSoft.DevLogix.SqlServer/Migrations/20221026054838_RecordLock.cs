using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace RingSoft.DevLogix.SqlServer.Migrations
{
    public partial class RecordLock : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UsersGroup_Group_GroupId",
                table: "UsersGroup");

            migrationBuilder.DropForeignKey(
                name: "FK_UsersGroup_Users_UserId",
                table: "UsersGroup");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UsersGroup",
                table: "UsersGroup");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Group",
                table: "Group");

            migrationBuilder.RenameTable(
                name: "UsersGroup",
                newName: "UsersGroups");

            migrationBuilder.RenameTable(
                name: "Group",
                newName: "Groups");

            migrationBuilder.RenameIndex(
                name: "IX_UsersGroup_GroupId",
                table: "UsersGroups",
                newName: "IX_UsersGroups_GroupId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UsersGroups",
                table: "UsersGroups",
                columns: new[] { "UserId", "GroupId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_Groups",
                table: "Groups",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "RecordLocks",
                columns: table => new
                {
                    Table = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    PrimaryKey = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    LockDateTime = table.Column<DateTime>(type: "datetime", nullable: false),
                    User = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RecordLocks", x => new { x.Table, x.PrimaryKey });
                });

            migrationBuilder.AddForeignKey(
                name: "FK_UsersGroups_Groups_GroupId",
                table: "UsersGroups",
                column: "GroupId",
                principalTable: "Groups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_UsersGroups_Users_UserId",
                table: "UsersGroups",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UsersGroups_Groups_GroupId",
                table: "UsersGroups");

            migrationBuilder.DropForeignKey(
                name: "FK_UsersGroups_Users_UserId",
                table: "UsersGroups");

            migrationBuilder.DropTable(
                name: "RecordLocks");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UsersGroups",
                table: "UsersGroups");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Groups",
                table: "Groups");

            migrationBuilder.RenameTable(
                name: "UsersGroups",
                newName: "UsersGroup");

            migrationBuilder.RenameTable(
                name: "Groups",
                newName: "Group");

            migrationBuilder.RenameIndex(
                name: "IX_UsersGroups_GroupId",
                table: "UsersGroup",
                newName: "IX_UsersGroup_GroupId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UsersGroup",
                table: "UsersGroup",
                columns: new[] { "UserId", "GroupId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_Group",
                table: "Group",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UsersGroup_Group_GroupId",
                table: "UsersGroup",
                column: "GroupId",
                principalTable: "Group",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_UsersGroup_Users_UserId",
                table: "UsersGroup",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}

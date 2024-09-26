using Microsoft.EntityFrameworkCore.Migrations;
using RingSoft.DevLogix.DataAccess;

#nullable disable

namespace RingSoft.DevLogix.Sqlite.Migrations
{
    /// <inheritdoc />
    public partial class AppGuid1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AppGuid",
                table: "SystemMaster",
                type: "TEXT",
                maxLength: 50,
                nullable: true);

            DataAccessGlobals.MigrateAppGuid(migrationBuilder);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AppGuid",
                table: "SystemMaster");
        }
    }
}

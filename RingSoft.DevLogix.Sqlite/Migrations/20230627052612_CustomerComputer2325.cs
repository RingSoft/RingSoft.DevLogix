using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RingSoft.DevLogix.Sqlite.Migrations
{
    /// <inheritdoc />
    public partial class CustomerComputer2325 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "DatabasePlatform",
                table: "CustomerComputer",
                type: "nvarchar",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(byte),
                oldType: "smallint",
                oldMaxLength: 50);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<byte>(
                name: "DatabasePlatform",
                table: "CustomerComputer",
                type: "smallint",
                maxLength: 50,
                nullable: false,
                defaultValue: (byte)0,
                oldClrType: typeof(string),
                oldType: "nvarchar",
                oldMaxLength: 50,
                oldNullable: true);
        }
    }
}

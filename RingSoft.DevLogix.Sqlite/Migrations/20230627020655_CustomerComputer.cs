using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RingSoft.DevLogix.Sqlite.Migrations
{
    /// <inheritdoc />
    public partial class CustomerComputer : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Notes",
                table: "Customer",
                type: "ntext",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar",
                oldNullable: true);

            migrationBuilder.AddColumn<double>(
                name: "MinutesCost",
                table: "Customer",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<double>(
                name: "TotalCost",
                table: "Customer",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<double>(
                name: "TotalSales",
                table: "Customer",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.CreateTable(
                name: "CustomerComputer",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "nvarchar", maxLength: 50, nullable: false),
                    CustomerId = table.Column<int>(type: "integer", nullable: false),
                    OperatingSystem = table.Column<string>(type: "nvarchar", maxLength: 50, nullable: true),
                    Speed = table.Column<double>(type: "numeric", nullable: true),
                    ScreenResolution = table.Column<string>(type: "nvarchar", maxLength: 50, nullable: true),
                    RamSize = table.Column<int>(type: "integer", nullable: true),
                    HardDriveSize = table.Column<int>(type: "integer", nullable: true),
                    HardDriveFree = table.Column<int>(type: "integer", nullable: true),
                    InternetSpeed = table.Column<int>(type: "integer", nullable: true),
                    DatabasePlatform = table.Column<byte>(type: "smallint", maxLength: 50, nullable: false),
                    Printer = table.Column<string>(type: "nvarchar", maxLength: 50, nullable: true),
                    Notes = table.Column<string>(type: "ntext", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomerComputer", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CustomerComputer_Customer_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customer",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CustomerComputer_CustomerId",
                table: "CustomerComputer",
                column: "CustomerId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CustomerComputer");

            migrationBuilder.DropColumn(
                name: "MinutesCost",
                table: "Customer");

            migrationBuilder.DropColumn(
                name: "TotalCost",
                table: "Customer");

            migrationBuilder.DropColumn(
                name: "TotalSales",
                table: "Customer");

            migrationBuilder.AlterColumn<string>(
                name: "Notes",
                table: "Customer",
                type: "nvarchar",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "ntext",
                oldNullable: true);
        }
    }
}

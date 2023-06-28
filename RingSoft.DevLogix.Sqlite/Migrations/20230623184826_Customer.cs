using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RingSoft.DevLogix.Sqlite.Migrations
{
    /// <inheritdoc />
    public partial class Customer : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TimeZone",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "nvarchar", maxLength: 50, nullable: false),
                    HourToGMT = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TimeZone", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Customer",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CompanyName = table.Column<string>(type: "nvarchar", maxLength: 50, nullable: false),
                    ContactName = table.Column<string>(type: "nvarchar", maxLength: 50, nullable: true),
                    ContactTitle = table.Column<string>(type: "nvarchar", maxLength: 50, nullable: true),
                    Address = table.Column<string>(type: "nvarchar", maxLength: 100, nullable: true),
                    City = table.Column<string>(type: "nvarchar", maxLength: 50, nullable: true),
                    Region = table.Column<string>(type: "nvarchar", maxLength: 50, nullable: true),
                    PostalCode = table.Column<string>(type: "nvarchar", maxLength: 50, nullable: true),
                    Country = table.Column<string>(type: "nvarchar", maxLength: 50, nullable: true),
                    Phone = table.Column<string>(type: "nvarchar", maxLength: 50, nullable: false),
                    TimeZoneId = table.Column<int>(type: "integer", nullable: false),
                    AssignedUserId = table.Column<int>(type: "integer", nullable: false),
                    SupportMinutesPurchased = table.Column<double>(type: "numeric", nullable: true),
                    SupportMinutesSpent = table.Column<double>(type: "numeric", nullable: true),
                    SupportCost = table.Column<double>(type: "numeric", nullable: true),
                    SalesMinutesSpent = table.Column<double>(type: "numeric", nullable: true),
                    SalesCost = table.Column<double>(type: "numeric", nullable: true),
                    Notes = table.Column<string>(type: "nvarchar", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customer", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Customer_TimeZone_TimeZoneId",
                        column: x => x.TimeZoneId,
                        principalTable: "TimeZone",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Customer_Users_AssignedUserId",
                        column: x => x.AssignedUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Customer_AssignedUserId",
                table: "Customer",
                column: "AssignedUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Customer_TimeZoneId",
                table: "Customer",
                column: "TimeZoneId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Customer");

            migrationBuilder.DropTable(
                name: "TimeZone");
        }
    }
}

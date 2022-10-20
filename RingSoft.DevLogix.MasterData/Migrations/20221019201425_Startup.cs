using Microsoft.EntityFrameworkCore.Migrations;

namespace RingSoft.DevLogix.MasterData.Migrations
{
    public partial class Startup : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Organization",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "nvarchar", maxLength: 50, nullable: false),
                    FilePath = table.Column<string>(type: "nvarchar", maxLength: 250, nullable: false),
                    FileName = table.Column<string>(type: "nvarchar", maxLength: 250, nullable: false),
                    IsDefault = table.Column<bool>(type: "bit", nullable: false),
                    Platform = table.Column<byte>(type: "smallint", nullable: false),
                    Server = table.Column<string>(type: "nvarchar", maxLength: 50, nullable: false),
                    Database = table.Column<string>(type: "nvarchar", maxLength: 50, nullable: false),
                    AuthenticationType = table.Column<byte>(type: "smallint", nullable: true),
                    Username = table.Column<string>(type: "nvarchar", maxLength: 50, nullable: false),
                    Password = table.Column<string>(type: "nvarchar", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Organization", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Organization");
        }
    }
}

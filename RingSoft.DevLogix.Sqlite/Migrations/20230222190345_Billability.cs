using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RingSoft.DevLogix.Sqlite.Migrations
{
    /// <inheritdoc />
    public partial class Billability : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "BillableProjectsMinutesSpent",
                table: "Users",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<double>(
                name: "ErrorsMinutesSpent",
                table: "Users",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<double>(
                name: "HourlyRate",
                table: "Users",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<double>(
                name: "NonBillableProjectsMinutesSpent",
                table: "Users",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<double>(
                name: "Cost",
                table: "Projects",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<bool>(
                name: "IsBillable",
                table: "Projects",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<double>(
                name: "MinutesSpent",
                table: "Projects",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<double>(
                name: "Cost",
                table: "Errors",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<double>(
                name: "MinutesSpent",
                table: "Errors",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BillableProjectsMinutesSpent",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "ErrorsMinutesSpent",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "HourlyRate",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "NonBillableProjectsMinutesSpent",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Cost",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "IsBillable",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "MinutesSpent",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "Cost",
                table: "Errors");

            migrationBuilder.DropColumn(
                name: "MinutesSpent",
                table: "Errors");
        }
    }
}

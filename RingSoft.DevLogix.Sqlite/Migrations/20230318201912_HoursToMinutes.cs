using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RingSoft.DevLogix.Sqlite.Migrations
{
    /// <inheritdoc />
    public partial class HoursToMinutes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "WednesdayHours",
                table: "ProjectUsers",
                newName: "WednesdayMinutes");

            migrationBuilder.RenameColumn(
                name: "TuesdayHours",
                table: "ProjectUsers",
                newName: "TuesdayMinutes");

            migrationBuilder.RenameColumn(
                name: "ThursdayHours",
                table: "ProjectUsers",
                newName: "ThursdayMinutes");

            migrationBuilder.RenameColumn(
                name: "SundayHours",
                table: "ProjectUsers",
                newName: "SundayMinutes");

            migrationBuilder.RenameColumn(
                name: "SaturdayHours",
                table: "ProjectUsers",
                newName: "SaturdayMinutes");

            migrationBuilder.RenameColumn(
                name: "MondayHours",
                table: "ProjectUsers",
                newName: "MondayMinutes");

            migrationBuilder.RenameColumn(
                name: "FridayHours",
                table: "ProjectUsers",
                newName: "FridayMinutes");

            migrationBuilder.RenameColumn(
                name: "WednesdayHours",
                table: "Projects",
                newName: "WednesdayMinutes");

            migrationBuilder.RenameColumn(
                name: "TuesdayHours",
                table: "Projects",
                newName: "TuesdayMinutes");

            migrationBuilder.RenameColumn(
                name: "ThursdayHours",
                table: "Projects",
                newName: "ThursdayMinutes");

            migrationBuilder.RenameColumn(
                name: "SundayHours",
                table: "Projects",
                newName: "SundayMinutes");

            migrationBuilder.RenameColumn(
                name: "SaturdayHours",
                table: "Projects",
                newName: "SaturdayMinutes");

            migrationBuilder.RenameColumn(
                name: "MondayHours",
                table: "Projects",
                newName: "MondayMinutes");

            migrationBuilder.RenameColumn(
                name: "FridayHours",
                table: "Projects",
                newName: "FridayMinutes");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "WednesdayMinutes",
                table: "ProjectUsers",
                newName: "WednesdayHours");

            migrationBuilder.RenameColumn(
                name: "TuesdayMinutes",
                table: "ProjectUsers",
                newName: "TuesdayHours");

            migrationBuilder.RenameColumn(
                name: "ThursdayMinutes",
                table: "ProjectUsers",
                newName: "ThursdayHours");

            migrationBuilder.RenameColumn(
                name: "SundayMinutes",
                table: "ProjectUsers",
                newName: "SundayHours");

            migrationBuilder.RenameColumn(
                name: "SaturdayMinutes",
                table: "ProjectUsers",
                newName: "SaturdayHours");

            migrationBuilder.RenameColumn(
                name: "MondayMinutes",
                table: "ProjectUsers",
                newName: "MondayHours");

            migrationBuilder.RenameColumn(
                name: "FridayMinutes",
                table: "ProjectUsers",
                newName: "FridayHours");

            migrationBuilder.RenameColumn(
                name: "WednesdayMinutes",
                table: "Projects",
                newName: "WednesdayHours");

            migrationBuilder.RenameColumn(
                name: "TuesdayMinutes",
                table: "Projects",
                newName: "TuesdayHours");

            migrationBuilder.RenameColumn(
                name: "ThursdayMinutes",
                table: "Projects",
                newName: "ThursdayHours");

            migrationBuilder.RenameColumn(
                name: "SundayMinutes",
                table: "Projects",
                newName: "SundayHours");

            migrationBuilder.RenameColumn(
                name: "SaturdayMinutes",
                table: "Projects",
                newName: "SaturdayHours");

            migrationBuilder.RenameColumn(
                name: "MondayMinutes",
                table: "Projects",
                newName: "MondayHours");

            migrationBuilder.RenameColumn(
                name: "FridayMinutes",
                table: "Projects",
                newName: "FridayHours");
        }
    }
}

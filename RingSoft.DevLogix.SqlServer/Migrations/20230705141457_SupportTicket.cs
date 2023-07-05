using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RingSoft.DevLogix.SqlServer.Migrations
{
    /// <inheritdoc />
    public partial class SupportTicket : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "YellowMinutes",
                table: "UserTracker",
                type: "numeric(38,17)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "numeric(18,0)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "RedMinutes",
                table: "UserTracker",
                type: "numeric(38,17)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "numeric(18,0)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "TestingOutlinesMinutesSpent",
                table: "Users",
                type: "numeric(38,17)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(18,0)");

            migrationBuilder.AlterColumn<decimal>(
                name: "NonBillableProjectsMinutesSpent",
                table: "Users",
                type: "numeric(38,17)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(18,0)");

            migrationBuilder.AlterColumn<decimal>(
                name: "HourlyRate",
                table: "Users",
                type: "numeric(38,17)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(18,0)");

            migrationBuilder.AlterColumn<decimal>(
                name: "ErrorsMinutesSpent",
                table: "Users",
                type: "numeric(38,17)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(18,0)");

            migrationBuilder.AlterColumn<decimal>(
                name: "BillableProjectsMinutesSpent",
                table: "Users",
                type: "numeric(38,17)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(18,0)");

            migrationBuilder.AlterColumn<decimal>(
                name: "MinutesSpent",
                table: "TimeClocks",
                type: "numeric(38,17)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "numeric(18,0)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "TotalCost",
                table: "TestingOutlines",
                type: "numeric(38,17)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(18,0)");

            migrationBuilder.AlterColumn<decimal>(
                name: "PercentComplete",
                table: "TestingOutlines",
                type: "numeric(38,17)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(18,0)");

            migrationBuilder.AlterColumn<decimal>(
                name: "MinutesSpent",
                table: "TestingOutlines",
                type: "numeric(38,17)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(18,0)");

            migrationBuilder.AlterColumn<decimal>(
                name: "TimeSpent",
                table: "TestingOutlineCosts",
                type: "numeric(38,17)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(18,0)");

            migrationBuilder.AlterColumn<decimal>(
                name: "Cost",
                table: "TestingOutlineCosts",
                type: "numeric(38,17)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(18,0)");

            migrationBuilder.AlterColumn<decimal>(
                name: "WednesdayMinutes",
                table: "ProjectUsers",
                type: "numeric(38,17)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "numeric(18,0)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "TuesdayMinutes",
                table: "ProjectUsers",
                type: "numeric(38,17)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "numeric(18,0)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "ThursdayMinutes",
                table: "ProjectUsers",
                type: "numeric(38,17)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "numeric(18,0)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "SundayMinutes",
                table: "ProjectUsers",
                type: "numeric(38,17)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "numeric(18,0)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "SaturdayMinutes",
                table: "ProjectUsers",
                type: "numeric(38,17)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "numeric(18,0)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "MondayMinutes",
                table: "ProjectUsers",
                type: "numeric(38,17)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "numeric(18,0)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "MinutesSpent",
                table: "ProjectUsers",
                type: "numeric(38,17)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(18,0)");

            migrationBuilder.AlterColumn<decimal>(
                name: "FridayMinutes",
                table: "ProjectUsers",
                type: "numeric(38,17)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "numeric(18,0)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "Cost",
                table: "ProjectUsers",
                type: "numeric(38,17)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(18,0)");

            migrationBuilder.AlterColumn<decimal>(
                name: "PercentComplete",
                table: "ProjectTasks",
                type: "numeric(38,17)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(18,0)");

            migrationBuilder.AlterColumn<decimal>(
                name: "MinutesSpent",
                table: "ProjectTasks",
                type: "numeric(38,17)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(18,0)");

            migrationBuilder.AlterColumn<decimal>(
                name: "MinutesCost",
                table: "ProjectTasks",
                type: "numeric(38,17)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(18,0)");

            migrationBuilder.AlterColumn<decimal>(
                name: "HourlyRate",
                table: "ProjectTasks",
                type: "numeric(38,17)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(18,0)");

            migrationBuilder.AlterColumn<decimal>(
                name: "EstimatedCost",
                table: "ProjectTasks",
                type: "numeric(38,17)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(18,0)");

            migrationBuilder.AlterColumn<decimal>(
                name: "Cost",
                table: "ProjectTasks",
                type: "numeric(38,17)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(18,0)");

            migrationBuilder.AlterColumn<decimal>(
                name: "Quantity",
                table: "ProjectTaskLaborParts",
                type: "numeric(38,17)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "numeric(18,0)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "PercentComplete",
                table: "ProjectTaskLaborParts",
                type: "numeric(38,17)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(18,0)");

            migrationBuilder.AlterColumn<decimal>(
                name: "MinutesCost",
                table: "ProjectTaskLaborParts",
                type: "numeric(38,17)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(18,0)");

            migrationBuilder.AlterColumn<decimal>(
                name: "WednesdayMinutes",
                table: "Projects",
                type: "numeric(38,17)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "numeric(18,0)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "TuesdayMinutes",
                table: "Projects",
                type: "numeric(38,17)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "numeric(18,0)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "ThursdayMinutes",
                table: "Projects",
                type: "numeric(38,17)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "numeric(18,0)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "SundayMinutes",
                table: "Projects",
                type: "numeric(38,17)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "numeric(18,0)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "SaturdayMinutes",
                table: "Projects",
                type: "numeric(38,17)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "numeric(18,0)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "MondayMinutes",
                table: "Projects",
                type: "numeric(38,17)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "numeric(18,0)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "MinutesSpent",
                table: "Projects",
                type: "numeric(38,17)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(18,0)");

            migrationBuilder.AlterColumn<decimal>(
                name: "FridayMinutes",
                table: "Projects",
                type: "numeric(38,17)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "numeric(18,0)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "EstimatedMinutes",
                table: "Projects",
                type: "numeric(38,17)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(18,0)");

            migrationBuilder.AlterColumn<decimal>(
                name: "EstimatedCost",
                table: "Projects",
                type: "numeric(38,17)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(18,0)");

            migrationBuilder.AlterColumn<decimal>(
                name: "Cost",
                table: "Projects",
                type: "numeric(38,17)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(18,0)");

            migrationBuilder.AlterColumn<decimal>(
                name: "ContractCost",
                table: "Projects",
                type: "numeric(38,17)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "numeric(18,0)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "Cost",
                table: "ProjectMaterials",
                type: "numeric(38,17)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(18,0)");

            migrationBuilder.AlterColumn<decimal>(
                name: "ActualCost",
                table: "ProjectMaterials",
                type: "numeric(38,17)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(18,0)");

            migrationBuilder.AlterColumn<decimal>(
                name: "Quantity",
                table: "ProjectMaterialParts",
                type: "numeric(38,17)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "numeric(18,0)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "Cost",
                table: "ProjectMaterialParts",
                type: "numeric(38,17)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "numeric(18,0)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "Quantity",
                table: "ProjectMaterialHistory",
                type: "numeric(38,17)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(18,0)");

            migrationBuilder.AlterColumn<decimal>(
                name: "Cost",
                table: "ProjectMaterialHistory",
                type: "numeric(38,17)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(18,0)");

            migrationBuilder.AlterColumn<decimal>(
                name: "Revenue",
                table: "Products",
                type: "numeric(38,17)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "numeric(18,0)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "Price",
                table: "Products",
                type: "numeric(38,17)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "numeric(18,0)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "Cost",
                table: "Products",
                type: "numeric(38,17)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "numeric(18,0)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "UnitPrice",
                table: "OrderDetail",
                type: "numeric(38,17)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(18,0)");

            migrationBuilder.AlterColumn<decimal>(
                name: "Quantity",
                table: "OrderDetail",
                type: "numeric(38,17)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(18,0)");

            migrationBuilder.AlterColumn<decimal>(
                name: "ExtendedPrice",
                table: "OrderDetail",
                type: "numeric(38,17)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(18,0)");

            migrationBuilder.AlterColumn<decimal>(
                name: "Discount",
                table: "OrderDetail",
                type: "numeric(38,17)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "numeric(18,0)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "TotalDiscount",
                table: "Order",
                type: "numeric(38,17)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "numeric(18,0)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "Total",
                table: "Order",
                type: "numeric(38,17)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "numeric(18,0)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "SubTotal",
                table: "Order",
                type: "numeric(38,17)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "numeric(18,0)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "Freight",
                table: "Order",
                type: "numeric(38,17)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "numeric(18,0)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "Cost",
                table: "MaterialParts",
                type: "numeric(38,17)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(18,0)");

            migrationBuilder.AlterColumn<decimal>(
                name: "MinutesCost",
                table: "LaborParts",
                type: "numeric(38,17)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(18,0)");

            migrationBuilder.AlterColumn<decimal>(
                name: "MinutesSpent",
                table: "ErrorUsers",
                type: "numeric(38,17)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(18,0)");

            migrationBuilder.AlterColumn<decimal>(
                name: "Cost",
                table: "ErrorUsers",
                type: "numeric(38,17)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(18,0)");

            migrationBuilder.AlterColumn<decimal>(
                name: "MinutesSpent",
                table: "Errors",
                type: "numeric(38,17)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(18,0)");

            migrationBuilder.AlterColumn<decimal>(
                name: "Cost",
                table: "Errors",
                type: "numeric(38,17)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(18,0)");

            migrationBuilder.AlterColumn<decimal>(
                name: "Speed",
                table: "CustomerComputer",
                type: "numeric(38,17)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "numeric(18,0)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "TotalSales",
                table: "Customer",
                type: "numeric(38,17)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(18,0)");

            migrationBuilder.AlterColumn<decimal>(
                name: "TotalCost",
                table: "Customer",
                type: "numeric(38,17)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(18,0)");

            migrationBuilder.AlterColumn<decimal>(
                name: "SupportMinutesSpent",
                table: "Customer",
                type: "numeric(38,17)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "numeric(18,0)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "SupportMinutesPurchased",
                table: "Customer",
                type: "numeric(38,17)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "numeric(18,0)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "SupportCost",
                table: "Customer",
                type: "numeric(38,17)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "numeric(18,0)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "MinutesCost",
                table: "Customer",
                type: "numeric(38,17)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(18,0)");

            migrationBuilder.AlterColumn<decimal>(
                name: "PercentWidth",
                table: "AdvancedFindColumns",
                type: "numeric(38,17)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(18,0)");

            migrationBuilder.CreateTable(
                name: "SupportTicket",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TicketId = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    CloseDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    CustomerId = table.Column<int>(type: "integer", nullable: false),
                    CreateUserId = table.Column<int>(type: "integer", nullable: false),
                    ProductId = table.Column<int>(type: "integer", nullable: false),
                    AssignedToUserId = table.Column<int>(type: "integer", nullable: true),
                    Notes = table.Column<string>(type: "ntext", nullable: true),
                    MinutesSpent = table.Column<decimal>(type: "numeric(38,17)", nullable: true),
                    Cost = table.Column<decimal>(type: "numeric(38,17)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SupportTicket", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SupportTicket_Customer_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customer",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SupportTicket_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SupportTicket_Users_AssignedToUserId",
                        column: x => x.AssignedToUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SupportTicket_Users_CreateUserId",
                        column: x => x.CreateUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SupportTicket_AssignedToUserId",
                table: "SupportTicket",
                column: "AssignedToUserId");

            migrationBuilder.CreateIndex(
                name: "IX_SupportTicket_CreateUserId",
                table: "SupportTicket",
                column: "CreateUserId");

            migrationBuilder.CreateIndex(
                name: "IX_SupportTicket_CustomerId",
                table: "SupportTicket",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_SupportTicket_ProductId",
                table: "SupportTicket",
                column: "ProductId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SupportTicket");

            migrationBuilder.AlterColumn<decimal>(
                name: "YellowMinutes",
                table: "UserTracker",
                type: "numeric(18,0)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "numeric(38,17)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "RedMinutes",
                table: "UserTracker",
                type: "numeric(18,0)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "numeric(38,17)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "TestingOutlinesMinutesSpent",
                table: "Users",
                type: "numeric(18,0)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(38,17)");

            migrationBuilder.AlterColumn<decimal>(
                name: "NonBillableProjectsMinutesSpent",
                table: "Users",
                type: "numeric(18,0)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(38,17)");

            migrationBuilder.AlterColumn<decimal>(
                name: "HourlyRate",
                table: "Users",
                type: "numeric(18,0)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(38,17)");

            migrationBuilder.AlterColumn<decimal>(
                name: "ErrorsMinutesSpent",
                table: "Users",
                type: "numeric(18,0)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(38,17)");

            migrationBuilder.AlterColumn<decimal>(
                name: "BillableProjectsMinutesSpent",
                table: "Users",
                type: "numeric(18,0)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(38,17)");

            migrationBuilder.AlterColumn<decimal>(
                name: "MinutesSpent",
                table: "TimeClocks",
                type: "numeric(18,0)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "numeric(38,17)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "TotalCost",
                table: "TestingOutlines",
                type: "numeric(18,0)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(38,17)");

            migrationBuilder.AlterColumn<decimal>(
                name: "PercentComplete",
                table: "TestingOutlines",
                type: "numeric(18,0)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(38,17)");

            migrationBuilder.AlterColumn<decimal>(
                name: "MinutesSpent",
                table: "TestingOutlines",
                type: "numeric(18,0)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(38,17)");

            migrationBuilder.AlterColumn<decimal>(
                name: "TimeSpent",
                table: "TestingOutlineCosts",
                type: "numeric(18,0)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(38,17)");

            migrationBuilder.AlterColumn<decimal>(
                name: "Cost",
                table: "TestingOutlineCosts",
                type: "numeric(18,0)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(38,17)");

            migrationBuilder.AlterColumn<decimal>(
                name: "WednesdayMinutes",
                table: "ProjectUsers",
                type: "numeric(18,0)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "numeric(38,17)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "TuesdayMinutes",
                table: "ProjectUsers",
                type: "numeric(18,0)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "numeric(38,17)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "ThursdayMinutes",
                table: "ProjectUsers",
                type: "numeric(18,0)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "numeric(38,17)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "SundayMinutes",
                table: "ProjectUsers",
                type: "numeric(18,0)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "numeric(38,17)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "SaturdayMinutes",
                table: "ProjectUsers",
                type: "numeric(18,0)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "numeric(38,17)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "MondayMinutes",
                table: "ProjectUsers",
                type: "numeric(18,0)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "numeric(38,17)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "MinutesSpent",
                table: "ProjectUsers",
                type: "numeric(18,0)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(38,17)");

            migrationBuilder.AlterColumn<decimal>(
                name: "FridayMinutes",
                table: "ProjectUsers",
                type: "numeric(18,0)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "numeric(38,17)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "Cost",
                table: "ProjectUsers",
                type: "numeric(18,0)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(38,17)");

            migrationBuilder.AlterColumn<decimal>(
                name: "PercentComplete",
                table: "ProjectTasks",
                type: "numeric(18,0)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(38,17)");

            migrationBuilder.AlterColumn<decimal>(
                name: "MinutesSpent",
                table: "ProjectTasks",
                type: "numeric(18,0)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(38,17)");

            migrationBuilder.AlterColumn<decimal>(
                name: "MinutesCost",
                table: "ProjectTasks",
                type: "numeric(18,0)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(38,17)");

            migrationBuilder.AlterColumn<decimal>(
                name: "HourlyRate",
                table: "ProjectTasks",
                type: "numeric(18,0)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(38,17)");

            migrationBuilder.AlterColumn<decimal>(
                name: "EstimatedCost",
                table: "ProjectTasks",
                type: "numeric(18,0)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(38,17)");

            migrationBuilder.AlterColumn<decimal>(
                name: "Cost",
                table: "ProjectTasks",
                type: "numeric(18,0)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(38,17)");

            migrationBuilder.AlterColumn<decimal>(
                name: "Quantity",
                table: "ProjectTaskLaborParts",
                type: "numeric(18,0)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "numeric(38,17)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "PercentComplete",
                table: "ProjectTaskLaborParts",
                type: "numeric(18,0)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(38,17)");

            migrationBuilder.AlterColumn<decimal>(
                name: "MinutesCost",
                table: "ProjectTaskLaborParts",
                type: "numeric(18,0)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(38,17)");

            migrationBuilder.AlterColumn<decimal>(
                name: "WednesdayMinutes",
                table: "Projects",
                type: "numeric(18,0)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "numeric(38,17)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "TuesdayMinutes",
                table: "Projects",
                type: "numeric(18,0)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "numeric(38,17)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "ThursdayMinutes",
                table: "Projects",
                type: "numeric(18,0)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "numeric(38,17)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "SundayMinutes",
                table: "Projects",
                type: "numeric(18,0)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "numeric(38,17)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "SaturdayMinutes",
                table: "Projects",
                type: "numeric(18,0)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "numeric(38,17)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "MondayMinutes",
                table: "Projects",
                type: "numeric(18,0)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "numeric(38,17)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "MinutesSpent",
                table: "Projects",
                type: "numeric(18,0)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(38,17)");

            migrationBuilder.AlterColumn<decimal>(
                name: "FridayMinutes",
                table: "Projects",
                type: "numeric(18,0)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "numeric(38,17)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "EstimatedMinutes",
                table: "Projects",
                type: "numeric(18,0)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(38,17)");

            migrationBuilder.AlterColumn<decimal>(
                name: "EstimatedCost",
                table: "Projects",
                type: "numeric(18,0)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(38,17)");

            migrationBuilder.AlterColumn<decimal>(
                name: "Cost",
                table: "Projects",
                type: "numeric(18,0)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(38,17)");

            migrationBuilder.AlterColumn<decimal>(
                name: "ContractCost",
                table: "Projects",
                type: "numeric(18,0)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "numeric(38,17)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "Cost",
                table: "ProjectMaterials",
                type: "numeric(18,0)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(38,17)");

            migrationBuilder.AlterColumn<decimal>(
                name: "ActualCost",
                table: "ProjectMaterials",
                type: "numeric(18,0)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(38,17)");

            migrationBuilder.AlterColumn<decimal>(
                name: "Quantity",
                table: "ProjectMaterialParts",
                type: "numeric(18,0)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "numeric(38,17)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "Cost",
                table: "ProjectMaterialParts",
                type: "numeric(18,0)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "numeric(38,17)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "Quantity",
                table: "ProjectMaterialHistory",
                type: "numeric(18,0)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(38,17)");

            migrationBuilder.AlterColumn<decimal>(
                name: "Cost",
                table: "ProjectMaterialHistory",
                type: "numeric(18,0)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(38,17)");

            migrationBuilder.AlterColumn<decimal>(
                name: "Revenue",
                table: "Products",
                type: "numeric(18,0)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "numeric(38,17)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "Price",
                table: "Products",
                type: "numeric(18,0)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "numeric(38,17)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "Cost",
                table: "Products",
                type: "numeric(18,0)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "numeric(38,17)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "UnitPrice",
                table: "OrderDetail",
                type: "numeric(18,0)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(38,17)");

            migrationBuilder.AlterColumn<decimal>(
                name: "Quantity",
                table: "OrderDetail",
                type: "numeric(18,0)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(38,17)");

            migrationBuilder.AlterColumn<decimal>(
                name: "ExtendedPrice",
                table: "OrderDetail",
                type: "numeric(18,0)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(38,17)");

            migrationBuilder.AlterColumn<decimal>(
                name: "Discount",
                table: "OrderDetail",
                type: "numeric(18,0)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "numeric(38,17)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "TotalDiscount",
                table: "Order",
                type: "numeric(18,0)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "numeric(38,17)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "Total",
                table: "Order",
                type: "numeric(18,0)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "numeric(38,17)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "SubTotal",
                table: "Order",
                type: "numeric(18,0)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "numeric(38,17)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "Freight",
                table: "Order",
                type: "numeric(18,0)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "numeric(38,17)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "Cost",
                table: "MaterialParts",
                type: "numeric(18,0)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(38,17)");

            migrationBuilder.AlterColumn<decimal>(
                name: "MinutesCost",
                table: "LaborParts",
                type: "numeric(18,0)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(38,17)");

            migrationBuilder.AlterColumn<decimal>(
                name: "MinutesSpent",
                table: "ErrorUsers",
                type: "numeric(18,0)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(38,17)");

            migrationBuilder.AlterColumn<decimal>(
                name: "Cost",
                table: "ErrorUsers",
                type: "numeric(18,0)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(38,17)");

            migrationBuilder.AlterColumn<decimal>(
                name: "MinutesSpent",
                table: "Errors",
                type: "numeric(18,0)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(38,17)");

            migrationBuilder.AlterColumn<decimal>(
                name: "Cost",
                table: "Errors",
                type: "numeric(18,0)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(38,17)");

            migrationBuilder.AlterColumn<decimal>(
                name: "Speed",
                table: "CustomerComputer",
                type: "numeric(18,0)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "numeric(38,17)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "TotalSales",
                table: "Customer",
                type: "numeric(18,0)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(38,17)");

            migrationBuilder.AlterColumn<decimal>(
                name: "TotalCost",
                table: "Customer",
                type: "numeric(18,0)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(38,17)");

            migrationBuilder.AlterColumn<decimal>(
                name: "SupportMinutesSpent",
                table: "Customer",
                type: "numeric(18,0)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "numeric(38,17)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "SupportMinutesPurchased",
                table: "Customer",
                type: "numeric(18,0)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "numeric(38,17)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "SupportCost",
                table: "Customer",
                type: "numeric(18,0)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "numeric(38,17)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "MinutesCost",
                table: "Customer",
                type: "numeric(18,0)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(38,17)");

            migrationBuilder.AlterColumn<decimal>(
                name: "PercentWidth",
                table: "AdvancedFindColumns",
                type: "numeric(18,0)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(38,17)");
        }
    }
}

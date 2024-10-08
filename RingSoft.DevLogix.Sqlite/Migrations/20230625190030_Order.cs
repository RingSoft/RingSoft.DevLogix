﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RingSoft.DevLogix.Sqlite.Migrations
{
    /// <inheritdoc />
    public partial class Order : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Order",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    OrderId = table.Column<string>(type: "nvarchar", maxLength: 50, nullable: false),
                    CustomerId = table.Column<int>(type: "integer", nullable: false),
                    OrderDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    ShippedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    CompanyName = table.Column<string>(type: "nvarchar", maxLength: 50, nullable: false),
                    ContactName = table.Column<string>(type: "nvarchar", maxLength: 50, nullable: true),
                    ContactTitle = table.Column<string>(type: "nvarchar", maxLength: 50, nullable: true),
                    Address = table.Column<string>(type: "nvarchar", maxLength: 100, nullable: true),
                    City = table.Column<string>(type: "nvarchar", maxLength: 50, nullable: true),
                    Region = table.Column<string>(type: "nvarchar", maxLength: 50, nullable: true),
                    PostalCode = table.Column<string>(type: "nvarchar", maxLength: 50, nullable: true),
                    Country = table.Column<string>(type: "nvarchar", maxLength: 50, nullable: true),
                    SubTotal = table.Column<double>(type: "numeric", nullable: true),
                    TotalDiscount = table.Column<double>(type: "numeric", nullable: true),
                    Freight = table.Column<double>(type: "numeric", nullable: true),
                    Total = table.Column<double>(type: "numeric", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Order", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Order_Customer_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customer",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Order_CustomerId",
                table: "Order",
                column: "CustomerId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Order");
        }
    }
}

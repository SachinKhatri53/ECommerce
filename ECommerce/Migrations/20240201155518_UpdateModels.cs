using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ECommerce.Migrations
{
    /// <inheritdoc />
    public partial class UpdateModels : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProductId",
                table: "Order");

            migrationBuilder.DropColumn(
                name: "Quantity",
                table: "Order");

            migrationBuilder.AddColumn<string>(
                name: "City",
                table: "Order",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Country",
                table: "Order",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "OrderDate",
                table: "Order",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "ShippingAddress",
                table: "Order",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ShippingName",
                table: "Order",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "State",
                table: "Order",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "Order",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ZipCode",
                table: "Order",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "OrderItem",
                columns: table => new
                {
                    OrderItemId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    OrdersOrderId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderItem", x => x.OrderItemId);
                    table.ForeignKey(
                        name: "FK_OrderItem_Order_OrdersOrderId",
                        column: x => x.OrdersOrderId,
                        principalTable: "Order",
                        principalColumn: "OrderId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_OrderItem_OrdersOrderId",
                table: "OrderItem",
                column: "OrdersOrderId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OrderItem");

            migrationBuilder.DropColumn(
                name: "City",
                table: "Order");

            migrationBuilder.DropColumn(
                name: "Country",
                table: "Order");

            migrationBuilder.DropColumn(
                name: "OrderDate",
                table: "Order");

            migrationBuilder.DropColumn(
                name: "ShippingAddress",
                table: "Order");

            migrationBuilder.DropColumn(
                name: "ShippingName",
                table: "Order");

            migrationBuilder.DropColumn(
                name: "State",
                table: "Order");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Order");

            migrationBuilder.DropColumn(
                name: "ZipCode",
                table: "Order");

            migrationBuilder.AddColumn<int>(
                name: "ProductId",
                table: "Order",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Quantity",
                table: "Order",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}

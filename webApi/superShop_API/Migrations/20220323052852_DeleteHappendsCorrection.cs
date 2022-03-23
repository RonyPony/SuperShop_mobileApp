using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace superShop_API.Migrations
{
    public partial class DeleteHappendsCorrection : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductOrders_Orders_orderId",
                table: "ProductOrders");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductOrders_Orders_orderId",
                table: "ProductOrders",
                column: "orderId",
                principalTable: "Orders",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductOrders_Orders_orderId",
                table: "ProductOrders");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductOrders_Orders_orderId",
                table: "ProductOrders",
                column: "orderId",
                principalTable: "Orders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}

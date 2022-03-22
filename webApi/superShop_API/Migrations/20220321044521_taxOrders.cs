using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace superShop_API.Migrations
{
    public partial class taxOrders : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "totalTax",
                table: "Orders",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "totalWhitoutTaxes",
                table: "Orders",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "totalTax",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "totalWhitoutTaxes",
                table: "Orders");
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace superShop_API.Migrations
{
    public partial class DeleteHappends : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_branches_Categories_categoryId",
                table: "branches");

            migrationBuilder.DropForeignKey(
                name: "FK_branches_malls_mallId",
                table: "branches");

            migrationBuilder.DropForeignKey(
                name: "FK_Orders_AspNetUsers_userId",
                table: "Orders");

            migrationBuilder.DropForeignKey(
                name: "FK_Orders_branches_branchId",
                table: "Orders");

            migrationBuilder.DropForeignKey(
                name: "FK_products_branches_branchId",
                table: "products");

            migrationBuilder.AddForeignKey(
                name: "FK_branches_Categories_categoryId",
                table: "branches",
                column: "categoryId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_branches_malls_mallId",
                table: "branches",
                column: "mallId",
                principalTable: "malls",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_AspNetUsers_userId",
                table: "Orders",
                column: "userId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_branches_branchId",
                table: "Orders",
                column: "branchId",
                principalTable: "branches",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_products_branches_branchId",
                table: "products",
                column: "branchId",
                principalTable: "branches",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_branches_Categories_categoryId",
                table: "branches");

            migrationBuilder.DropForeignKey(
                name: "FK_branches_malls_mallId",
                table: "branches");

            migrationBuilder.DropForeignKey(
                name: "FK_Orders_AspNetUsers_userId",
                table: "Orders");

            migrationBuilder.DropForeignKey(
                name: "FK_Orders_branches_branchId",
                table: "Orders");

            migrationBuilder.DropForeignKey(
                name: "FK_products_branches_branchId",
                table: "products");

            migrationBuilder.AddForeignKey(
                name: "FK_branches_Categories_categoryId",
                table: "branches",
                column: "categoryId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_branches_malls_mallId",
                table: "branches",
                column: "mallId",
                principalTable: "malls",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_AspNetUsers_userId",
                table: "Orders",
                column: "userId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_branches_branchId",
                table: "Orders",
                column: "branchId",
                principalTable: "branches",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_products_branches_branchId",
                table: "products",
                column: "branchId",
                principalTable: "branches",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

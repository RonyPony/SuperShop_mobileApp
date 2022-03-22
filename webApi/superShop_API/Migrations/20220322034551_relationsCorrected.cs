using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace superShop_API.Migrations
{
    public partial class relationsCorrected : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_branches_Categories_categoryId1",
                table: "branches");

            migrationBuilder.DropForeignKey(
                name: "FK_branches_malls_mallId1",
                table: "branches");

            migrationBuilder.DropForeignKey(
                name: "FK_Orders_AspNetUsers_userId1",
                table: "Orders");

            migrationBuilder.DropForeignKey(
                name: "FK_Orders_branches_branchId1",
                table: "Orders");

            migrationBuilder.DropForeignKey(
                name: "FK_products_branches_branchId1",
                table: "products");

            migrationBuilder.DropIndex(
                name: "IX_products_branchId1",
                table: "products");

            migrationBuilder.DropIndex(
                name: "IX_Orders_branchId1",
                table: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_Orders_userId1",
                table: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_branches_categoryId1",
                table: "branches");

            migrationBuilder.DropIndex(
                name: "IX_branches_mallId1",
                table: "branches");

            migrationBuilder.AlterColumn<Guid>(
                name: "userId1",
                table: "Orders",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<Guid>(
                name: "branchId1",
                table: "Orders",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.CreateIndex(
                name: "IX_products_branchId",
                table: "products",
                column: "branchId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_branchId",
                table: "Orders",
                column: "branchId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_userId",
                table: "Orders",
                column: "userId");

            migrationBuilder.CreateIndex(
                name: "IX_branches_categoryId",
                table: "branches",
                column: "categoryId");

            migrationBuilder.CreateIndex(
                name: "IX_branches_mallId",
                table: "branches",
                column: "mallId");

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

            migrationBuilder.DropIndex(
                name: "IX_products_branchId",
                table: "products");

            migrationBuilder.DropIndex(
                name: "IX_Orders_branchId",
                table: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_Orders_userId",
                table: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_branches_categoryId",
                table: "branches");

            migrationBuilder.DropIndex(
                name: "IX_branches_mallId",
                table: "branches");

            migrationBuilder.AlterColumn<Guid>(
                name: "userId1",
                table: "Orders",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "branchId1",
                table: "Orders",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_products_branchId1",
                table: "products",
                column: "branchId1");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_branchId1",
                table: "Orders",
                column: "branchId1");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_userId1",
                table: "Orders",
                column: "userId1");

            migrationBuilder.CreateIndex(
                name: "IX_branches_categoryId1",
                table: "branches",
                column: "categoryId1");

            migrationBuilder.CreateIndex(
                name: "IX_branches_mallId1",
                table: "branches",
                column: "mallId1");

            migrationBuilder.AddForeignKey(
                name: "FK_branches_Categories_categoryId1",
                table: "branches",
                column: "categoryId1",
                principalTable: "Categories",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_branches_malls_mallId1",
                table: "branches",
                column: "mallId1",
                principalTable: "malls",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_AspNetUsers_userId1",
                table: "Orders",
                column: "userId1",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_branches_branchId1",
                table: "Orders",
                column: "branchId1",
                principalTable: "branches",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_products_branches_branchId1",
                table: "products",
                column: "branchId1",
                principalTable: "branches",
                principalColumn: "Id");
        }
    }
}

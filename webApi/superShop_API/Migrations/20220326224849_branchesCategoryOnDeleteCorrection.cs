using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace superShop_API.Migrations
{
    public partial class branchesCategoryOnDeleteCorrection : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_branches_Categories_categoryId",
                table: "branches");

            migrationBuilder.AddForeignKey(
                name: "FK_branches_Categories_categoryId",
                table: "branches",
                column: "categoryId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_branches_Categories_categoryId",
                table: "branches");

            migrationBuilder.AddForeignKey(
                name: "FK_branches_Categories_categoryId",
                table: "branches",
                column: "categoryId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}

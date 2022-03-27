using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace superShop_API.Migrations
{
    public partial class branchesMallOnUpdateCorrection : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_branches_Categories_categoryId",
                table: "branches");

            migrationBuilder.DropForeignKey(
                name: "FK_branches_malls_mallId",
                table: "branches");

            migrationBuilder.AddForeignKey(
                name: "FK_branches_Categories_categoryId",
                table: "branches",
                column: "categoryId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict, onUpdate: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_branches_malls_mallId",
                table: "branches",
                column: "mallId",
                principalTable: "malls",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict, onUpdate: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_branches_Categories_categoryId",
                table: "branches");

            migrationBuilder.DropForeignKey(
                name: "FK_branches_malls_mallId",
                table: "branches");

            migrationBuilder.AddForeignKey(
                name: "FK_branches_Categories_categoryId",
                table: "branches",
                column: "categoryId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict, onUpdate: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_branches_malls_mallId",
                table: "branches",
                column: "mallId",
                principalTable: "malls",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}

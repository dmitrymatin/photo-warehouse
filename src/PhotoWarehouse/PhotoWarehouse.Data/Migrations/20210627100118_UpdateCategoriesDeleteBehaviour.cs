using Microsoft.EntityFrameworkCore.Migrations;

namespace PhotoWarehouse.Data.Migrations
{
    public partial class UpdateCategoriesDeleteBehaviour : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Photos_PhotoCategories_CategoryId",
                table: "Photos");

            migrationBuilder.AddForeignKey(
                name: "FK_Photos_PhotoCategories_CategoryId",
                table: "Photos",
                column: "CategoryId",
                principalTable: "PhotoCategories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Photos_PhotoCategories_CategoryId",
                table: "Photos");

            migrationBuilder.AddForeignKey(
                name: "FK_Photos_PhotoCategories_CategoryId",
                table: "Photos",
                column: "CategoryId",
                principalTable: "PhotoCategories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

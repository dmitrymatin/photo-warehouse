using Microsoft.EntityFrameworkCore.Migrations;

namespace PhotoWarehouse.Data.Migrations
{
    public partial class AddPathToPhotoItem : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Path",
                table: "PhotoItems",
                type: "text",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Path",
                table: "PhotoItems");
        }
    }
}

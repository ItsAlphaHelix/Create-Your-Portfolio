using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Portfolio.API.Migrations
{
    public partial class AddedColumnProjectDetailsImageUrlTOProjectImagesTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ImageUrl",
                table: "ProjectImages",
                newName: "ProjectDetailsImageUrl");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ProjectDetailsImageUrl",
                table: "ProjectImages",
                newName: "ImageUrl");
        }
    }
}

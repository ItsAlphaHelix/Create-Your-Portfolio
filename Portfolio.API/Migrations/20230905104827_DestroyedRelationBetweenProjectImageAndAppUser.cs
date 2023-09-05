using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Portfolio.API.Migrations
{
    public partial class DestroyedRelationBetweenProjectImageAndAppUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProjectImages_AspNetUsers_UserId",
                table: "ProjectImages");

            migrationBuilder.DropIndex(
                name: "IX_ProjectImages_UserId",
                table: "ProjectImages");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "ProjectImages");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "ProjectImages",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectImages_UserId",
                table: "ProjectImages",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectImages_AspNetUsers_UserId",
                table: "ProjectImages",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

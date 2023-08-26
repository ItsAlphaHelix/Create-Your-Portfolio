using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Portfolio.API.Migrations
{
    public partial class AddAboutUsersTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AboutUser_AspNetUsers_UserId",
                table: "AboutUser");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AboutUser",
                table: "AboutUser");

            migrationBuilder.RenameTable(
                name: "AboutUser",
                newName: "AboutUsers");

            migrationBuilder.RenameIndex(
                name: "IX_AboutUser_UserId",
                table: "AboutUsers",
                newName: "IX_AboutUsers_UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AboutUsers",
                table: "AboutUsers",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AboutUsers_AspNetUsers_UserId",
                table: "AboutUsers",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AboutUsers_AspNetUsers_UserId",
                table: "AboutUsers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AboutUsers",
                table: "AboutUsers");

            migrationBuilder.RenameTable(
                name: "AboutUsers",
                newName: "AboutUser");

            migrationBuilder.RenameIndex(
                name: "IX_AboutUsers_UserId",
                table: "AboutUser",
                newName: "IX_AboutUser_UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AboutUser",
                table: "AboutUser",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AboutUser_AspNetUsers_UserId",
                table: "AboutUser",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

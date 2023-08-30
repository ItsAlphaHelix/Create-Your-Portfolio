using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Portfolio.API.Migrations
{
    public partial class removeTableUserProgramLanguages : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserProgramLanguages_AspNetUsers_UserId",
                table: "UserProgramLanguages");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserProgramLanguages",
                table: "UserProgramLanguages");

            migrationBuilder.RenameTable(
                name: "UserProgramLanguages",
                newName: "UserProgramLanguage");

            migrationBuilder.RenameIndex(
                name: "IX_UserProgramLanguages_UserId",
                table: "UserProgramLanguage",
                newName: "IX_UserProgramLanguage_UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserProgramLanguage",
                table: "UserProgramLanguage",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UserProgramLanguage_AspNetUsers_UserId",
                table: "UserProgramLanguage",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserProgramLanguage_AspNetUsers_UserId",
                table: "UserProgramLanguage");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserProgramLanguage",
                table: "UserProgramLanguage");

            migrationBuilder.RenameTable(
                name: "UserProgramLanguage",
                newName: "UserProgramLanguages");

            migrationBuilder.RenameIndex(
                name: "IX_UserProgramLanguage_UserId",
                table: "UserProgramLanguages",
                newName: "IX_UserProgramLanguages_UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserProgramLanguages",
                table: "UserProgramLanguages",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UserProgramLanguages_AspNetUsers_UserId",
                table: "UserProgramLanguages",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

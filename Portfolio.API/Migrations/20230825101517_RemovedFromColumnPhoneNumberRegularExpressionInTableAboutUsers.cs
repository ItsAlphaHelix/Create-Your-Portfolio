using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Portfolio.API.Migrations
{
    public partial class RemovedFromColumnPhoneNumberRegularExpressionInTableAboutUsers : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_AboutUsers_UserId",
                table: "AboutUsers");

            migrationBuilder.CreateIndex(
                name: "IX_AboutUsers_UserId",
                table: "AboutUsers",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_AboutUsers_UserId",
                table: "AboutUsers");

            migrationBuilder.CreateIndex(
                name: "IX_AboutUsers_UserId",
                table: "AboutUsers",
                column: "UserId",
                unique: true);
        }
    }
}

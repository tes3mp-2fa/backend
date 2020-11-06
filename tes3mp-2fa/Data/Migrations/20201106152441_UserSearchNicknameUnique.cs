using Microsoft.EntityFrameworkCore.Migrations;

namespace tes3mp_verifier.Data.Migrations
{
  public partial class UserSearchNicknameUnique : Migration
  {
    protected override void Up(MigrationBuilder migrationBuilder)
    {
      migrationBuilder.CreateIndex(
        name: "IX_Users_SearchNickname",
        table: "Users",
        column: "SearchNickname",
        unique: true
      );
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
      migrationBuilder.DropIndex(
        name: "IX_Users_SearchNickname",
        table: "Users"
      );
    }
  }
}

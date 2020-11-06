using Microsoft.EntityFrameworkCore.Migrations;

namespace tes3mp_verifier.Data.Migrations
{
  public partial class UserSearchNickname : Migration
  {
    protected override void Up(MigrationBuilder migrationBuilder)
    {
      migrationBuilder.AddColumn<string>(
        name: "SearchNickname",
        table: "Users",
        nullable: false,
        defaultValue: ""
      );
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
      migrationBuilder.DropColumn(
        name: "SearchNickname",
        table: "Users"
      );
    }
  }
}

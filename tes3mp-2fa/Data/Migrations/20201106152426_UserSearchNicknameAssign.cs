using Microsoft.EntityFrameworkCore.Migrations;
using System.Collections.Generic;
using System.Linq;
using tes3mp_verifier.Data.Models;

namespace tes3mp_verifier.Data.Migrations
{
  public partial class UserSearchNicknameAssign : Migration
  {
    protected override void Up(MigrationBuilder migrationBuilder)
    {
      var context = Startup.ContextFactory();
      IList<User> users;
      int i = 0;
      int CHUNK = 300;
      do
      {
        users = context.Users.Skip(i * CHUNK).Take(CHUNK).ToList();

        foreach (var user in users)
          user.SearchNickname = User.NormalizeNickname(user.Nickname);

        context.UpdateRange(users);
        context.SaveChanges();
        i++;
      } while (users.Count > 0);
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {

    }
  }
}

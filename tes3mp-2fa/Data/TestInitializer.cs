using System.Collections.Generic;
using tes3mp_verifier.Data.Models;

namespace tes3mp_verifier.Data
{
  public class TestInitializer
  {
    protected void Seed(VerifierContext context)
    {
      var users= new List<User> {
      };
      users.ForEach(s => context.Users.Add(s));
      context.SaveChanges();
    }
  }
}

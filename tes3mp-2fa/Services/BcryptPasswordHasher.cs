using Bcrypt = BCrypt.Net.BCrypt;

namespace tes3mp_verifier.Services
{
  public class BcryptPasswordHasher: PasswordHasher
  {
    public BcryptPasswordHasher(): base() {}

    public string Hash(string password)
    {
      return Bcrypt.HashPassword(password);
    }

    bool PasswordHasher.Verify(string inputPassword, string hashedPassword)
    {
      return Bcrypt.Verify(inputPassword, hashedPassword);
    }
  }
}

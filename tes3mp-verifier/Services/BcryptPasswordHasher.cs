using Bcrypt = BCrypt.Net.BCrypt;

namespace tes3mp_verifier.Services
{
  public class BcryptPasswordHasher: IPasswordHasher
  {
    public BcryptPasswordHasher(): base() {}

    public string Hash(string password)
    {
      return Bcrypt.HashPassword(password);
    }

    bool IPasswordHasher.Verify(string inputPassword, string hashedPassword)
    {
      return Bcrypt.Verify(inputPassword, hashedPassword);
    }
  }
}

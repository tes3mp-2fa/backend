namespace tes3mp_verifier.Services
{
  public interface PasswordHasher
  {
    public string Hash(string password);
    public bool Verify(string inputPassword, string hashedPassword);
  }
}

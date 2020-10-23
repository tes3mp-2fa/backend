namespace tes3mp_verifier.Services
{
  public interface IPasswordHasher
  {
    public string Hash(string password);
    public bool Verify(string inputPassword, string hashedPassword);
  }
}

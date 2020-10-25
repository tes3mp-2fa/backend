using System.Security.Cryptography;
using System.Text;

namespace tes3mp_verifier.Services
{
  public class SHAPhoneNumberHasher : PhoneNumberHasher
  {
    private readonly SHA512 sha;
    public SHAPhoneNumberHasher()
    {
      sha = new SHA512Managed();
    }

    public string Hash(string phoneNumber)
    {
      var bytes = Encoding.UTF8.GetBytes(phoneNumber);
      var hash = sha.ComputeHash(bytes);
      return Encoding.UTF8.GetString(hash);
    }
  }
}

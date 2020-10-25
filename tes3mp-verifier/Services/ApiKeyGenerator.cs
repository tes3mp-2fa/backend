namespace tes3mp_verifier.Services
{
  public class ApiKeyGenerator: StringGenerator
  {
    private static readonly char[] CHARACTER_SET =
    {
      'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M',
        'N', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z',
      '1', '2', '3', '4', '5', '6', '7', '8', '9'
    };
    private const int LENGTH = 20;

    public ApiKeyGenerator(): base(CHARACTER_SET, LENGTH) {}
  }
}

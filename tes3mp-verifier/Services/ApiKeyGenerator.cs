using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using tes3mp_verifier.Data;

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
    private const int MAX_REPEATS = 10;

    private readonly VerifierContext _context;

    public ApiKeyGenerator(VerifierContext context): base(CHARACTER_SET, LENGTH) {
      _context = context;
    }

    public async Task<string> GenerateUnique()
    {
      string uniqueKey = null;
      for(int i = 0; i < MAX_REPEATS; i++)
      {
        var key = Generate();
        var apiKey = await _context.ApiKeys
          .Where(s => s.Key == key)
          .FirstOrDefaultAsync();
        if (apiKey == null)
        {
          uniqueKey = key;
          break;
        }
      }
      if (uniqueKey == null) throw new Exception();
      return uniqueKey;
    }
  }
}

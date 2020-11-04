using System;
using System.Text;

namespace tes3mp_verifier.Services
{
  public class StringGenerator
  {
    private char[] CharacterSet;
    private int LastCharacter;
    private int Length;

    private readonly Random _random;
    public StringGenerator(char[] characterSet, int length)
    {
      CharacterSet = characterSet;
      LastCharacter = CharacterSet.Length - 1;
      Length = length;

      _random = new Random();
    }
    public string Generate()
    {
      var builder = new StringBuilder(Length);
      for (int i = 0; i < Length; i++)
      {
        int j = _random.Next(0, LastCharacter);
        builder.Append(CharacterSet[j]);
      }
      return builder.ToString();
    }
  }
}

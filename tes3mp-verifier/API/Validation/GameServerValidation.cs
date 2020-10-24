using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace tes3mp_verifier.API.Validation
{
  public static class GameServerValidation
  {
    public const int NAME_MIN = 3;
    public const int NAME_MAX = 100;

    public const int ADDRESS_MAX = 1000;
    public const int DESCRIPTION_MAX = 1000;
  }
}

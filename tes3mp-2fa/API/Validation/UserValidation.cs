namespace tes3mp_verifier.API.Validation
{
  public static class UserValidation
  {
    public const int NICKNAME_MIN = 3;
    public const int NICKNAME_MAX = 50;
    public const string NICKNAME_REGEX = "^[a-zA-Z0-9\\-\\._+]+$";

    public const int PASSWORD_MIN = 8;
    public const int PASSWORD_MAX = 10000;
  }
}

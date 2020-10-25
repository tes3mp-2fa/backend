namespace tes3mp_verifier.Services
{
  public interface PhoneVerifier
  {
    public string SendSMS(string phoneNumber);
    public bool Check(string requestId, string code);
  }
}

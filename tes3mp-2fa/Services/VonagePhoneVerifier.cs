using Microsoft.Extensions.Configuration;
using Nexmo.Api;

namespace tes3mp_verifier.Services
{
  public class VonagePhoneVerifier : PhoneVerifier
  {
    private readonly Client client;
    public VonagePhoneVerifier(IConfiguration configuration)
    {
      var apiKey = configuration["Vonage:ApiKey"];
      var apiSecret = configuration["Vonage:ApiSecret"];
      client = new Client(creds: new Nexmo.Api.Request.Credentials
      {
        ApiKey = apiKey,
        ApiSecret = apiSecret
      });
    }

    public string SendSMS(string phoneNumber)
    {
      var start = client.NumberVerify.Verify(new NumberVerify.VerifyRequest
      {
        number = phoneNumber,
        brand = "TES3MP Verifier",
        code_length = "4"
      });

      return start.request_id;
    }

    public bool Check(string requestId, string code)
    {
      var result = client.NumberVerify.Check(new NumberVerify.CheckRequest
      {
        request_id = requestId,
        code = code
      });

      return result.status == "0";
    }

    public void Cancel(string requestId)
    {
      client.NumberVerify.Control(new NumberVerify.ControlRequest
      {
        cmd = "cancel",
        request_id = requestId
      });
    }
  }
}

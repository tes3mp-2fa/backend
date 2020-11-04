using Microsoft.AspNetCore.Mvc;

namespace tes3mp_verifier.API
{
  public class ApiRoute: RouteAttribute
  {
    public ApiRoute(string template) : base("api/" + template) { }
  }
}

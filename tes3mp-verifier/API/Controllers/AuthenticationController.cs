using Microsoft.AspNetCore.Mvc;
using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using tes3mp_verifier.Data;
using tes3mp_verifier.Data.Models;
using tes3mp_verifier.Services;

namespace tes3mp_verifier.API.Controllers
{
  [Route("api/auth")]
  [ApiController]
  public class AuthenticationController : ControllerBase
  {
    private IPasswordHasher hasher;
    private VerifierContext context;
    public AuthenticationController(IPasswordHasher _hasher, VerifierContext _context)
    {
      hasher = _hasher;
      context = _context;
    }
    public class RegisterInput
    {
      [Required]
      [StringLength(50, MinimumLength = 3)]
      [RegularExpression(
        "^[a-zA-Z0-9\\-/\\.]+$"
      )]
      public string Nickname { get; set; }
      [Required]
      [EmailAddress]
      public string Email { get; set; }
      [Required]
      [RegularExpression("^.{8,}$")]
      public string Password { get; set; }
    }
    [HttpPost]
    [Route("register")]
    public async Task<ActionResult> Register([FromBody] RegisterInput input)
    {
      User user = new User()
      {
        Nickname = input.Nickname,
        Email = input.Email,
        Password = hasher.Hash(input.Password),
        Created = DateTime.Now
      };
      context.Users.Add(user);
      await context.SaveChangesAsync();

      return Ok();
    }
  }
}

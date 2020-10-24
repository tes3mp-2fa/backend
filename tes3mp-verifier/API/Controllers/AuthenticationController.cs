using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using tes3mp_verifier.API.Validation;
using tes3mp_verifier.Data.Models;

namespace tes3mp_verifier.API.Controllers
{
  [Route("api/auth")]
  [ApiController]
  [AllowAnonymous]
  public class AuthenticationController : ControllerBase
  {
    private readonly UserManager _userManager;

    public AuthenticationController(UserManager userManager)
    {
      _userManager = userManager;
    }

    public class RegisterInput
    {
      [Required]
      [StringLength(UserValidation.NICKNAME_MAX, MinimumLength = UserValidation.NICKNAME_MIN)]
      [RegularExpression(UserValidation.NICKNAME_REGEX)]
      public string Nickname { get; set; }
      [EmailAddress]
      public string Email { get; set; }
      [Required]
      [StringLength(UserValidation.PASSWORD_MAX, MinimumLength = UserValidation.PASSWORD_MIN)]
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
        Password = input.Password,
        Created = DateTime.Now
      };
      await _userManager.Register(user);

      return Ok();
    }

    public class LoginInput
    {
      [Required]
      [StringLength(UserValidation.NICKNAME_MAX, MinimumLength = UserValidation.NICKNAME_MIN)]
      [RegularExpression(UserValidation.NICKNAME_REGEX)]
      public string Nickname { get; set; }
      [Required]
      [StringLength(UserValidation.PASSWORD_MAX, MinimumLength = UserValidation.PASSWORD_MIN)]
      public string Password { get; set; }
    }
    [HttpPost]
    [Route("login")]
    public async Task<ActionResult> Login([FromBody] RegisterInput input)
    {
      await _userManager.Login(input.Nickname, input.Password);
      return Ok();
    }

    [HttpGet]
    [Route("logout")]
    public async Task<ActionResult> Logout()
    {
      await _userManager.Logout();
      return Ok();
    }
  }
}

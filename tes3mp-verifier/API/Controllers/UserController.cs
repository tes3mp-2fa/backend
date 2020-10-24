using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using tes3mp_verifier.API.Validation;
using tes3mp_verifier.Data;
using tes3mp_verifier.Data.Models;

namespace tes3mp_verifier.API.Controllers
{
  [ApiRoute("user")]
  [ApiController]
  public class UserController : ControllerBase
  {
    private readonly UserManager _userManager;
    private readonly VerifierContext _context;

    public UserController(UserManager userManager, VerifierContext context)
    {
      _userManager = userManager;
      _context = context;
    }

    [HttpGet]
    [Route("me")]
    public async Task<User> Me()
    {
      return await _userManager.CurrentUser(q => q.Include(u => u.Settings));
    }

    public class ChangePasswordInput
    {
      [Required]
      public string OldPassword { get; set; }
      [Required]
      [StringLength(UserValidation.PASSWORD_MAX, MinimumLength = UserValidation.PASSWORD_MIN)]
      public string NewPassword { get; set; }
    }
    [HttpPost]
    [Route("change-password")]
    public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordInput input)
    {
      var user = await _userManager.CurrentUser();
      if (!_userManager.CorrectPassword(user, input.OldPassword))
        return BadRequest();

      _userManager.UpdatePassword(user, input.NewPassword);
      await _context.SaveChangesAsync();

      return Ok();
    }

    public class ChangeEmailInput
    {
      [Required]
      public string Password { get; set; }
      [Required]
      [EmailAddress]
      public string NewEmail { get; set; }
    }
    [HttpPost]
    [Route("change-email")]
    public async Task<IActionResult> ChangeEmail([FromBody] ChangeEmailInput input)
    {
      var user = await _userManager.CurrentUser();
      if (!_userManager.CorrectPassword(user, input.Password))
        return BadRequest();

      user.Email = input.NewEmail;
      await _context.SaveChangesAsync();

      return Ok();
    }

    [HttpPost]
    [Route("change-settings")]
    public async Task<IActionResult> ChangeSettings([FromBody] UserSettingsData data)
    {
      var user = await _userManager.CurrentUser(q => q.Include(u => u.Settings));
      user.Settings.Data = data; // TODO: validation of settings?
      await _context.SaveChangesAsync();

      return Ok();
    }
  }
}

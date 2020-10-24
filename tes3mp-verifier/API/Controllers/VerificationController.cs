using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using tes3mp_verifier.Data;
using tes3mp_verifier.Data.Models;

namespace tes3mp_verifier.API.Controllers
{
  [Route("verify")]
  [ApiController]
  public class VerificationController : ControllerBase
  {
    private readonly UserManager _userManager;
    private readonly VerifierContext _context;

    public VerificationController(UserManager userManager, VerifierContext context)
    {
      _userManager = userManager;
      _context = context;
    }

    public class RequestInput
    {
      [Required]
      public string PhoneNumber { get; set; }
    }
    [HttpPost]
    [Route("start")]
    public async Task<IActionResult> Start([FromBody] RequestInput input)
    {
      var user = await _userManager.CurrentUser();
      user.PhoneNumber = input.PhoneNumber; // TODO: hash the phone number

      var verification = new Verification()
      {
        User = user,
        Password = "", // TODO: implement an SMS verification API
        Created = DateTime.Now,
        Confirmed = null
      };
      await _context.Verifications.AddAsync(verification);
      await _context.SaveChangesAsync();

      return Ok();
    }

    public class ConfirmInput
    {
      [Required]
      public string Password { get; set; }
    }
    [HttpPost]
    [Route("confirm")]
    public async Task<IActionResult> Confirm([FromBody] ConfirmInput input)
    {
      var user = await _userManager.CurrentUser();
      var verification = await _context.Verifications
        .Where(s => s.UserId == user.Id)
        .OrderByDescending(s => s.Created)
        .FirstOrDefaultAsync();
      if (verification == null) return NotFound();

      if (verification.Password == input.Password)
        verification.Confirmed = DateTime.Now;
      else
        return BadRequest();

      return Ok();
    }
  }
}

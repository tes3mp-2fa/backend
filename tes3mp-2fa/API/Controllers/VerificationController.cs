using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using tes3mp_verifier.Data;
using tes3mp_verifier.Data.Models;
using tes3mp_verifier.Services;

namespace tes3mp_verifier.API.Controllers
{
  [ApiRoute("verify")]
  [ApiController]
  public class VerificationController : ControllerBase
  {
    private readonly UserManager _userManager;
    private readonly VerifierContext _context;
    private readonly PhoneVerifier _verifier;
    private readonly PhoneNumberHasher _hasher;

    public VerificationController(
      UserManager userManager,
      VerifierContext context,
      PhoneVerifier verifier,
      PhoneNumberHasher hasher)
    {
      _userManager = userManager;
      _context = context;
      _verifier = verifier;
      _hasher = hasher;
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
      var user = await _userManager.CurrentUser(q => q.Include(u => u.Verification));
      var hashedPhoneNumber = _hasher.Hash(input.PhoneNumber);

      if (user.Verification != null)
      {
        if (user.PhoneNumber == hashedPhoneNumber)
          return BadRequest();

        _context.Remove(user.Verification);
        user.Verification = null;
      }

      var nonUnique = await _context.Users
        .Where(u => u.Id != user.Id)
        .Where(u => u.PhoneNumber == hashedPhoneNumber)
        .FirstOrDefaultAsync();

      if (nonUnique != null) return BadRequest();

      user.PhoneNumber = hashedPhoneNumber;
      var code = _verifier.SendSMS(input.PhoneNumber);
      var verification = new Verification()
      {
        User = user,
        Code = code,
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
      public string Code { get; set; }
    }
    [HttpPost]
    [Route("confirm")]
    public async Task<IActionResult> Confirm([FromBody] ConfirmInput input)
    {
      var user = await _userManager.CurrentUser(q => q.Include(u => u.Verification));
      var verification = user.Verification;
      if (verification == null) return NotFound();

      if (_verifier.Check(verification.Code, input.Code))
      {
        verification.Confirmed = DateTime.Now;
        await _context.SaveChangesAsync();
      }
      else
        return BadRequest();

      return Ok();
    }
  }
}

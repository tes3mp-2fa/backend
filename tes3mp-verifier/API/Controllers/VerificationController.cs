﻿using System;
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
      var user = await _userManager.CurrentUser();
      user.PhoneNumber = _hasher.Hash(input.PhoneNumber);

      var password = _verifier.SendSMS(input.PhoneNumber);
      var verification = new Verification()
      {
        User = user,
        Password = password,
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
        .Where(s => s.Confirmed == null)
        .OrderByDescending(s => s.Created)
        .FirstOrDefaultAsync();
      if (verification == null) return NotFound();

      if (_verifier.Check(verification.Password, input.Password))
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

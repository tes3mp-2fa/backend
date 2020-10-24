﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using tes3mp_verifier.API.Validation;
using tes3mp_verifier.Data;
using tes3mp_verifier.Data.Models;
using tes3mp_verifier.Data.Queries;

namespace tes3mp_verifier.API.Controllers
{
  [ApiRoute("gameserver")]
  [ApiController]
  public class GameServerController : ControllerBase
  {
    private readonly UserManager _userManager;
    private readonly VerifierContext _context;

    public GameServerController(UserManager userManager, VerifierContext context)
    {
      _userManager = userManager;
      _context = context;
    }

    public class CreateInput
    {
      [Required]
      [StringLength(GameServerValidation.NAME_MAX, MinimumLength = GameServerValidation.NAME_MIN)]
      public string Name { get; set; }
      [StringLength(GameServerValidation.ADDRESS_MAX)]
      public string Address { get; set; }
      [StringLength(GameServerValidation.DESCRIPTION_MAX)]
      public string Description { get; set; }
    }
    [HttpPost]
    [Route("create")]
    public async Task<IActionResult> Create(CreateInput input)
    {
      var user = await _userManager.CurrentUser();
      GameServer gameServer = new GameServer
      {
        Owner = user,
        Name = input.Name,
        Address = input.Address,
        Description = input.Description,
        Created = DateTime.Now
      };

      _context.GameServers.Add(gameServer);
      await _context.SaveChangesAsync();

      return Ok();
    }

    public class UpdateInput: CreateInput
    {
      [Required]
      public int Id { get; set; }
    }
    [HttpPut]
    [Route("update")]
    public async Task<IActionResult> Update(UpdateInput input)
    {
      var user = await _userManager.CurrentUser();
      GameServer gameServer = await _context.GameServers
        .Where(s => s.OwnerId == user.Id)
        .Where(s => s.Id == input.Id)
        .FirstOrDefaultAsync();

      if (gameServer == null) return NotFound(); 

      gameServer.Name = input.Name;
      gameServer.Address = input.Address;
      gameServer.Description = input.Description;

      await _context.SaveChangesAsync();

      return Ok();
    }

    [HttpGet]
    [Route("list")]
    public async Task<ICollection<GameServer>> List([FromQuery] int id, [FromQuery] int page = 1)
    {
      return await new PaginateWithOverflow<GameServer>(page, 10).ToListAsync(
        _context.GameServers
        .Where(s => s.OwnerId == id)
      );
    }
  }
}

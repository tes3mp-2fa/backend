using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using tes3mp_verifier.API.Validation;
using tes3mp_verifier.Data;
using tes3mp_verifier.Data.Models;
using tes3mp_verifier.Data.Queries;
using tes3mp_verifier.Services;

namespace tes3mp_verifier.API.Controllers
{
  [ApiRoute("gameserver")]
  [ApiController]
  public class GameServerController : ControllerBase
  {
    private readonly UserManager _userManager;
    private readonly VerifierContext _context;
    private readonly ApiKeyGenerator _generator;

    public GameServerController(
      UserManager userManager,
      VerifierContext context,
      ApiKeyGenerator generator)
    {
      _userManager = userManager;
      _context = context;
      _generator = generator;
    }
   
    [AllowAnonymous]
    [HttpGet]
    [Route("")]
    public async Task<GameServer> Get([FromQuery] int id)
    {
      return await _context.GameServers
        .Where(s => s.Id == id)
        .FirstOrDefaultAsync();
    }

    [HttpGet]
    [Route("mine")]
    public async Task<GameServer> Mine([FromQuery] int id)
    {
      var user = await _userManager.CurrentUser();
      return await _context.GameServers
        .Where(s => s.Id == id)
        .Where(s => s.OwnerId == user.Id)
        .Include(s => s.ApiKey)
        .FirstOrDefaultAsync();
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
      gameServer.ApiKey = ApiKey.Create(await _generator.GenerateUnique());

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

    [AllowAnonymous]
    [HttpGet]
    [Route("list")]
    public async Task<ICollection<GameServer>> List([FromQuery] int id, [FromQuery] int page = 1)
    {
      var paginate = new PaginateWithOverflow<GameServer>(page, 10);
      return await paginate.ToListAsync(
        _context.GameServers.Where(s => s.OwnerId == id)
      );
    }

    [HttpGet]
    [Route("api-key")]
    public async Task<ApiKey> NewApiKey([FromQuery] int id)
    {
      User user = await _userManager.CurrentUser();
      GameServer gameServer = await _context.GameServers
        .Where(s => s.Id == id)
        .Where(s => s.OwnerId == user.Id)
        .Include(s => s.ApiKey)
        .FirstOrDefaultAsync();
      if (gameServer == null) throw new Exception();

      _context.ApiKeys.Remove(gameServer.ApiKey);
      gameServer.ApiKey = ApiKey.Create(await _generator.GenerateUnique());
      await _context.SaveChangesAsync();

      return gameServer.ApiKey;
    }
  }
}

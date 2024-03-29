﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using tes3mp_verifier.Data;
using tes3mp_verifier.Data.Models;

namespace tes3mp_verifier.API.Controllers
{
  [ApiRoute("game")]
  [ApiController]
  [AllowAnonymous]
  public class GameController : ControllerBase
  {
    private readonly VerifierContext _context;

    public GameController(VerifierContext context)
    {
      _context = context;
    }

    public class GameRequest
    {
      public string ApiKey { get; set; }
    }

    public class GameResponse
    {
      public ICollection<string> Errors { get; set; }
    };

    protected async Task<ApiKey> CheckApiKey(GameRequest request, GameResponse response)
    {
      ApiKey apiKey = await _context.ApiKeys
          .Include(k => k.GameServer)
          .Where(k => k.Key == request.ApiKey)
          .FirstOrDefaultAsync();
      if (apiKey == null)
      {
        response.Errors.Add("Invalid API key!");
      }

      return apiKey;
    }


    public class LoginRequest: GameRequest
    {
      public string Nickname { get; set; }
      public string LoginKey { get; set; }
      public string Ip { get; set; }
    }
    public class LoginResponse: GameResponse
    {
      public bool Success { get; set; }
      public bool Verified { get; set; }
    }

    [HttpPost]
    [Route("login")]
    public async Task<LoginResponse> Login([FromBody] LoginRequest request)
    {
      var response = new LoginResponse
      {
        Success = false,
        Verified = false,
        Errors = new List<string>()
      };

      try
      {
        ApiKey apiKey = await CheckApiKey(request, response);
        if(apiKey == null) return response;

        LoginKey loginKey = await _context.LoginKeys
          .Include(k => k.User)
          .Include(u => u.User.Verification)
          .Include(k => k.User.Settings)
          .Where(k => k.Key == request.LoginKey)
          .FirstOrDefaultAsync();

        if (loginKey == null || loginKey.User.Nickname != request.Nickname)
        {
          response.Errors.Add("Invalid login key!");
          return response;
        }

        Login login = new Login
        {
          GameServerId = apiKey.GameServer.Id,
          UserId = loginKey.User.Id,
          Ip = null,
          Created = DateTime.Now
        };
        if (loginKey.User.Settings.Data.TrackIp) login.Ip = request.Ip;

        await _context.Logins.AddAsync(login);
        await _context.SaveChangesAsync();

        response.Success = true;
        response.Verified = loginKey.User.Verification != null && loginKey.User.Verification.IsConfirmed();
      }
      catch (Exception)
      {
        response.Errors.Add("Unknown error!");
      }

      return response;
    }

    public class InfoRequest : GameRequest
    {
      public string Nickname { get; set; }
    }
    public class InfoResponse: GameResponse
    {
      public bool Exists { get; set; }
      public bool Verified { get; set; }
    }

    [HttpPost]
    [Route("info")]
    public async Task<InfoResponse> Info([FromBody] InfoRequest request)
    {
      var response = new InfoResponse
      {
        Exists = false,
        Verified = false,
        Errors = new List<string>()
      };

      try
      {
        if (await CheckApiKey(request, response) == null)
          return response;

        User user = await _context.Users
          .Include(u => u.Verification)
          .WhereNickname(request.Nickname)
          .FirstOrDefaultAsync();

        if (user != null)
        {
          response.Exists = true;
          response.Verified = user.Verification != null && user.Verification.IsConfirmed();
        }
      }
      catch (Exception)
      {
        response.Errors.Add("Unknown error!");
      }

      return response;
    }
  }
}

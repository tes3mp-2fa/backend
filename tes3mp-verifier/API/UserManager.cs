using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Security.Authentication;
using System.Security.Claims;
using System.Threading.Tasks;
using tes3mp_verifier.Data;
using tes3mp_verifier.Data.Models;

namespace tes3mp_verifier.API
{
  public class UserManager
  {
    private readonly VerifierContext _dbContext;
    private readonly Services.PasswordHasher _hasher;
    private readonly IHttpContextAccessor _httpContextAccessor;

    private const string CLAIM_ID = "Id";

    public UserManager(VerifierContext dbContext, Services.PasswordHasher hasher, IHttpContextAccessor httpContextAccessor)
    {
      _dbContext = dbContext;
      _hasher = hasher;
      _httpContextAccessor = httpContextAccessor;
    }

    public async Task SignIn(User user)
    {
      var httpContext = _httpContextAccessor.HttpContext;
      var principal = new ClaimsPrincipal(new ClaimsIdentity(
        new[] {
              new Claim(CLAIM_ID, user.Id.ToString())
        },
        DefaultAuthenticationTypes.ApplicationCookie
      ));
      await httpContext.SignInAsync(principal);
    }

    public async Task<User> Login(string nickname, string password)
    {
      User user = await _dbContext.Users
        .Where(s => s.Nickname == nickname)
        .FirstAsync();

      if (!_hasher.Verify(password, user.Password))
        throw new AuthenticationException();

      await SignIn(user);

      return user;
    }

    public async Task<User> Register(User user)
    {
      user.Password = _hasher.Hash(user.Password);

      await _dbContext.Users.AddAsync(user);
      await _dbContext.SaveChangesAsync();

      await SignIn(user);

      return user;
    }
    
    private User currentUser = null;
    public async Task<User> CurrentUser(Func<IQueryable<User>, IQueryable<User>> relations = null)
    {
      if (currentUser != null) return currentUser;

      var httpContext = _httpContextAccessor.HttpContext;
      var claim = httpContext.User.FindFirst(CLAIM_ID);
      if (claim == null) throw new AuthenticationException();

      int id = 0;
      try
      {
        id = int.Parse(claim.Value);
      }
      catch(Exception)
      {
        throw new AuthenticationException();
      }

      var query = _dbContext.Users
        .Where(s => s.Id == id);

      if (relations != null) query = relations(query);

      currentUser = await query.FirstOrDefaultAsync();

      return currentUser;
    }

    public async Task Logout()
    {
      var httpContext = _httpContextAccessor.HttpContext;
      await httpContext.SignOutAsync();
    }

    public bool CorrectPassword(User user, string password)
    {
      return _hasher.Verify(password, user.Password);
    }

    public void UpdatePassword(User user, string password)
    {
      user.Password = _hasher.Hash(password);
    }
  }
}

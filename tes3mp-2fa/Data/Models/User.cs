using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json.Serialization;

namespace tes3mp_verifier.Data.Models
{
  public class User
  {
    [Key]
    public int Id { get; set; }
    private string _Nickname;
    public string Nickname
    {
      get { return _Nickname; }
      set { _Nickname = value; SearchNickname = NormalizeNickname(value); }
    }
    public string SearchNickname { get; set; }
    public string Email { get; set; }
    [JsonIgnore]
    public string PhoneNumber { get; set; }
    [JsonIgnore]
    public string Password { get; set; }
    public DateTime Created { get; set; }

    public virtual UserSettings Settings { get; set; }
    [JsonIgnore]
    public virtual ICollection<Login> Logins { get; set; }
    [JsonIgnore]
    public virtual ICollection<GameServer> GameServers { get; set; }
    [JsonIgnore]
    public virtual Verification Verification{ get; set; }
    [JsonIgnore]
    public virtual ICollection<LoginKey> LoginKeys { get; set; }

    public static string NormalizeNickname(string Nickname)
    {
      return Nickname.ToLower();
    }
  }

  public static class UserExtension
  {
    public static IQueryable<User> WhereNickname(this IQueryable<User> query, string Nickname)
    {
      string SearchNickname = User.NormalizeNickname(Nickname);
      return query.Where(u => u.SearchNickname == Nickname);
    }
  }
}

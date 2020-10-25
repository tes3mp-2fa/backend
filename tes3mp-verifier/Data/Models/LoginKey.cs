using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace tes3mp_verifier.Data.Models
{
  public class LoginKey
  {
    public static TimeSpan VALID_FOR = TimeSpan.FromMinutes(1);

    [Key]
    public string Key { get; set; }
    public int UserId { get; set; }
    public DateTime Created { get; set; }

    [JsonIgnore]
    public virtual User User { get; set; }

    public static LoginKey Create(string key)
    {
      return new LoginKey
      {
        Key = key,
        Created = DateTime.Now
      };
    }
  }
}

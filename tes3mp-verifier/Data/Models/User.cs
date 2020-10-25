using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace tes3mp_verifier.Data.Models
{
  public class User
  {
    [Key]
    public int Id { get; set; }
    public string Nickname { get; set; }
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
    public virtual ICollection<Verification> Verifications{ get; set; }
    [JsonIgnore]
    public virtual ICollection<LoginKey> LoginKeys { get; set; }
  }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace tes3mp_verifier.Data.Models
{
  public class User
  {
    [Key]
    public int Id { get; set; }
    public string Nickname { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
    public string Password { get; set; }
    public DateTime Created { get; set; }

    public virtual UserSettings Settings { get; set; }
    public virtual ICollection<Login> Logins { get; set; }
    public virtual ICollection<GameServer> GameServers { get; set; }
    public virtual ICollection<Verification> Verifications{ get; set; }
    public virtual ICollection<LoginKey> LoginKeys { get; set; }
  }
}

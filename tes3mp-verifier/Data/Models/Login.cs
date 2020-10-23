using System;
using System.ComponentModel.DataAnnotations;

namespace tes3mp_verifier.Data.Models
{
  public class Login
  {
    [Key]
    public long Id { get; set; }
    public int UserId { get; set; }
    public int GameServerId { get; set; }
    public string Ip { get; set; }
    public DateTime Created { get; set; }

    public virtual User User { get; set; }

    public virtual GameServer GameServer { get; set; }
  }
}

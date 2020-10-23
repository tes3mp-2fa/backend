using System;
using System.ComponentModel.DataAnnotations;

namespace tes3mp_verifier.Data.Models
{
  public class ApiKey
  {
    [Key]
    public string Key { get; set; }
    public int GameServerId { get; set; }
    public DateTime Created { get; set; }
    
    public virtual GameServer GameServer { get; set; }
  }
}

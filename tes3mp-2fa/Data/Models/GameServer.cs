using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace tes3mp_verifier.Data.Models
{
  public class GameServer
  {
    [Key]
    public int Id { get; set; }
    public int OwnerId { get; set; }
    public string Name { get; set; }
    public string Address { get; set; }
    public string Description { get; set; }
    public DateTime Created { get; set; }

    [JsonIgnore]
    public virtual User Owner { get; set; }
    public virtual ApiKey ApiKey { get; set; }
    [JsonIgnore]
    public virtual ICollection<Login> Logins { get; set; }
  }
}

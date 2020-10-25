using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace tes3mp_verifier.Data.Models
{
  public class Verification
  {
    [Key]
    public int Id { get; set; }
    public int UserId { get; set; }
    public string Password { get; set; }
    public DateTime Created { get; set; }
    public DateTime? Confirmed { get; set; }

    [JsonIgnore]
    public virtual User User { get; set; }

    public bool IsConfirmed()
    {
      return Confirmed != null;
    }
  }
}

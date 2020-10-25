using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using tes3mp_verifier.Services;

namespace tes3mp_verifier.Data.Models
{
  public class ApiKey
  {
    [Key]
    public string Key { get; set; }
    public int GameServerId { get; set; }
    public DateTime Created { get; set; }
    
    [JsonIgnore]
    public virtual GameServer GameServer { get; set; }

    public static ApiKey Create(ApiKeyGenerator generator)
    {
      return new ApiKey
      {
        Key = generator.Generate(),
        Created = DateTime.Now
      };
    }
  }
}

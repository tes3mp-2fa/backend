using System.Text.Json.Serialization;

namespace tes3mp_verifier.Data.Models
{
  public class UserSettings
  {
    [JsonIgnore]
    public int Id { get; set; }
    [JsonIgnore]
    public int UserId { get; set; }
    public UserSettingsData Data { get; set; }

    [JsonIgnore]
    public virtual User User { get; set; }
  }

  public class UserSettingsData
  {
    public static UserSettingsData Default()
    {
      return new UserSettingsData
      {
        TrackIp = false
      };
    }

    public bool TrackIp { get; set; }
  }
}

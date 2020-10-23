namespace tes3mp_verifier.Data.Models
{
  public class UserSettings
  {
    public int Id { get; set; }
    public int UserId { get; set; }
    public UserSettingsData Data { get; set; }

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

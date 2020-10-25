using Microsoft.EntityFrameworkCore;
using tes3mp_verifier.Data.Converters;
using tes3mp_verifier.Data.Models;

namespace tes3mp_verifier.Data
{
  public class VerifierContext : DbContext
  {
    public VerifierContext(DbContextOptions options) : base(options) { }

    public DbSet<User> Users { get; set; }
    public DbSet<LoginKey> LoginKeys { get; set; }
    public DbSet<Verification> Verifications { get; set; }
    public DbSet<GameServer> GameServers { get; set; }
    public DbSet<ApiKey> ApiKeys { get; set; }
    public DbSet<Login> Logins { get; set; }

    private void ConfigureApiKey(ModelBuilder builder)
    {
      builder.Entity<ApiKey>()
        .HasOne(s => s.GameServer)
        .WithOne(g => g.ApiKey)
        .HasForeignKey<ApiKey>(s => s.GameServerId);
    }

    private void ConfigureGameServer(ModelBuilder builder)
    {
      builder.Entity<GameServer>()
        .Property(s => s.Name)
        .IsRequired();

      builder.Entity<GameServer>()
        .HasOne(s => s.Owner)
        .WithMany(g => g.GameServers)
        .HasForeignKey(s => s.OwnerId);
    }

    private void ConfigureLogin(ModelBuilder builder)
    {
      builder.Entity<Login>()
        .HasOne(s => s.User)
        .WithMany(g => g.Logins)
        .HasForeignKey(s => s.UserId);
      builder.Entity<Login>()
        .HasOne(s => s.GameServer)
        .WithMany(g => g.Logins)
        .HasForeignKey(s => s.GameServerId);
    }

    private void ConfigureLoginKey(ModelBuilder builder)
    {
      builder.Entity<LoginKey>()
        .HasOne(s => s.User)
        .WithMany(g => g.LoginKeys)
        .HasForeignKey(s => s.UserId);
    }

    private void ConfigureUser(ModelBuilder builder)
    {
      builder.Entity<User>()
        .Property(s => s.Nickname)
        .IsRequired();
      builder.Entity<User>()
        .Property(s => s.Password)
        .IsRequired();
      builder.Entity<User>()
        .Property(s => s.PhoneNumber);

      builder.Entity<User>()
        .HasIndex(u => u.Nickname)
        .IsUnique();
      builder.Entity<User>()
        .HasIndex(u => u.Email)
        .IsUnique();
      builder.Entity<User>()
        .HasIndex(u => u.PhoneNumber)
        .IsUnique();
    }

    private void ConfigureUserSettings(ModelBuilder builder)
    {
      builder.Entity<UserSettings>()
        .Property(s => s.Data)
        .IsRequired()
        .HasConversion(new JsonValueConverter<UserSettingsData>());

      builder.Entity<UserSettings>()
        .HasOne(s => s.User)
        .WithOne(g => g.Settings)
        .HasForeignKey<UserSettings>(s => s.UserId);
    }

    private void ConfigureVerification(ModelBuilder builder)
    {
      builder.Entity<Verification>()
        .Property(s => s.Code)
        .IsRequired();

      builder.Entity<Verification>()
        .HasOne(s => s.User)
        .WithOne(g => g.Verification)
        .HasForeignKey<Verification>(s => s.UserId);
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
      ConfigureApiKey(builder);
      ConfigureGameServer(builder);
      ConfigureLogin(builder);
      ConfigureLoginKey(builder);
      ConfigureUser(builder);
      ConfigureUserSettings(builder);
      ConfigureVerification(builder);
    }
  }
}

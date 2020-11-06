using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using tes3mp_verifier.Data;
using tes3mp_verifier.Services;
using System.Threading.Tasks;
using tes3mp_verifier.API;
using tes3mp_verifier.Scheduling;
using tes3mp_verifier.Scheduling.Tasks;
using System;

namespace tes3mp_verifier
{
  public class Startup
  {
    public static Func<VerifierContext> ContextFactory;
    private IConfiguration _configuration;

    public Startup(IConfiguration configuration, IWebHostEnvironment environment)
    {
      _configuration = configuration;
      ContextFactory = () => GetContext();
    }

    VerifierContext GetContext()
    {
      var builder = new DbContextOptionsBuilder<VerifierContext>();
      builder.UseNpgsql(_configuration.GetConnectionString("VerifierContext"));
      return new VerifierContext(builder.Options);
    }

    private void ConfigureAuthentication(IServiceCollection services)
    {
      services.AddAuthentication(options =>
        {
          options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
        })
        .AddCookie(config =>
        {
          config.SlidingExpiration = true;
          config.Events = new CookieAuthenticationEvents
          {
            OnRedirectToLogin = context =>
            {
              if (context.Response.StatusCode == 200)
                context.Response.StatusCode = 401;
              return Task.CompletedTask;
            },
            OnRedirectToAccessDenied = context =>
            {
              if (context.Response.StatusCode == 200)
                context.Response.StatusCode = 401;
              return Task.CompletedTask;
            }
          };
        });
    }

    public void ConfigureServices(IServiceCollection services)
    {
      services.AddDbContext<VerifierContext>(options => {
        options.UseNpgsql(_configuration.GetConnectionString("VerifierContext"));
      });

      ConfigureAuthentication(services);

      services.AddSingleton<PasswordHasher, BcryptPasswordHasher>();
      services.AddSingleton<PhoneNumberHasher, SHAPhoneNumberHasher>();
      services.AddSingleton<PhoneVerifier, VonagePhoneVerifier>();
      services.AddScoped<ApiKeyGenerator>();
      services.AddScoped<LoginKeyGenerator>();
      services.AddScoped<UserManager>();

      services.AddSingleton<RegularTask, CleanLoginKeys>();
      services.AddSingleton<RegularTask, CancelVerifications>();
      services.AddHostedService<Scheduler>();

      services.AddHttpContextAccessor();
      services.AddControllers();
      services.AddRouting(options => {});
    }

    private void RouteControllers(IEndpointRouteBuilder endpoints)
    {
      endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller}/{action}/{id?}"
      );
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
      if (env.IsDevelopment())
      {
        app.UseDeveloperExceptionPage();
      }

      app.UseRouting();

      app.UseAuthentication();
      app.UseAuthorization();

      app.UseEndpoints(endpoints => RouteControllers(endpoints));
    }
  }
}

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

namespace tes3mp_verifier
{
  public class Startup
  {
    private IConfiguration Configuration;

    public Startup(IConfiguration configuration)
    {
      Configuration = configuration;
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
        options.UseNpgsql(Configuration.GetConnectionString("VerifierContext"));
      });

      ConfigureAuthentication(services);

      services.AddSingleton<PasswordHasher, BcryptPasswordHasher>();
      services.AddSingleton<PhoneVerifier, VonagePhoneVerifier>();
      services.AddSingleton<ApiKeyGenerator>();
      services.AddSingleton<LoginKeyGenerator>();
      services.AddScoped<UserManager>();

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

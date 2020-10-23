using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using tes3mp_verifier.Data;
using tes3mp_verifier.Services;

namespace tes3mp_verifier
{
  public class Startup
  {
    private IConfiguration Configuration;

    public Startup(IConfiguration configuration)
    {
      Configuration = configuration;
    }

    public void ConfigureServices(IServiceCollection services)
    {
      services.AddDbContext<VerifierContext>(options => {
        options.UseNpgsql(Configuration.GetConnectionString("VerifierContext"));
      });

      services.AddSingleton<IPasswordHasher, BcryptPasswordHasher>();

      services.AddControllers();
      services.AddRouting(options => {});
    }

    private void RouteControllers(IEndpointRouteBuilder endpoints)
    {
      endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}"
      );
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
      if (env.IsDevelopment())
      {
        app.UseDeveloperExceptionPage();
      }

      app.UseRouting();

      app.UseEndpoints(endpoints => RouteControllers(endpoints));
    }
  }
}

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Threading.Tasks;
using tes3mp_verifier.Data;
using tes3mp_verifier.Data.Models;

namespace tes3mp_verifier.Scheduling.Tasks
{
  public class CleanLoginKeys : RegularTask
  {
    public TimeSpan Interval()
    {
      return LoginKey.VALID_FOR * 2;
    }

    public async Task Run(IServiceProvider services)
    {
      VerifierContext context = services.GetService<VerifierContext>();
      var staleTime = DateTime.Now - LoginKey.VALID_FOR;
      var staleKeys = await context.LoginKeys
        .Where(s => s.Created < staleTime)
        .ToListAsync();
      context.LoginKeys.RemoveRange(staleKeys);
      await context.SaveChangesAsync();
    }
  }
}

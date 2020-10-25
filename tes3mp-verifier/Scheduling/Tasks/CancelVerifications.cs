using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Threading.Tasks;
using tes3mp_verifier.Data;
using tes3mp_verifier.Data.Models;
using tes3mp_verifier.Services;

namespace tes3mp_verifier.Scheduling.Tasks
{
  public class CancelVerifications : RegularTask
  {
    public TimeSpan Interval()
    {
      return TimeSpan.FromSeconds(1);
    }

    public async Task Run(IServiceProvider services)
    {
      VerifierContext context = services.GetService<VerifierContext>();
      PhoneVerifier verifier = services.GetService<PhoneVerifier>();

      var staleTime = DateTime.Now - Verification.VALID_FOR;
      var staleVerifications = await context.Verifications
        .Where(s => s.Confirmed == null)
        .Where(s => s.Created < staleTime)
        .ToListAsync();

      foreach (var verification in staleVerifications)
      {
        try
        {
          verifier.Cancel(verification.Code);
        }
        catch (Exception) { }
      }
        

      context.Verifications.RemoveRange(staleVerifications);
      await context.SaveChangesAsync();
    }
  }
}

using System;
using System.Threading.Tasks;

namespace tes3mp_verifier.Scheduling
{
  public interface RegularTask
  {
    public TimeSpan Interval();
    public Task Run(IServiceProvider services);
  }
}

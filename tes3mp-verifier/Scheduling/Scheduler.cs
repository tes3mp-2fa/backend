using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace tes3mp_verifier.Scheduling
{
  public class Scheduler : IHostedService, IDisposable
  {
    private readonly IServiceProvider _services;
    private readonly IEnumerable<RegularTask> _regularTasks;

    private List<Timer> timers = new List<Timer>();

    public Scheduler(IServiceProvider services, IEnumerable<RegularTask> regularTasks)
    {
      _services = services;
      _regularTasks = regularTasks;
    }

    private void AddTask(RegularTask task) {
      var interval = task.Interval();
      var timer = new Timer(Process, task, interval, interval);
      timers.Add(timer);
    }

    private async void Process(object state)
    {
      RegularTask regularTask = (RegularTask) state;
      var task = regularTask.Run(_services.CreateScope().ServiceProvider);
      await task;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
      foreach (var regularTask in _regularTasks) AddTask(regularTask);
      return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
      foreach (var timer in timers)
      {
        timer.Dispose();
      }
      return Task.CompletedTask;
    }

    public void Dispose()
    {
      foreach(var timer in timers)
      {
        timer.Dispose();
      }
    }
  }
}

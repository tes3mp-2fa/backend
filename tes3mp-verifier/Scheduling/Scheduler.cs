using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;

namespace tes3mp_verifier.Scheduling
{
  using Timer = System.Timers.Timer;

  class PriorityQueue<V, R> where R: IComparable
  {
    private List<Tuple<V, R>> list = new List<Tuple<V, R>>();

    public void Enqueue(V v, R r)
    {
      var tuple = new Tuple<V, R>(v, r);
      list.Add(tuple);
      list.Sort((a, b) => b.Item2.CompareTo(a.Item2));
    }

    public V Dequeue()
    {
      if (list.Count == 0) return default;
      var item = list[list.Count - 1];
      list.RemoveAt(list.Count - 1);
      return item.Item1;
    }

    public R HighestPriority()
    {
      if (list.Count == 0) return default;
      var item = list[list.Count - 1];
      return item.Item2;
    }
  }
  

  public class Scheduler : IHostedService, IDisposable
  {
    private readonly IServiceProvider _services;
    private readonly IEnumerable<RegularTask> _regularTasks;

    private PriorityQueue<RegularTask, DateTime> queue = new PriorityQueue<RegularTask, DateTime>();
    private DateTime? nextCheck;
    private Timer timer;

    public Scheduler(IServiceProvider services, IEnumerable<RegularTask> regularTasks)
    {
      _services = services;
      _regularTasks = regularTasks;
    }

    private void AddTask(RegularTask task) {
      queue.Enqueue(task, DateTime.Now + task.Interval());
      var newCheck = queue.HighestPriority();
      if (nextCheck == null || nextCheck > newCheck)
      {
        nextCheck = newCheck;
        if (timer != null) timer.Stop();
        var interval = Math.Max(0, (newCheck - DateTime.Now).TotalMilliseconds);
        timer = new Timer(interval);
        timer.Elapsed += Process;
        timer.Start();
      }
    }

    private async void Process(object sender, ElapsedEventArgs e)
    {
      timer.Stop();
      nextCheck = null;
      var regularTask = queue.Dequeue();
      if (regularTask == null) throw new NullReferenceException();
      var task = regularTask.Run(_services.CreateScope().ServiceProvider);
      await task;
      AddTask(regularTask);
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
      foreach (var regularTask in _regularTasks)
        AddTask(regularTask);
      return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
      timer.Stop();
      return Task.CompletedTask;
    }

    public void Dispose()
    {
      timer.Dispose();
    }
  }
}

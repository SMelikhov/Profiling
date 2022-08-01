# Profiling
Sampling and tracing profiler


Code Examples

Profile thread

var tracker = new StackTracker(new Stacks(100, 20, true, 10000));
 
 
var thread = new Thread(() =>
{
  using (tracker.RegisterThread(Thread.CurrentThread))
  {
....
  }
});


Profile UI thread


_latency = new UILatencyManager(this, false, new SettingsManagerProfilingStub(), Thread.CurrentThread); //service app lifetime
...
_latency.Start(new ProfilingSettings
{
CollectLatency = true,
StartCollectingBeforeThreshold = true,
LatencySnapshotPeriod = 20,
LatencyThreshold = 100,
ProfilingLevel = ProfilingLevel.ProfilingStacksOnly
 
 
});
.....
_latency.GetStacks()

# Profiling
***Sampling and tracing profiler***

Main purpose of that profiling is to see what is going on in UI thread. 
Also stacks could be collected from any thread.
It can can collect this information from Production.

***Restrictions: .Net 4.7***

***Code Examples***

____

***Profile thread***

```C#
var tracker = new StackTracker(new Stacks(100, 20, true, 10000));
 
 
var thread = new Thread(() =>
{
  using (tracker.RegisterThread(Thread.CurrentThread))
  {
....
  }
});
```
____

***Profile UI thread***

```C#
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
```

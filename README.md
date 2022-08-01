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

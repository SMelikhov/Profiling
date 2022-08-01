using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Profiling.Common.Tracing;
using Profiling.Common.Util;

namespace Profiling.Common.Sampling
{
	public sealed partial class Stacks : IDisposable
	{
		private readonly IProfilingSpecific _logger = ProfilingSpecificProvider.GetProfilingSpecific();

		private readonly ConcurrentDictionary<Thread, StackEntry> _logEntries = new ConcurrentDictionary<Thread, StackEntry>();
		
		private readonly ManualResetEventSlim _stopEvent = new ManualResetEventSlim(false);
		private readonly WaitHandle[] _waitHandles;
		private readonly int _period;
		private volatile bool _postponed;
		
		private readonly long _threshold;
		private readonly bool _collectAllStacks;
		private readonly long _frozenThreshold;
		private readonly CancellationTokenSource _cts = new CancellationTokenSource();
		private readonly Task _task;
		private bool _isDisposed = false;

		public Stacks(int threshold, int period, bool collectAllStacks, int frozenThreshold)
		{
			_waitHandles = new[] { _stopEvent.WaitHandle };
			_period = period;

			_threshold = threshold;
			_collectAllStacks = collectAllStacks;
			_frozenThreshold = frozenThreshold;

			if (_logger.StubMode())
			{
				_logger.Error(string.Format("Profiling is ON. Threshold = {0}, Period = {1}, CollectAllStacks = {2}, Stack = {3}",
					threshold, period, collectAllStacks, new StackTrace()));

				return;
			}

			_task = Task.Factory.StartNew(WaitForThreshold, _cts.Token, TaskCreationOptions.LongRunning, TaskScheduler.Default);
		}

		public ILogEntry CreateLogEntry(Thread thread)
		{
			if (_postponed) 
				return new LogEntryStab();

			StackEntry stackEntry;
			if (_logEntries.TryGetValue(thread, out stackEntry))
			{
				stackEntry.Increment();
				return stackEntry.LogEntry;
			}

			stackEntry = new StackEntry(new LogEntry(), _threshold, _period);
			_logEntries[thread] = stackEntry;

			return stackEntry.LogEntry;
		}

		public bool TryRemoveLogEntry(Thread thread, IEnumerable<string> tags, out StackItem item)
		{
			StackEntry old;
			if (!_logEntries.TryGetValue(thread, out old))
			{
				item = null;
				return false;
			}

			old.Decrement();
			if (old.CanRemove())
			{			
				item = old.CreateStackItem(tags);
				_logEntries.TryRemove(thread, out old);
				return item != null;
			}

			item = null;
			return false;
		}

		public bool TryGetFrozenLogEntry(Thread thread, out StackItem item)
		{
			StackEntry old;
			if (!_logEntries.TryGetValue(thread, out old))
			{
				item = null;
				return false;
			}

			item = old.GetFrozenStackItem(_frozenThreshold);
			return item != null;
		}

		public void AddTag(Thread thread, string tag)
		{
			StackEntry entry;
			if (_logEntries.TryGetValue(thread, out entry))
			{
				entry.AddTag(tag);
			}
		}

		public IDisposable Postpone()
		{
			_postponed = true;

			return new DisposeAction(() => { _postponed = false; });
		}

		public void Dispose()
		{
			if (_isDisposed)
				return;

			try
			{
				_stopEvent.Set();
				_logEntries.Clear();

				if (!_task.IsCompleted)
					_cts.Cancel();

				_task.Dispose();
				_cts.Dispose();
			}
			catch
			{
				// ignored
			}

			_isDisposed = true;
		}

		private void WaitForThreshold()
		{
			while (!_cts.IsCancellationRequested)
			{
				int eventIndex = WaitHandle.WaitAny(_waitHandles, _period, false);

				if (eventIndex != WaitHandle.WaitTimeout)
				{
					if (_waitHandles[eventIndex] == _stopEvent.WaitHandle)
						return;
				}
				if (/*Debugger.IsAttached ||*/ _postponed)
				{
					// do not disturb us while we are debugging!
					continue;
				}

				AddStackIfRequired();
			}
		}

		public void AddStackIfRequired(bool needEmptyStack = false)
		{
			if(!_logEntries.Any())
				return;

			var entries = _logEntries.ToArray();

			foreach (var entry in entries)
			{
				if (_collectAllStacks || (entry.Value.LogEntry.ElapsedMilliseconds > _threshold))
				{
					AddStack(entry, needEmptyStack);
				}				
			}
		}

		public List<Thread> GetRegisteredThreads()
		{
			return _logEntries.Select(c => c.Key).ToList();
		} 

		private void AddStack(KeyValuePair<Thread, StackEntry> kvp, bool needEmptyStack)
		{
			var stack = GetStackTrace(kvp.Key, _stopEvent);
			if (stack != null)
			{
				var str = stack.ToString();
				kvp.Value.AddStack(str);
			}
			else 
			{
				if (needEmptyStack)
					kvp.Value.AddStack("Empty stack" + Guid.NewGuid().ToString());
				else
				{
					kvp.Value.ResetElapsed();
					kvp.Value.FailedToCollectStack();
				}
			}
		}

		public static StackTrace GetStackTrace(Thread targetThread, ManualResetEventSlim stopEvent)
		{
			StackTrace stackTrace = null;
			var resumeEvent = new ManualResetEventSlim(false);
			var terminateResumeThreadEvent = new ManualResetEventSlim(false);
			var handles = new [] { terminateResumeThreadEvent.WaitHandle, stopEvent.WaitHandle};

			ThreadPool.QueueUserWorkItem(
				(x) =>
				{
					// Backstop to release thread in case of deadlock:
					resumeEvent.Set();

					int eventIndex = WaitHandle.WaitAny(handles, 200, false);

					if (eventIndex != WaitHandle.WaitTimeout)
					{
						if (handles[eventIndex] == terminateResumeThreadEvent.WaitHandle)
							return;

						//do not return on stopEvent
					}

#pragma warning disable 612,618
					try
					{
						targetThread.Resume();
					}
#pragma warning restore 612,618
					catch
					{
					}
				});

			resumeEvent.Wait();
#pragma warning disable 612,618
			try { targetThread.Suspend(); }
			catch { /* Deadlock */ }
#pragma warning restore 612,618
#pragma warning disable 612,618
			try { stackTrace = new StackTrace(targetThread, true); }
#pragma warning restore 612,618
			catch { /* Deadlock */ }
			finally
			{
#pragma warning disable 612,618
				try
				{
					targetThread.Resume();
					terminateResumeThreadEvent.Set();
				}
#pragma warning restore 612,618
				catch { stackTrace = null;  /* Deadlock */  }
			}

			return stackTrace;
		}
	}
}

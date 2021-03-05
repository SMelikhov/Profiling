using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Profiling.Common.Util;

namespace Profiling.Common.Sampling
{
	public class StackTracker : IStackTracker
	{
		public const int MaxStacksLength = 2000;
		private readonly ConcurrentQueue<ProfilingStack> _stacks = new ConcurrentQueue<ProfilingStack>();
		private readonly ConcurrentQueue<StackItem> _toApply = new ConcurrentQueue<StackItem>();
		private readonly Stacks _stack;
		private readonly Job _job;
		private readonly object _synch = new object();

		public StackTracker(Stacks stacks)
		{
			_stack = stacks;
	
			_job = new Job(Parse){ Enabled = true };
		}

		public ILogEntry CreateLogEntry(Thread thread)
		{
			return _stack.CreateLogEntry(thread);
		}

		public bool RemoveLogEntry(Thread thread, IEnumerable<string> tags = null)
		{
			StackItem item;
			if (_stack.TryRemoveLogEntry(thread, tags, out item))
			{
				_toApply.Enqueue(item);
				_job.PerformRecalculation();
				return true;
			}
			return false;
		}

		public bool TryGetFrozenLogEntry(Thread thread, out ProfilingStack stack)
		{
			StackItem item;
			if (_stack.TryGetFrozenLogEntry(thread, out item))
			{		
				var aggregator = new ProfilingStackAggregator(item.Stacks, item.Elapsed);
				var additionalInfo = GetAdditionalInfo(item);
				stack = new ProfilingStack(item.Elapsed, DateTime.UtcNow, aggregator.GetRoots(), StackType.Frozen, additionalInfo);			
				return true;
			}
			stack = null;
			return false;
		}

		public IDisposable RegisterThread(Thread thread)
		{
			CreateLogEntry(thread);

			return new DisposeAction(() => RemoveLogEntry(thread));
		}

		public void AddTag(Thread thread, string tag)
		{
			_stack.AddTag(thread, tag);
		}

		public ProfilingStack[] GetStacks()
		{
			return _stacks.ToArray();
		}

		public bool IsFake 
		{
			get { return false; }
		}

		public IDisposable Postpone()
		{
			return _stack.Postpone();
		}

		public void Dispose()
		{
			_stack?.Dispose();
		}

		public void Parse()
		{
			StackItem item;
			lock (_synch)
			{
				while (_toApply.TryDequeue(out item))
				{
					var aggregator = new ProfilingStackAggregator(item.Stacks, item.Elapsed);

					var additionalInfo = GetAdditionalInfo(item);

					_stacks.Enqueue(new ProfilingStack(item.Elapsed, DateTime.UtcNow, aggregator.GetRoots(), StackType.Normal,
						additionalInfo));
				}
			}
			ReduceSize();
		}

		public void CopyStacks(IStackTracker stackTracker)
		{
			stackTracker.Parse();
			foreach (var stack in stackTracker.GetStacks())
			{
				_stacks.Enqueue(stack);
			}		
		}

		public Stacks Stacks
		{
			get { return _stack; }
		}

		public List<Thread> GetRegisteredThreads()
		{
			return _stack.GetRegisteredThreads();
		}

		private void ReduceSize()
		{
			if (_stacks.Count > MaxStacksLength)
			{
				var sorted = _stacks.ToArray().OrderByDescending(c => c.ElapsedMilliseconds);
				ProfilingStack stack;
				while(_stacks.TryDequeue(out stack)){ }
				foreach (var profilingStack in sorted.Take(MaxStacksLength / 2))
				{
					_stacks.Enqueue(profilingStack);
				}	
			}
		}

		private static string GetAdditionalInfo(StackItem item)
		{
			var additionalInfo = String.Empty;
			if (item.GcDif != null)
			{
				additionalInfo = string.Format("GC Collect: {0}{1}",
					item.GcDif.GetGCString(),
					Environment.NewLine);
			}

			if (item.Pauses != null && item.Pauses.Any())
			{
				additionalInfo += string.Format("Pauses: {0}. Total Sum pauses: {1}{2}",
					string.Join(",", 
					item.Pauses
					.OrderByDescending(c => c)
					.Take(10)
					.Select(c => c.ToString())
					.ToArray()),
					item.Pauses.Sum(),
					Environment.NewLine);
			}

			if (item.Tags != null && item.Tags.Any())
			{
				additionalInfo += string.Format("Tags: {0}{1}",
					string.Join(",", item.Tags.Distinct()),
					Environment.NewLine);
			}

			if (item.FailedStacks != 0)
			{
				additionalInfo += string.Format("Failed Stacks: {0}{1}",
					item.FailedStacks,
					Environment.NewLine);
			}

			if (item.AvailableMemory != 0)
			{
				additionalInfo += string.Format("Available Memory: {0} MB{1}",
					item.AvailableMemory,
					Environment.NewLine);
			}

			return additionalInfo;
		}
	}


}

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Profiling.Common.InstanceData;
using Profiling.Common.Tracing;

namespace Profiling.Common.Sampling
{
	partial class Stacks
	{
		sealed class StackEntry
		{
			private const int MaxElapsed = 30 * 1000;

			private long _stackCount = 1;
			private readonly ILogEntry _logEntry;
			private readonly long _threshold;
			private readonly int _periodToCapture;
			private readonly ConcurrentQueue<string> _currentStack = new ConcurrentQueue<string>();
			private readonly ConcurrentQueue<long> _pauses = new ConcurrentQueue<long>();
			private readonly ConcurrentQueue<string> _tags = new ConcurrentQueue<string>();
			private long _failedStacks = 0;

			private long _lastEllapsed;
			private readonly GcItem _firstGcItem;

			public StackEntry(ILogEntry logEntry, long threshold, int period)
			{
				_logEntry = logEntry;
				_threshold = threshold;
				_periodToCapture = period * 5;
				_firstGcItem = new GcItem();

				ResetElapsed();
			}

			public ILogEntry LogEntry
			{
				get { return _logEntry; }
			}

			public void AddStack(string stack)
			{
				var diff = _logEntry.ElapsedMilliseconds - _lastEllapsed;
				if (diff > _periodToCapture)
				{
					_pauses.Enqueue(diff);
				}

				ResetElapsed();
				_currentStack.Enqueue(stack);
			}

			public void ResetElapsed()
			{
				_lastEllapsed = _logEntry.ElapsedMilliseconds;				
			}

			public void FailedToCollectStack()
			{
				Interlocked.Increment(ref _failedStacks);		
			}

			public void Increment()
			{
				Interlocked.Increment(ref _stackCount);
			}

			public void Decrement()
			{
				Interlocked.Decrement(ref _stackCount);
			}

			public bool CanRemove()
			{
				return Interlocked.Read(ref _stackCount)<= 0;
			}

			public void AddTag(string tag)
			{
				_tags.Enqueue(tag);
			}

			public StackItem CreateStackItem(IEnumerable<string> uiTags)
			{
				_logEntry.Stop();
				if (_logEntry.ElapsedMilliseconds < _threshold)
					return null;

				var pauses = new List<long>();
				long pause;
				while (_pauses.TryDequeue(out pause))
					pauses.Add(pause);
							
				var stacks = new List<string>();
				string result;
				while (_currentStack.TryDequeue(out result))
					stacks.Add(result);

				var tags = new List<string>();
				while (_tags.TryDequeue(out result))
					tags.Add(result);

				if(uiTags != null)
					tags.AddRange(uiTags);

				if (!stacks.Any())
					return null;

				var gcDif = _firstGcItem.CreateDif();
				var availableMemory = GetAvailableMemory(gcDif, _logEntry.ElapsedMilliseconds);
				return new StackItem(_logEntry.ElapsedMilliseconds, _failedStacks, availableMemory, stacks.ToArray(), gcDif, pauses, tags.ToList());
			}

			private static float GetAvailableMemory(GcItem gcDif, long elapsed)
			{
				float availableMemory = 0;

				if (gcDif.NeedCollectAvailableMemory() || elapsed > MaxElapsed)
				{
					availableMemory = MemoryProfiling.GetAvailable().Available;
				}
				return availableMemory;
			}

			public StackItem GetFrozenStackItem(long frozenThreshold)
			{
				if (_logEntry.ElapsedMilliseconds < frozenThreshold)
					return null;

				var stacks = _currentStack.ToList();
				var pauses = _pauses.ToList();
				var tags = _tags.ToList();

				if (!stacks.Any())
					return null;

				var gcDif = _firstGcItem.CreateDif();
				var availableMemory = GetAvailableMemory(gcDif, _logEntry.ElapsedMilliseconds);
				tags.AddRange(ProfilingSpecificProvider.GetProfilingSpecific().GetAdditionalTagsForStacks());
				return new StackItem(_logEntry.ElapsedMilliseconds, _failedStacks, availableMemory, stacks.ToArray(), gcDif, pauses, tags);
			}	
		}
	}
}
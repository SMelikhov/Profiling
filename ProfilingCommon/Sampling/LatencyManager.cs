using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Profiling.Common.DTO;
using Profiling.Common.Tracing;

namespace Profiling.Common.Sampling
{
	public class LatencyManager : ILatencyManager
	{
		private volatile IStackTracker _tracker = new StackTrackerStub();
		private readonly IProfilingSpecific _profilingSpecific = ProfilingSpecificProvider.GetProfilingSpecific();

		public void Start(ProfilingSettings setting)
		{
			DisposeLatency();
			ReloadTracker(setting);
		}

		public void Stop()
		{
			DisposeLatency();
		}

		public ProfilingLatencyDTO GetFrozenAggregatedItem()
		{
			var threads = _tracker.GetRegisteredThreads();
			if (threads != null && threads.Any())
			{
				var list = new List<ProfilingStack>();

				foreach (var thread in threads)
				{
					ProfilingStack stack;
					if (_tracker.TryGetFrozenLogEntry(thread, out stack))
					{
						list.Add(stack);
					}
				}

				if (list.Any())
				{
					var item = new ProfilingLatencyDTO
					{
						Stacks =
							list.Select(stack => new ProfilingStackDTO
							{
								ElapsedMilliseconds = stack.ElapsedMilliseconds,
								AdditionalInfo = stack.AdditionalInfo,
								CreateDate = stack.CreateDate,
								StackType = (int) stack.StackType,
								Stacks = stack.Stacks.Select(i => i.ToDto()).ToArray()
							}).ToArray()
					};

					return item;
				}
			}
			return null;			
		}

		public IDisposable RegisterThread()
		{
			return _tracker.RegisterThread(Thread.CurrentThread);
		}

		public void AddTag(string tag)
		{
			_tracker.AddTag(Thread.CurrentThread, tag);
		}

		public void CopyStacks(IStackTracker stackTracker)
		{
			_tracker.CopyStacks(stackTracker);
		}

		public ProfilingLatencyDTO GetAggregatedItem()
		{
			var item = new ProfilingLatencyDTO
			{
				Stacks = _tracker.GetStacks()
				.Select(c => new ProfilingStackDTO
				{
					ElapsedMilliseconds = c.ElapsedMilliseconds,
					AdditionalInfo = c.AdditionalInfo,
					CreateDate = c.CreateDate,
					StackType = (int)c.StackType,
					Stacks = c.Stacks.Select(i => i.ToDto()).ToArray()
				})
				.ToArray()
			};

			return item;
		}

		public ProfilingStack[] GetStacks()
		{
			return _tracker.GetStacks();
		}

		public IDisposable Postpone()
		{
			return _tracker.Postpone();
		}

		public LatencyIntervalDTO GetLatencyInterval()
		{
			return null;
		}

		public List<Tuple<DateTime, int>> GetLatencyThresholds()
		{
			return null;
		}

		public void Dispose()
		{
			DisposeLatency();
		}

		public IStackTracker StackTracker
		{
			get { return _tracker; }
		}

		private void DisposeLatency()
		{
			AssignNewTraker(new StackTrackerStub());	
		}

		private void ReloadTracker(ProfilingSettings settings)
		{
			if (settings.CollectLatency && _tracker.IsFake && _profilingSpecific.CanCollectStacks(settings))
				AssignNewTraker(new StackTracker(new Stacks(settings.LatencyThreshold, settings.LatencySnapshotPeriod, 
					settings.StartCollectingBeforeThreshold, settings.UIFreezeThreshold)));			
			else
				AssignNewTraker(new StackTrackerStub());			
		}

		private void AssignNewTraker(IStackTracker tracker)
		{
			_tracker?.Dispose();
			_tracker = tracker;
		}
	}
}
